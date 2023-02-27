using TransformTransactions.Models;

namespace TransformTransactions.Models.Validators
{
    internal interface IValidator
    {
        public long TotalInvalidLines { get; }
        public bool IsValid(List<string> values);
    }
}
