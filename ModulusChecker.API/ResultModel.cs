using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModulsChecker.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ModulusChecker.API
{
    public class ResultModel
    {
        public bool CanValidate { get; set; }
        public bool PassedValidation { get; set; }
        public string ResultMessage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ModulusCheckingAlgorithm FirstAlgorithm { get; set; }
        public bool FirstCheckIsException4 { get; set; }
        public bool FirstCheckIsException7 { get; set; }
        public bool PassedFirstCheck { get; set; }
        public bool RequiresSecondCheck { get; set; }
        public bool PassedSecondCheck { get; set; }            
        public bool SecondCheckIsException4 { get; set; }        
        public bool SecondCheckIsException7 { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ModulusCheckingAlgorithm SecondAlgorithm { get; set; }
        
    }
}
