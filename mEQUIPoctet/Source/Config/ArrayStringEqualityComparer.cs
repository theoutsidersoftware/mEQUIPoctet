using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Config
{
    /// <summary>
    /// Compares two array of strings for equality. Both arrays are considered equal if every element in the array are
    /// equal.
    /// </summary>
    public class ArrayStringEqualityComparer : IEqualityComparer<string[]>
    {
        public bool Equals(string[] x, string[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(string[] obj)
        {
            unchecked
            {
                int hash = 17;

                foreach (string element in obj)
                {
                    hash = hash * 23 + element.GetHashCode();
                }

                return hash;
            }
        }
    }
}
