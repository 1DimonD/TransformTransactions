using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformTransactions.Models.ConcreteTransactions
{
    internal class Payer
    {
        public string name { get; set; } = "";
        public decimal payment { get; set; }
        public DateTime date { get; set; }
        public long account_number { get; set; }
    }
}
