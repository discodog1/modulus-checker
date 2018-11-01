using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ModulsChecker.Models.Enums;
using Newtonsoft.Json;

namespace ModulsChecker.Models
{
    public class BankAccount
    {   
        public SortCode SortCode { get; set; }    
        public AccountNumber AccountNumber { get; set; }
        public string FullCheckNumber => $"{SortCode.ToString()}{AccountNumber.AccountNumberValue}";
        public bool PassedFirstCheck { get; set; }
        public bool PassedSecondCheck { get; set; }

        public ModulusCheckingAlgorithm FirstCheckAlgorithm => WeightMappings.First().Algorithm;
        public ModulusCheckingAlgorithm SecondCheckAlgorithm => WeightMappings.Count != 1 ? WeightMappings[1].Algorithm : FirstCheckAlgorithm;
        private readonly List<int> _secondCheckExceptions = new List<int> {2,5,9,10,11,12,13,14};

        public int FirstException => WeightMappings.Any() ? WeightMappings.First().Exception : 0;
        public int SecondException => WeightMappings.Count != 1 ? WeightMappings[1].Exception : 0;

        public List<WeightMapping> WeightMappings { get;}
        public BankAccount(SortCode sortCode, AccountNumber accountNumber, List<WeightMapping> mappings)
        {

            SortCode =sortCode;
            AccountNumber =accountNumber;
            WeightMappings = mappings;

            if (IsExceptionSeven())
            {            
                WeightMappings = WeightMappings.Select((map, i) => ZeroiseMappings(map)).ToList();
            }
        }

        /// <summary>
        /// true if more than one weight mapping and the first exception is in a given list
        /// </summary>
        /// <returns></returns>
        public bool RequiresSecondCheck()
        {
            if (PassedFirstCheck)
            {
                return WeightMappings.Count() != 1 || _secondCheckExceptions.Contains(FirstException);
            }
            return _secondCheckExceptions.Contains(FirstException);

        }
        private bool IsExceptionSeven()
        {
            return FirstException == 7 && AccountNumber.CheckExceptionSeven();
        }

        /// <summary>
        /// checks if any weight mappings exist for sortcode
        /// </summary>
        /// <returns></returns>
        public bool CanValidate()
        {
            return WeightMappings.Any();
        }

        /// <summary>
        /// Zeroise weight mapping for exception 7
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        private  WeightMapping ZeroiseMappings(WeightMapping mapping)
        {
            var zeroised = new WeightMapping
            {
                SortCodeStart = mapping.SortCodeStart,
                SortCodeEnd = mapping.SortCodeEnd,
                Exception = mapping.Exception,
                Algorithm = mapping.Algorithm,
                WeightValues = mapping.WeightValues.Select((wv, index) => index < 8 ? 0 : wv).ToArray()
            };

            return zeroised;
        }
    }
}
