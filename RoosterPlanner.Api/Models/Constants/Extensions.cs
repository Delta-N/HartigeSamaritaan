using RoosterPlanner.Common.Config;

namespace RoosterPlanner.Api.Models.Constants
{

    public class Extensions
    {
        private static Extensions _extensions;
        private readonly AzureAuthenticationConfig azureB2CConfig;

        private Extensions(AzureAuthenticationConfig azureB2CConfig)
        {
            this.azureB2CConfig = azureB2CConfig;
        }

        public static Extensions GetInstance(AzureAuthenticationConfig azureB2CConfig)
        {
            return _extensions ??= new Extensions(azureB2CConfig);
        }

        public string UserRoleExtension =>
            $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_UserRole";

        public string DateOfBirthExtension =>
            $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_DateOfBirth";

        public string PhoneNumberExtension =>
            $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_PhoneNumber";

        public string NationalityExtension =>
            $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_Nationality";
    }
}