namespace FBDropshipper.Domain.Constant
{
    public static class RoleNames
    {
        public const string Admin = "Administrator";
        public const string TeamLeader = "TeamLeader";
        public const string TeamMember = "TeamMember";
        
        public static string[] AllRoles =
        {
            Admin,
            TeamLeader,
            TeamMember
        };
    }
}