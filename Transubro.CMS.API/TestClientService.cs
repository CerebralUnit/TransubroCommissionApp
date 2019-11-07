using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transubro.CMS.Model;

namespace Transubro.CMS.API
{
    public class TestClientService : IClientService
    {
        private List<Client> mockClients = new List<Client>()
        { 
            //new Client()
            //{
            //    Name = "41",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "3K",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "ACE",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 4, 15),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "ATL",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "AMB",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 8, 28),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-4", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "BT",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 3, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "CBT",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 5, 15),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.12m },
            //        { "TS-3", 0.05m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "CH",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "CIC",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 2, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.25m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "DC",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 10, 8),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "DEL",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 8, 21)
            //},
            //new Client()
            //{
            //    Name = "DJ",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "DM",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.5m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "DT",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.5m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "FLB",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 3, 26)
            //},
            //new Client()
            //{
            //    Name = "FO",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.5m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "GMC",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 4, 12),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-2", 0.15m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "GVC",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 12, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-8", 0.15m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "HRI",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 9, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "ICB",
            //     TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "ICC",
            //     TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "JO",
            //     TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "JSB",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 4, 26),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "JT",
            //     TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "JTS",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 7, 15),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "KBL",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 2, 2),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.125m },
            //        { "TS-1", 0.125m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "KCT",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "KLS",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 3, 27),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "LB",
            //     TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "LL",
            //     TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "LMI",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 1, 25),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.25m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "MLS",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 9, 27),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "MPC",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 10, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-8", 0.15m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "MTW",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 10, 21),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.25m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "MV",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "NC",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "NEX",
            //    TransubroPercentageOld = 0.6m,
            //    TransubroPercentageNew = 0.7m,
            //    ThresholdDate = new DateTime(2018, 11, 20),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.2m },
            //        { "TS-9", 0.05m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "P",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 5, 22),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "PAN",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 2, 1)
            //},
            //new Client()
            //{
            //    Name = "PRH",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 7, 9),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "PW",
            //        TransubroPercentageOld = 0.666666m,
            //            TransubroPercentageNew = 0.666666m,
            //            ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "QB",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2016, 10, 26)
            //},
            //new Client()
            //{
            //    Name = "QLL",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2016, 6, 8)
            //},
            //new Client()
            //{
            //    Name = "RAP",
            //        TransubroPercentageOld = 0.666666m,
            //            TransubroPercentageNew = 0.666666m,
            //            ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "RVB",
            //        TransubroPercentageOld = 0.666666m,
            //            TransubroPercentageNew = 0.666666m,
            //            ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "SC",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 4, 12)
            //},
            //new Client()
            //{
            //    Name = "SEL",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 9, 1)
            //},
            //new Client()
            //{
            //    Name = "STA",
            //    TransubroPercentageOld = 0.7m,
            //    TransubroPercentageNew = 0.7m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "STG",
            //        TransubroPercentageOld = 0.666666m,
            //            TransubroPercentageNew = 0.666666m,
            //            ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "STS",
            //    TransubroPercentageOld = 0.75m,
            //    TransubroPercentageNew = 0.75m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "SWF",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 3, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "TA",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 5, 14)
            //},
            //new Client()
            //{
            //    Name = "TH",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "THX",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2019, 1, 1)
            //},
            //new Client()
            //{
            //    Name = "TM",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 4, 15)
            //},
            //new Client()
            //{
            //    Name = "TSE",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 9, 1),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //},
            //new Client()
            //{
            //    Name = "TTG",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 5, 16)
            //},
            //new Client()
            //{
            //    Name = "TW",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2017, 3, 30),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-6", 0.2m } 
            //    }
            //},
            //new Client()
            //{
            //    Name = "VB",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "VCRD",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "VPL",
            //    TransubroPercentageOld = 0.666666m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = null
            //},
            //new Client()
            //{
            //    Name = "WC-L",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 3, 29)
            //},
            //new Client()
            //{
            //    Name = "WB",
            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2016, 1, 12)
            //},
            //new Client()
            //{
            //    Name = "YCC",

            //    TransubroPercentageOld = 0.5m,
            //    TransubroPercentageNew = 0.666666m,
            //    ThresholdDate = new DateTime(2018, 6, 4),
            //    SalespersonSplits = new Dictionary<string, decimal>()
            //    {
            //        { "TS-5", 0.1m },
            //        { "TS-1", 0.15m },
            //    }
            //}

        };

        public List<Client> GetAllClients()
        {
            return mockClients;
        }
    }
}
