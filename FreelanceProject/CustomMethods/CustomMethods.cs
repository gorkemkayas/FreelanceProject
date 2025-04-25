namespace FreelanceProject.CustomMethods
{
    public static class CustomMethods
    {
        public static string GenerateUsername(string input)
        {
            if (string.IsNullOrEmpty(input)) return "Guest";

            return input.Substring(0, input.IndexOf('@')) +  new Random().Next(1000,9999);

        }
    }
}

    
