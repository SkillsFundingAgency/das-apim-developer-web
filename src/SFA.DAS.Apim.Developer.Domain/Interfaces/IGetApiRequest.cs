using System.Text.Json.Serialization;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IGetApiRequest 
    {
        [JsonIgnore]
        string GetUrl { get; }
    }
}