namespace RoosterPlanner.Service.Helpers
{
    internal class B2cCustomAttributeHelper
    {
        internal readonly string _b2cExtensionAppClientId;

        internal B2cCustomAttributeHelper(string b2cExtensionAppClientId)
        {
            _b2cExtensionAppClientId = b2cExtensionAppClientId.Replace("-", "");
        }

        internal string GetCompleteAttributeName(string attributeName)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new System.ArgumentException("Parameter cannot be null", nameof(attributeName));
            }

            return $"extension_{_b2cExtensionAppClientId}_{attributeName}";
        }

        //nog te testen
        internal string GetAttributeName(string CompleteAttributeName)
        {
            if (string.IsNullOrWhiteSpace(CompleteAttributeName))
            {
                throw new System.ArgumentException("Parameter cannot be null", nameof(CompleteAttributeName));
            }

            return CompleteAttributeName.Replace($"extension_{_b2cExtensionAppClientId}","");
        }

        public string GetTenant()
        {
            return "DeltanHackaton.onmicrosoft.com";
        }
    }
}