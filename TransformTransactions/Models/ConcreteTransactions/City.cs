using TransformTransactions.Models;

namespace TransformTransactions.Models.ConcreteTransactions
{
    internal class City : IModel
    {
        public string city { get; set; } = "";
        public List<Service> services { get; set; } = new List<Service>();
        public decimal total { get; set; }
    }
}
