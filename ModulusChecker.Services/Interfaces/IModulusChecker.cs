using System;
using System.Collections.Generic;
using System.Text;
using ModulsChecker.Models;

namespace ModulusChecker.Services.Interfaces
{
    public interface IModulusChecker
    {
        bool FirstStandardTenCheck(BankAccount details);
        bool FirstStandardElevenCheck(BankAccount details);
        bool SecondStandardTenCheck(BankAccount details);
        bool SecondStandardElevenCheck(BankAccount details);
        bool DoubleAlternateCheck(BankAccount details);
    }
}
