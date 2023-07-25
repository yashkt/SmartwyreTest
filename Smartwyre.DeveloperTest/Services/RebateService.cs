using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{

    public SupportedIncentiveType GetSupportedIncentiveType(IncentiveType incentive)
    {
        return Enum.Parse<SupportedIncentiveType>(incentive.ToString());
    }

    public bool GetRebateResult(Product product, IncentiveType incentiveType, Rebate rebate)
    {
        if (rebate == null)
            return false;
        else if (!product.SupportedIncentives.HasFlag(GetSupportedIncentiveType(incentiveType)))
            return false;
        else if (rebate.Amount == 0)
            return false;
        else
            return true;

    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {

        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();

        Rebate rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        var rebateAmount = 0m;

        switch (rebate.Incentive)
        {
            case IncentiveType.FixedCashAmount:
                if (!GetRebateResult(product, rebate.Incentive, rebate))
                {
                    result.Success = false;
                }
                else
                {
                    rebateAmount = rebate.Amount;
                    result.Success = true;
                }

                break;

            case IncentiveType.FixedRateRebate:
                if (GetRebateResult(product, rebate.Incentive, rebate) == false)
                {
                    result.Success = false;
                }
                else if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
                {
                    result.Success = false;
                }
                else
                {
                    rebateAmount += product.Price * rebate.Percentage * request.Volume;
                    result.Success = true;
                }

                break;

            case IncentiveType.AmountPerUom:
                if (GetRebateResult(product, rebate.Incentive, rebate) == false)
                {
                    result.Success = false;
                }
                else if(rebate.Amount == 0 || request.Volume == 0)
                {
                    result.Success = false;
                }
                else
                {
                    rebateAmount += rebate.Amount * request.Volume;
                    result.Success = true;
                }

                break;
        }

        if (result.Success)
        {
            var storeRebateDataStore = new RebateDataStore();
            storeRebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }

        return result;
    }
}
