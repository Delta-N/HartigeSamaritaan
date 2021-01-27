namespace RoosterPlanner.Models.Models.Enums
{
    /// <summary>
    /// Can be Boardmember, Committeemember(Manager),Volunteer(Currently not used),None
    /// </summary>
    public enum UserRole
    {
        Boardmember = 1,
        Committeemember = 2,
        Volunteer = 3,
        None = 4
    }
}