using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformTransactions.Extractors
{
    internal class FileDataExtractor : IDataExtractor
    {
        private string? _filePath;
        public string? FilePath
        {
            get => _filePath;
            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException($"The file {value} could not be found.");
                }
                _filePath = value;
            }
        }

        public List<string> ExtractData()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                throw new InvalidOperationException("The FilePath property must be set before calling ExtractData().");
            }

            List<string> data = new();

            using var reader = new StreamReader(_filePath);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                data.Add(line);
            }

            return data;
        }
    }
}
