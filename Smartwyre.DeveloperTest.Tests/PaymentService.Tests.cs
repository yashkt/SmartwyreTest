using NUnit.Framework;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Tests
{
    [TestFixture]
    public class RebateServiceTests
    {
        private RebateService _rebateService;
        private RebateDataStore _rebateDataStore;
        private ProductDataStore _productDataStore;

        [SetUp]
        public void Setup()
        {
            _rebateService = new RebateService();
            _rebateDataStore = new RebateDataStore();
            _productDataStore = new ProductDataStore();
        }

        [Test]
        public void GetSupportedIncentiveType_WhenCalledWithIncentiveType_ReturnsSupportedIncentiveType()
        {
            // Arrange
            IncentiveType incentiveType = IncentiveType.FixedCashAmount;

            // Act
            SupportedIncentiveType result = _rebateService.GetSupportedIncentiveType(incentiveType);

            // Assert
            Assert.AreEqual(SupportedIncentiveType.FixedCashAmount, result);
        }

        [Test]
        public void GetRebateResult_WhenRebateIsNull_ReturnsFalse()
        {
            // Arrange
            Product product = new Product();
            IncentiveType incentiveType = IncentiveType.FixedCashAmount;
            Rebate rebate = null;

            // Act
            bool result = _rebateService.GetRebateResult(product, incentiveType, rebate);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetRebateResult_WhenProductDoesNotSupportIncentive_ReturnsFalse()
        {
            // Arrange
            Product product = new Product();
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;
            IncentiveType incentiveType = IncentiveType.FixedCashAmount;
            Rebate rebate = new Rebate();

            // Act
            bool result = _rebateService.GetRebateResult(product, incentiveType, rebate);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetRebateResult_WhenRebateAmountIsZero_ReturnsFalse()
        {
            // Arrange
            Product product = new Product();
            product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;
            IncentiveType incentiveType = IncentiveType.FixedCashAmount;
            Rebate rebate = new Rebate();
            rebate.Amount = 0;

            // Act
            bool result = _rebateService.GetRebateResult(product, incentiveType, rebate);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetRebateResult_WhenAllConditionsAreMet_ReturnsTrue()
        {
            // Arrange
            Product product = new Product();
            product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;
            IncentiveType incentiveType = IncentiveType.FixedCashAmount;
            Rebate rebate = new Rebate();
            rebate.Amount = 10;

            // Act
            bool result = _rebateService.GetRebateResult(product, incentiveType, rebate);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Calculate_WhenCalledWithCalculateRebateRequest_ReturnsCalculateRebateResult()
        {
            // Arrange
            CalculateRebateRequest request = new CalculateRebateRequest();
            request.RebateIdentifier = "rebate1";
            request.ProductIdentifier = "product1";
            request.Volume = 10;

            // Act
            CalculateRebateResult result = _rebateService.Calculate(request);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}