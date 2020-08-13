﻿using FluentAssertions;
using Stankins.Excel;
using Stankins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbehave;
using Xunit;

namespace StankinsTestXUnit
{
    [Trait("ReceiverExcel", "")]
    [Trait("ExternalDependency", "0")]
    public class TestReceiverExcel
    {
        [Scenario]
        //[Example("Assets/Excel/newempty.xlsx", 2, "Sheet1", 1, 1)]
        [Example("Assets/Excel/ProgrammingTools2018.xlsx", 2, "ProgrammingTools2018",9,8)]
        public void TestSimpleExcel(string fileName, int NumberSheets,string verifySheet,int nrCols, int nrRows)
        {
            IReceive receiver = null;
            IDataToSent data = null;
            $"When I create the receiver Excel for the {fileName}".w(() => receiver = new ReceiverExcel(fileName));
            $"And I read the data".w(async () => data = await receiver.TransformData(null));
            $"Then should be a data".w(() => data.Should().NotBeNull());
            $"With some tables".w(() =>
            {
                data.DataToBeSentFurther.Should().NotBeNull();                
            });
            $"The number of sheets should be {NumberSheets}".w(() => data.DataToBeSentFurther.Count.Should().Be(NumberSheets+1));
            $"and verification for first sheet ".w(() =>
            {
                var firstTable = data.FindAfterName(verifySheet).Value;
                firstTable.Columns.Count.Should().Be(nrCols);
                firstTable.Rows.Count.Should().Be(nrRows);

                //firstTable.Rows[0].ItemArray[0].Should().Be("Browsers");
                //firstTable.Rows[0].ItemArray[7].Should().Be(1);
                //firstTable.Rows[7].ItemArray[0].Should().Be("Communications");
                //firstTable.Rows[7].ItemArray[7].Should().Be(5);

            });


        }
    }
}
