﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
    }
}
