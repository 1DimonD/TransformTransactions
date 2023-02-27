using TransformTransactions.Models;

namespace TransformTransactions.Models.Validators
{
    internal class CityValidator : IValidator
    {
        private long _totalInvalidFiles = 0;
        public long TotalInvalidFiles { get => _totalInvalidFiles; }

        private long _totalInvalidLines = 0;
        public long TotalInvalidLines { get => _totalInvalidLines; }

        public bool IsValid(List<string> values)
        {
            if(
                values.Count != 7 || !values[0].All(x => Char.IsLetter(x)) || !values[1].All(x => Char.IsLetter(x)) ||
                !values[2].All(x => Char.IsLetter(x)) || !Decimal.TryParse(values[3], out _) || !DateTime.TryParse(values[4], out _) ||
                !Int64.TryParse(values[5], out _) || !values[6].All(x => Char.IsLetter(x))
                )
            {
                _totalInvalidFiles++;
                _totalInvalidLines++;
                return false;
            }

            return true;
        }
    }
}
