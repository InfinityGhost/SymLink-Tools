using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymLink_Tools.Tools
{
    internal static class StringTools
    {
        public static string Combine(this IEnumerable<string> vs)
        {
            string total = string.Empty;
            foreach (var line in vs)
            {
                if (!string.IsNullOrEmpty(total))
                    total += Environment.NewLine;
                total += line;
            }
            return total;
        }
    }
}
