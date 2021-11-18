using System;

namespace SFA.DAS.Apim.Developer.MockServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mock Server starting on http://localhost:5010");

            MockApiServer.Start();

            Console.WriteLine(("Press any key to stop the server"));
            Console.ReadKey();
        }
    }
}
