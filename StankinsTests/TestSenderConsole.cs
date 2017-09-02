﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SenderAction;
//using SenderCSV;
using SenderToFile;
using StankinsInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StankinsTests
{
    [TestClass]
    public class TestSenderAction
    {
        

        //public TestContext TestContext { get; set; }

        
        [TestMethod]
        public async Task SenderActionGenericRowStringBuilder()
        {
            var dir = AppContext.BaseDirectory;
            #region arange
           
            var rows = new List<IRow>();
            int nrRows = 10;
            for (int i = 0; i < nrRows; i++)
            {
                var rowAndrei = new Mock<IRow>();

                rowAndrei.SetupProperty(it => it.Values,
                    new Dictionary<string, object>()
                    {
                        ["ID"] = i,
                        ["FirstName"] = "Andrei"+i,
                        ["LastName"] = "Ignat"+i
                    }
                );

                rows.Add(rowAndrei.Object);
            }


            #endregion
            #region act
            var send = new SenderStringBuilder();
            send.valuesToBeSent = rows.ToArray();
            await send.Send();
            #endregion
            #region assert
            var data = send.StringWithData.ToString();
            //it adds a bank line
            var lines = data.Split(new[] { Environment.NewLine },StringSplitOptions.None);
            Assert.AreEqual(nrRows, lines.Length);
            
            #endregion
        }
    }
}