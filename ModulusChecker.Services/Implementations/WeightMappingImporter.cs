using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulsChecker.Models;
using ModulusChecker.Services.Interfaces;

namespace ModulusChecker.Services.Implementations
{
    public class WeightMappingImporter : IWeightMappingImporter
    {

        private List<WeightMapping> AllMappings { get; set; }

       
        public List<WeightMapping> GetWeightMappings(string sourceFile)
        {
            AllMappings= sourceFile
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Where(row => row.Length > 0)
                .Select(WeightMapping.WeightMappingFromRow)
                .ToList();

            return AllMappings;
        }

         
    }
}
