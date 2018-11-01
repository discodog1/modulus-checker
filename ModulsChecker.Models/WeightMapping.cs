using System;
using System.Collections.Generic;
using System.Text;
using ModulsChecker.Models.Enums;

namespace ModulsChecker.Models
{
    public class WeightMapping
    {
        public SortCode SortCodeStart { get; set; }
        public SortCode SortCodeEnd { get; set; }
        public ModulusCheckingAlgorithm Algorithm { get; set; }
        public int[] WeightValues { get; set; }
        public int Exception { get; set; }

        public static WeightMapping WeightMappingFromRow(string row)
        {
            var weightValues = new int[14];
            var items = row.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sortCodeStart = new SortCode(items[0]);
            var sortCodeEnd = new SortCode(items[1]);
            var algorithm = (ModulusCheckingAlgorithm)Enum.Parse(typeof(ModulusCheckingAlgorithm), items[2], true);
            for (var i = 3; i < 17; i++)
            {
                weightValues[i - 3] = int.Parse(items[i]);
            }
            var exception = -1;
            if (items.Length == 18)
            {
                exception = int.Parse(items[17]);
            }

            return new WeightMapping
            {
                SortCodeStart=sortCodeStart,
                SortCodeEnd = sortCodeEnd,
                Algorithm = algorithm,
                WeightValues=weightValues,
                Exception = exception
            };
        }
    }
}
