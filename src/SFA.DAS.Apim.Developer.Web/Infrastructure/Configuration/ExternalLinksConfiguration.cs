namespace SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration
{
    public class ExternalLinksConfiguration
    {
        public const string ApimDeveloperExternalLinksConfiguration = "ExternalLinks";

        public virtual string ManageApprenticeshipSiteUrl { get; set; }
        public virtual string CommitmentsSiteUrl { get; set; }
        public virtual string EmployerRecruitmentSiteUrl { get; set; }
        public virtual string FindAnApprenticeshipSiteUrl { get; set; }
    }
}
