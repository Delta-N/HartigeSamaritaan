namespace RoosterPlanner.Api.Models.Constants
{
    //Keuze gemaakt om de constanten hier op te nemen. @Dennis waar zou jij dat doen.
    //Het leek mij overigens geen goed idee op de appsettings te laten injecteren in PersonViewmodel
    public class Extensions
    {
        public static string UserRoleExtension =>
            "extension_4e6dae7dd1c74eac85fefc6da42e7b61_UserRole";

        public static string DateOfBirthExtension
        {
            get { return "extension_4e6dae7dd1c74eac85fefc6da42e7b61_DateOfBirth"; }
        }
        
        public static string PhoneNumberExtension
        {
            get { return "extension_4e6dae7dd1c74eac85fefc6da42e7b61_PhoneNumber"; }
        }
    }
}