namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IApiDescriptionHelper
    {
        string ProcessApiDescription(string data, string keyName, bool showDocumentationUrl = true);
    }
}