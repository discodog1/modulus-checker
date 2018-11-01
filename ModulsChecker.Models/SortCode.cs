using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ModulsChecker.Models
{
    public class SortCode
    {
        private static  Regex _regex => new Regex("^[0-9]{6}$");
        private readonly string _sortCodeAsString;
        public  double SortCodeAsDouble { get;}

        public override string ToString()
        {
            return _sortCodeAsString;
        }
        public SortCode(string input)
        {
            if (!_regex.IsMatch(input))
            {
                throw new ArgumentException("Provided sort-code is invalid, should be a 6 digit string.");
            }

            _sortCodeAsString = input;
            SortCodeAsDouble=double.Parse(input);
        }
    }
}
