using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToObject
{
    class MonthComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            DateTime xDate = DateTime.ParseExact(x, "MMMM", new CultureInfo("en-US"));
            DateTime yDate = DateTime.ParseExact(y, "MMMM", new CultureInfo("en-US"));
            return (Comparer<DateTime>.Default.Compare(xDate, yDate));
        }
    }
}
