﻿using StankinsInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StanskinsImplementation
{
    public class SyncReceiverMultiple : IReceive
    {
        public string Name { get; set; }
        public IReceive[] Receivers { get; set; }

        public IRowReceive[] valuesRead { get; set; }

        public SyncReceiverMultiple(params IReceive[] receivers)
        {
            this.Receivers = receivers;
        }
        async Task<IRowReceive[]> DataFromReceivers()
        {
            var data = new List<IRowReceive>();
            for (int i = 0; i < Receivers.Length; i++)
            {
                try
                {
                    var item = Receivers[i];
                    await item.LoadData();
                    data.AddRange(item.valuesRead);
                }
                catch(Exception ex)
                {
                    //TODO:log
                }

            }
            return data.ToArray();
        }
        public async Task LoadData()
        {
            valuesRead = await DataFromReceivers();

        }
    }
}