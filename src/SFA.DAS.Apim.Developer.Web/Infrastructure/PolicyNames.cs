namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public static class PolicyNames
    {
        public static string HasProviderEmployerAdminOrExternalAccount = nameof(HasProviderEmployerAdminOrExternalAccount);
        public static string HasEmployerAccount => nameof(HasEmployerAccount);
        public static string HasProviderAccount => nameof(HasProviderAccount);
        public static string HasEmployerViewAccount => nameof(HasEmployerViewAccount);
        public static string HasExternalAccount => nameof(HasExternalAccount);
    }
}