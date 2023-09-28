using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geld_Automaat.classes
{
    internal class Transaction
    {
        public DateTime Time { get; set; }
        public decimal Amount { get; set; }
        public int Type { get; set; }
    }
}

