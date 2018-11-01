using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModulsChecker.Models;
using ModulsChecker.Models.Enums;
using ModulusChecker.API.Properties;
using ModulusChecker.Services.Implementations;
using ModulusChecker.Services.Interfaces;
using Newtonsoft.Json;

namespace ModulusChecker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulusController : ControllerBase
    {
        private readonly IWeightMappingImporter _weightMappingImporter;
        private readonly IModulusChecker _modulusChecker;
        public ModulusController(IWeightMappingImporter weightMappingImporter,
            IModulusChecker modulusChecker)
        {
            _weightMappingImporter = weightMappingImporter;
            _modulusChecker = modulusChecker;
        }
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] BankAccountModel bankAccount)
        {

            var accountDetails = CreateBankAccountFromModel(bankAccount);

            var resultModel = new ResultModel
            {
                PassedSecondCheck = false,
                PassedFirstCheck = false,
                RequiresSecondCheck = false,
                CanValidate = true,
                FirstAlgorithm = accountDetails.FirstCheckAlgorithm,
                SecondAlgorithm = accountDetails.SecondCheckAlgorithm,
                FirstCheckIsException4 = accountDetails.FirstException == 4,
                SecondCheckIsException4 = accountDetails.SecondException == 4,
                FirstCheckIsException7 = accountDetails.FirstException == 7 &&
                                         accountDetails.AccountNumber.CheckExceptionSeven(),
                SecondCheckIsException7 = accountDetails.SecondException == 7 &&
                                          accountDetails.AccountNumber.CheckExceptionSeven(),
                PassedValidation = true,
                
            };

            //presume valid if can't invalidate
            if (!accountDetails.CanValidate())
            {
                resultModel.CanValidate = false;
                resultModel.ResultMessage = "Cannot Invalidate details, test passed";
                return Content(JsonConvert.SerializeObject(resultModel));
            }

            PerformFirstCheck(accountDetails);

            resultModel.PassedFirstCheck = accountDetails.PassedFirstCheck;
            resultModel.PassedValidation = resultModel.PassedFirstCheck;

            resultModel.RequiresSecondCheck = accountDetails.RequiresSecondCheck();

            if (accountDetails.RequiresSecondCheck())
            {
                PerformSecondCheck(accountDetails);
                resultModel.PassedSecondCheck = accountDetails.PassedSecondCheck;        
            }

            if (!resultModel.PassedFirstCheck || (resultModel.RequiresSecondCheck && !resultModel.PassedSecondCheck))
            {
                resultModel.PassedValidation = false;
            }

            var handledExceptions = new List<int> {4, 7};
            if (accountDetails.FirstException > 0 && !handledExceptions.Contains(accountDetails.FirstException))
            {
                resultModel.ResultMessage =
                    $"First Exception is {accountDetails.FirstException} which has not been handled in this test{Environment.NewLine}";
            }
            if (accountDetails.SecondException > 0 && !handledExceptions.Contains(accountDetails.SecondException))
            {
                resultModel.ResultMessage +=
                    $"Second Exception is {accountDetails.SecondException} which has not been handled in this test{Environment.NewLine}";
            }

            return Content(JsonConvert.SerializeObject(resultModel));
        }

        private void PerformFirstCheck(BankAccount details)
        {
            switch (details.FirstCheckAlgorithm)
            {
                case ModulusCheckingAlgorithm.MOD10:
                    details.PassedFirstCheck = _modulusChecker.FirstStandardTenCheck(details);

                    return;
                case ModulusCheckingAlgorithm.MOD11:
                    details.PassedFirstCheck = _modulusChecker.FirstStandardElevenCheck(details);
                    return;
                case ModulusCheckingAlgorithm.DBLAL:
                    details.PassedFirstCheck = _modulusChecker.DoubleAlternateCheck(details);
                    return;            
                
            }
            details.PassedFirstCheck = false;
        }

        private void PerformSecondCheck(BankAccount details)
        {
            switch (details.SecondCheckAlgorithm)
            {
                case ModulusCheckingAlgorithm.MOD10:
                    details.PassedSecondCheck = _modulusChecker.SecondStandardTenCheck(details);
                    return;
                case ModulusCheckingAlgorithm.MOD11:
                    details.PassedSecondCheck = _modulusChecker.SecondStandardElevenCheck(details);
                    return;
                case ModulusCheckingAlgorithm.DBLAL:
                    details.PassedSecondCheck = _modulusChecker.DoubleAlternateCheck(details);
                    return;
              

            }
            details.PassedSecondCheck = false;
        }

        private BankAccount CreateBankAccountFromModel(BankAccountModel model)
        {
            var sortCode = new SortCode(model.SortCode.ToCleanString());
            var accountNumber = new AccountNumber(model.AccountNumber.ToCleanString());
            var mappings = _weightMappingImporter
                .GetWeightMappings(Resources.valacdos)
                .BySortCode(sortCode);
            return new BankAccount(sortCode, accountNumber, mappings);
        }
    }
}
