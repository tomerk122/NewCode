using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UserManagement.Api.Models.DTOS;

namespace UserManagement.Api.Repositories
{
    public static class JsonCredentialsManager
    {
        private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Credentials.json");

        public static List<Credentials> LoadCredentials()
        {
            if (!File.Exists(FilePath))
                return new List<Credentials>();
          
            var jsonData = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Credentials>>(jsonData) ?? new List<Credentials>();
        }

        public static void SaveCredentials(List<Credentials> credentials)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(credentials, Formatting.Indented);
                File.WriteAllText(FilePath, jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving credentials to file: " + ex.Message);
            }
        }
    }

}
