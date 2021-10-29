namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public class ServiceParameters
    {
        public AuthenticationType? AuthenticationType { get; set; }
    }

    public enum AuthenticationType
    {
        Employer,
        Provider
    }
}