using Settlement.Domain.Constants;
using Settlement.Domain.Enums;

namespace Settlement.Domain.Calculation
{
    public class CommisionCalculator
    {
        public static decimal GetCommisionPercentage(RoleType roleType)
        {
            switch (roleType)
            {
                case RoleType.Trial:
                    return TrialRolePercentageConstant.Trial;
                case RoleType.VIP:
                    return VIPRolePercentageConstant.VIP;
                case RoleType.Special:
                    return SpecialRolePercentageConstant.Special;
                default:
                    return CommissionPercentageConstant.commissionPercentage;
            }
        }
    }
}
