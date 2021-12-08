using System.ComponentModel;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public class ServiceParameters
    {
        public virtual AuthenticationType? AuthenticationType { get; set; }
    }

    public enum AuthenticationType
    {
        [Description("Employers")]
        Employer,
        [Description("Providers")]
        Provider,
        [Description("ExternalUsers")]
        External
    }
}