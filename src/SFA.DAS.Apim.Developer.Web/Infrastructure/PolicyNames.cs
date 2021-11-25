namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public static class PolicyNames
    {
        public static string HasProviderOrEmployerAccount = nameof(HasProviderOrEmployerAccount);
        public static string HasEmployerAccount => nameof(HasEmployerAccount);
        public static string HasProviderAccount => nameof(HasProviderAccount);
        public static string HasEmployerViewAccount => nameof(HasEmployerViewAccount);
    }
}