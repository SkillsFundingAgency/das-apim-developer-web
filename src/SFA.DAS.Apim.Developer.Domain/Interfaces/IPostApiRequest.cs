using System.Text.Json.Serialization;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IPostApiRequest 
    {
        [JsonIgnore]
        string PostUrl { get; }

        public object Data { get; set; }
    }
    
    public interface IPostApiRequest<TData> : IPostApiRequest
    {
        new TData Data { get; set; }
    }
}