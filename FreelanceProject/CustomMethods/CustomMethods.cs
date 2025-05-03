using FreelanceProject.Data.Entities;

namespace FreelanceProject.CustomMethods
{
    public static class CustomMethods
    {
        public static string GenerateUsername(string input)
        {
            if (string.IsNullOrEmpty(input)) return "Guest";

            return input.Substring(0, input.IndexOf('@')) +  new Random().Next(1000,9999);

        }
        public static bool ChangeFolderName(AppUser user, string oldJobName, string newJobName)
        {
            if (oldJobName == newJobName) return true;

            // wwwroot yolunu al
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Eski ve yeni klasör yollarını oluştur
            var oldFolderPath = Path.Combine(wwwRootPath,"Users" ,$"{user.UserName}", "CreatedJobs", oldJobName);
            var newFolderPath = Path.Combine(wwwRootPath,"Users",$"{user.UserName}", "CreatedJobs", newJobName);

            if (Directory.Exists(oldFolderPath))
            {
                if (Directory.Exists(newFolderPath))
                {
                    return false;
                }

                Directory.Move(oldFolderPath, newFolderPath);

                return true;
            }
            return false;


        }
    }
}

    
