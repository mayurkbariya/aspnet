using System;
using System.IO;
using System.Reflection;

namespace FBDropshipper.Application.Templates
{
    public class TemplateManager
    {
        static string BasePath
        {
            get
            {
                var result = Assembly.GetExecutingAssembly().Location;
                return Path.GetDirectoryName(result);
            }
        }
        public static string ReadEmailTemplate(string fileName)
        {
            var path = Path.Combine(BasePath, "Templates", "Email", fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            throw new ArgumentException("No File Exits",nameof(fileName));
        }

        public static string SignUpEmail(string fullName, string email, string password)
        {
            var template = ReadEmailTemplate("SignUp.html");
            return template.Replace("{{fullName}}", fullName)
                .Replace("{{email}}", email)
                .Replace("{{password}}", password);
        }
    }
}