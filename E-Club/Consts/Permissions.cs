namespace E_Club.Consts
{
    public static class Permissions
    {
        public static string Type { get; } = "Permissions";

       
        public static IList<string?> GetAllPermissions() =>
            typeof(Permissions).GetFields().Select(x=>x.GetValue(x) as string).ToList();

    }
}
