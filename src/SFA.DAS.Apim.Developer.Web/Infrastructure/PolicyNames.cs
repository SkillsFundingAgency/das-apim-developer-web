namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public static class PolicyNames
    {
        public static string HasProviderOrEmployerAdminAccount = nameof(HasProviderOrEmployerAdminAccount);
        public static string HasEmployerAccount => nameof(HasEmployerAccount);
        public static string HasProviderAccount => nameof(HasProviderAccount);
        public static string HasEmployerViewAccount => nameof(HasEmployerViewAccount);
        public static string HasExternalAccount => nameof(HasExternalAccount);
    }
}