namespace UserManagement.Api.Middleware
{
    public static class IpValidator
    {
        private static readonly List<string> AllowedIps = new List<string>
        {
            "::1",// IPv6 localhost
            "127.0.0.1",

        };

        public static bool IsIpAllowed(string? ipAddress)
        {
            return !string.IsNullOrEmpty(ipAddress) && AllowedIps.Contains(ipAddress);
        }
    }
}
