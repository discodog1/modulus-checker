using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModulsChecker.Models;

namespace ModulusChecker.API
{
    public static class ExtensionMethods
    {
        public static string ToCleanString(this string input)
        {
            return input.Replace("-", "");
        }

        public static List<WeightMapping> BySortCode(this List<WeightMapping> list ,SortCode sortcode)
        {
            return list.Where(x => MappingContainsSortCode(x, sortcode)).ToList();
        }

        private static bool MappingContainsSortCode(WeightMapping row, SortCode sortcode)
        {
            var doubleIn = sortcode.SortCodeAsDouble;
            var doubleStart = row.SortCodeStart.SortCodeAsDouble;
            var doubleEnd = row.SortCodeEnd.SortCodeAsDouble;
            return doubleIn >= doubleStart && doubleIn <= doubleEnd;
        }


    }
}
