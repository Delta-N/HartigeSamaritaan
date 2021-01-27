namespace RoosterPlanner.Api.Models.Constants
{
    /// <summary>
    /// This class is used for transforming AdditionalData attributes, from Microsoft Graph, to PersonViewModels.
    /// </summary>
    public class Extensions
    {
        private static Extensions _extensions;
        private readonly string b2CExtentionApplicationId;
        
        private Extensions(string b2CExtentionApplicationId)
        {
            this.b2CExtentionApplicationId = b2CExtentionApplicationId;
        }

        public static Extensions GetInstance(string b2CExtentionApplicationId)
        {
            return _extensions ??= new Extensions(b2CExtentionApplicationId);
        }

        public string UserRoleExtension =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_UserRole";

        public string DateOfBirthExtension =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_DateOfBirth";

        public string PhoneNumberExtension =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_PhoneNumber";

        public string NationalityExtension =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_Nationality";

        public string NativeLanguageExtention =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_NativeLanguage";

        public string DutchProficiencyExtention =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_DutchProficiency";

        public string TermsOfUseConsentedExtention =>
            $"extension_{b2CExtentionApplicationId.Replace("-", "")}_TermsOfUseConsented";
    }
}