namespace E_Club.Consts
{
    public class DefaultRoles
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Coach = "Coach";
        public const string Physiotherapist = "Physiotherapist";
        public const string Supervisor = "Supervisor";

        public static readonly List<string> All = new()
        {
            Admin, Member, Coach, Physiotherapist, Supervisor
        };
    }
}
