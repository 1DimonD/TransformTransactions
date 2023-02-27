using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformTransactions.Models.ConcreteTransactions
{
    internal class Service
    {
        public string name { get; set; } = "";
        public List<Payer> payers { get; set; } = new List<Payer>();
        public decimal total { get; set; }
    }
}
