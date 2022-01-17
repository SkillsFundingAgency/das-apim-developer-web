using System.IO;

namespace SFA.DAS.Apim.Developer.Web.AcceptanceTests.Infrastructure
{
    public class DataFileManager
    {
        public static string GetFile(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}