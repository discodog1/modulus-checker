using ModulsChecker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModulusChecker.Services.Interfaces
{
   public interface IWeightMappingImporter
   {
       List<WeightMapping> GetWeightMappings(string sourceFile);
   }
}
