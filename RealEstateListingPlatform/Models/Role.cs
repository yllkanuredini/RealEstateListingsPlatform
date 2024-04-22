namespace RealEstateListingPlatform.Models
{
    public enum Role
    {
        Admin,
        Agent,
        User
    }

    public static class RoleExtensions
    {
        public static string ToRoleString(this Role role)
        {
            return role.ToString();
        }
    }
}