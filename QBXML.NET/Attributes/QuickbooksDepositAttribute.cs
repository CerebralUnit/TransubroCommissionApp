﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class QuickbooksDepositAttribute :Attribute
    { 

        public QuickbooksDepositAttribute()
        {

           
        }
 
    }
}
