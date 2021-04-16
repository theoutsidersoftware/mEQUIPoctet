using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Config
{
    /// <summary>
    /// Compares two string as if they were integers. Falls back to string comparison if they cannot be parsed as
    /// integers.
    /// </summary>
    public class IntegerStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (int.TryParse(x, out int parsedX) && int.TryParse(y, out int parsedY))
            {
                return parsedX - parsedY;
            }

            return x.CompareTo(y);
        }
    }
}
