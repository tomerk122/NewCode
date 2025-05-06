using Microsoft.Extensions.Options;

namespace UserManagement.Models
{
    public class Manager
    {
        private readonly ManagerSettings _managerSettings;

        public Manager(IOptions<ManagerSettings> managerSettings)
        {
            _managerSettings = managerSettings.Value;
        }
        public void Print()
        {
            Console.WriteLine($"Im the manager: {_managerSettings.ManagerName} and my id is {_managerSettings.ManagerId}");

        }
        public string GetManagerName()
        {
            return _managerSettings.ManagerName;
        }
    }
}
