using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformTransactions.Extractors
{
    internal interface IDataExtractor
    {
        public List<string> ExtractData();
    }
}
