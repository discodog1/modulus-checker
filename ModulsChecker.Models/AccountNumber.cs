using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ModulsChecker.Models
{
    public class AccountNumber
    {
        private static  Regex _accountNumberRegex => new Regex("^[0-9]{6,8}$");

        private readonly int[] _accountNumberArray;
        public string AccountNumberValue => string.Join("", _accountNumberArray);
        public AccountNumber(string accountNumber)
        {
            if (!_accountNumberRegex.IsMatch(accountNumber))
            {
                throw new ArgumentException($"Account Number({accountNumber}) should be 6 to 8 characters long.");
            }

            accountNumber = accountNumber.PadLeft(8, '0'); //make standard size
            _accountNumberArray = new int[8];
            for (var index = 0; index < accountNumber.Count(); index++)
            {
                var character = accountNumber[index];
                _accountNumberArray[index] = int.Parse(character.ToString());
            }
        }

        /// <summary>
        /// returns true if the second last character equals 9 (relevant for exception 7)
        /// </summary>
        /// <returns></returns>
        public bool CheckExceptionSeven()
        {
            return _accountNumberArray[6] == 9;
        }

        /// <summary>
        /// Get the last two digits of the AccountNumber for the purposes of exception 4 checking
        /// </summary>
        /// <returns></returns>
        public int CheckDigitsForException4()
        {
            if (_accountNumberArray.Length != 8)
            {
                throw new ArgumentException($"Account Number {_accountNumberArray} invalid");
            }

            if (int.TryParse($"{_accountNumberArray[6]}{_accountNumberArray[7]}", out var result))
            {
                return result;
            }
           throw new ArgumentException("Error determining Exception 4 check digits");
        }
    }
}
