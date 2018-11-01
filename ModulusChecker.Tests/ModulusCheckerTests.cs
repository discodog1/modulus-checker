using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModulsChecker.Models;
using ModulsChecker.Models.Enums;
using ModulusChecker.API;
using ModulusChecker.API.Controllers;
using ModulusChecker.Services.Implementations;
using Newtonsoft.Json;

namespace ModulusChecker.Tests
{
    [TestClass]
    public class ModulusCheckerTests
    {

        private ModulusController GetController()
        {
            var mappingImporter = new WeightMappingImporter();
            var modulusChecker = new Services.Implementations.ModulusChecker();
            
            return new ModulusController(mappingImporter, modulusChecker);
        }
        [TestMethod]
        public void ShouldPassModulus10WithNoSecondCheckRequired()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "66374958",
                SortCode = "089999"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result =JsonConvert.DeserializeObject<ResultModel>(content.Content) ;
                       
            Assert.AreEqual(result.CanValidate,true);
            Assert.AreEqual(result.PassedValidation, true);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD10);
            Assert.AreEqual(result.RequiresSecondCheck, false);

        }
        [TestMethod]
        public void ShouldPassModulus11WithNoSecondCheckRequired()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "88837491",
                SortCode = "107999"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, true);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.RequiresSecondCheck, false);

        }
        [TestMethod]
        public void ShouldPassModulus11AndDoubleAlternativeChecks()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "63748472",
                SortCode = "202959"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, true);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.PassedFirstCheck, true);
            Assert.AreEqual(result.RequiresSecondCheck, true);
            Assert.AreEqual(result.SecondAlgorithm, ModulusCheckingAlgorithm.DBLAL);
            Assert.AreEqual(result.PassedSecondCheck, true);

        }

        [TestMethod]
        public void ShouldPassModulus10WithException4()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "63849203",
                SortCode = "134020"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, true);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.FirstCheckIsException4, true);

        }

        [TestMethod]
        public void ShouldPassModulus10WithException7()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "99345694",
                SortCode = "772798"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, true);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.FirstCheckIsException7, true);

        }

        [TestMethod]
        public void ShouldPassModulus11AndFailDoubleAlternativeCheck()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "66831036",
                SortCode = "203099"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, false);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.PassedFirstCheck, true);
            Assert.AreEqual(result.RequiresSecondCheck, true);
            Assert.AreEqual(result.SecondAlgorithm, ModulusCheckingAlgorithm.DBLAL);
            Assert.AreEqual(result.PassedSecondCheck, false);

        }

        [TestMethod]
        public void ShouldFailModulus11WithNo()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "58716970",
                SortCode = "203099"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, false);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.PassedFirstCheck, false);          

        }

        [TestMethod]
        public void ShouldFailModulus10Check()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "66374959",
                SortCode = "089999"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, false);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD10);
            Assert.AreEqual(result.PassedFirstCheck, false);
           

        }

        [TestMethod]
        public void ShouldFailModulus11Check()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "88837493",
                SortCode = "107999"
            };
            var output = controller.Post(model);
            var content = output as ContentResult;
            Assert.IsNotNull(content);
            var result = JsonConvert.DeserializeObject<ResultModel>(content.Content);

            Assert.AreEqual(result.CanValidate, true);
            Assert.AreEqual(result.PassedValidation, false);
            Assert.AreEqual(result.FirstAlgorithm, ModulusCheckingAlgorithm.MOD11);
            Assert.AreEqual(result.PassedFirstCheck, false);

        }

        [TestMethod]
        public void ShouldFailInvalidAccountNumberCheck()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "999",
                SortCode = "107999"
            };
          
            Assert.ThrowsException<ArgumentException>(()=> controller.Post(model));


        }

        [TestMethod]
        public void ShouldFailInvalidSortCodeCheck()
        {
            var controller = GetController();

            var model = new BankAccountModel
            {
                AccountNumber = "88837493",
                SortCode = "abc"
            };

            Assert.ThrowsException<ArgumentException>(() => controller.Post(model));
        }
    }
}
