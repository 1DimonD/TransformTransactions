using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformTransactions.Extractors
{
    internal class FileDataExtractor : IDataExtractor
    {

        public FileDataExtractor() { }
        public FileDataExtractor(string filePath) 
        {
            _filePath = filePath;
        }   

        private string? _filePath;
        public void SetFilePath(string filePath) => _filePath = filePath;

        public List<string> ExtractData()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                throw new InvalidOperationException("The FilePath property must be set before calling ExtractData().");
            }

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"The file {_filePath} could not be found.");
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
