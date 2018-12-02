﻿using OpenMcdf;
using Stankins.Interfaces;
using StankinsCommon;
using StankinsObjects;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stankins.WLW
{
    public class SenderWindowsLiveWriter : BaseObject, ISender
    {
        public SenderWindowsLiveWriter(CtorDictionary dataNeeded) : base(dataNeeded)
        {
            FolderPath = base.GetMyDataOrDefault<string>(nameof(FolderPath),Environment.CurrentDirectory);

        }
        public SenderWindowsLiveWriter(string folderPath) : this(new CtorDictionary()
        {

            { nameof(folderPath),folderPath},
        })
        {

        }
        static byte[] Str(string text)
        {
            return Encoding.Unicode.GetBytes(text);
        }


        public string FolderPath { get; }

        public override async Task<IDataToSent> TransformData(IDataToSent receiveData)
        {
            var nr = receiveData.DataToBeSentFurther.Count;
            var fmt = "#".PadLeft(nr).Replace(" ","0");
            foreach (var item in receiveData.DataToBeSentFurther)
            {
                
                string fileName = item.Key.ToString (fmt) + item.Value.TableName;
                var fullFilePath= Path.Combine(FolderPath, fileName + ".wpost");
                var cf = new CompoundFile(@"a.wpost", CFSUpdateMode.ReadOnly, CFSConfiguration.Default);

                var rs = cf.RootStorage;
                var t = rs.TryGetStream("Title");

                t.SetData(Str(fileName));
                var allData = new StringBuilder();
                var c = rs.TryGetStream("Contents");
                foreach(DataRow dr in item.Value.Rows)
                {
                    var str = string.Join(" ", dr.ItemArray);
                    allData.AppendLine(str);
                }

                c.SetData(Str(allData.ToString()));
                cf.Save(fullFilePath);
                
            }
            return receiveData;
        }

        public override Task<IMetadata> TryLoadMetadata()
        {
            throw new NotImplementedException();
        }
    }
}
