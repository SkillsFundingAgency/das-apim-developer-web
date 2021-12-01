namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IApiDescriptionHelper
    {
        string ProcessApiDescription(string data, string keyName, string apiName, bool showDocumentationUrl = true);
    }
}