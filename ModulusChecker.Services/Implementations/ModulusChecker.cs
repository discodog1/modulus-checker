using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ModulsChecker.Models;
using ModulusChecker.Services.Interfaces;

namespace ModulusChecker.Services.Implementations
{
    public class ModulusChecker : IModulusChecker
    {
        public bool DoubleAlternateCheck(BankAccount details)
        {
            return DoubleAlternateCheckInternal(details, 10);

        }

        public bool FirstStandardElevenCheck(BankAccount details)
        {
            return FirstModulusCheckInternal(details, 11);
        }

        public bool FirstStandardTenCheck(BankAccount details)
        {
            return FirstModulusCheckInternal(details, 10);

        }
        public bool SecondStandardElevenCheck(BankAccount details)
        {
            return SecondModulusCheckInternal(details, 11);
        }

        public bool SecondStandardTenCheck(BankAccount details)
        {
            return SecondModulusCheckInternal(details, 10);
        }

        private bool FirstModulusCheckInternal(BankAccount details, int modulus)
        {
            var value = details.FullCheckNumber;
            if (value.Length != 14)
            {
                throw new FormatException("Sort Code and Account Number combined should equal 14 characters");

            }
            var sum = 0;
            for (var i = 0; i < 14; i++)
            {
                var weightMapping = details.WeightMappings.First();
                sum += (int.Parse(value[i].ToString(CultureInfo.InvariantCulture)) * weightMapping.WeightValues[i]);
            }

            var remainder = sum % modulus;

            if (details.FirstException == 4)
            {
                return remainder == details.AccountNumber.CheckDigitsForException4();
            }

            return remainder == 0;

        }

        private bool SecondModulusCheckInternal(BankAccount details, int modulus)
        {
            var value = details.FullCheckNumber;
            if (value.Length != 14)
            {
                throw new FormatException("Sort Code and Account Number combined should equal 14 characters");

            }
            var sum = 0;
            for (var i = 0; i < 14; i++)
            {
                var weightMapping = details.WeightMappings[1];
                sum += (int.Parse(value[i].ToString(CultureInfo.InvariantCulture)) * weightMapping.WeightValues[i]);
            }

            var remainder = sum % modulus;

            if (details.SecondException == 4)
            {
                return remainder == details.AccountNumber.CheckDigitsForException4();
            }

            return remainder == 0;

        }

        private bool DoubleAlternateCheckInternal(BankAccount details, int modulus)
        {
            var value = details.FullCheckNumber;
            if (value.Length != 14)
            {
                throw new FormatException("Sort Code and Account Number combined should equal 14 characters");

            }
            var sum = 0;
            for (var i = 0; i < 14; i++)
            {
                var weightMapping = details.WeightMappings[1];
                var result= (int.Parse(value[i].ToString(CultureInfo.InvariantCulture)) * weightMapping.WeightValues[i]);
                sum += GetIntAsArray(result).Sum();
            }

            var remainder = sum % modulus;
            return remainder == 0;
        }

        private static IEnumerable<int> GetIntAsArray(int num)
        {
            var numbers = new List<int>();
            while (num > 0)
            {
                numbers.Add(num % 10);
                num = num / 10;
            }
            numbers.Reverse();
            return numbers.ToArray();
        }

    }
}
