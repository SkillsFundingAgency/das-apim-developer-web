using System.Text.Json.Serialization;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IPutApiRequest 
    {
        [JsonIgnore]
        string PutUrl { get; }

        public object Data { get; set; }
    }
}