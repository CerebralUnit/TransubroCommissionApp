﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{ 
    public class QuickbooksStatus
    {
        public int Code { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
    }
}
