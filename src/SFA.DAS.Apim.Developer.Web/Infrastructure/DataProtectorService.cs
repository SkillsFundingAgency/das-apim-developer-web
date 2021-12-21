using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Developer.Domain.Configuration;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public interface IDataProtectorService
    {
        string EncodedData(Guid data);
        Guid? DecodeData(string data);
    }
    
    public class DataProtectorService : IDataProtectorService
    {
        private readonly ILogger<DataProtectorService> _logger;
        private readonly IDataProtector _dataProtector;

        public DataProtectorService (IDataProtectionProvider provider, ILogger<DataProtectorService> logger)
        {
            _logger = logger;
            _dataProtector = provider.CreateProtector(ApimDeveloperWebConstants.ProtectorName);
        }
        
        public string EncodedData(Guid data)
        {
            return WebEncoders.Base64UrlEncode(_dataProtector.Protect(
                System.Text.Encoding.UTF8.GetBytes($"{data}")));
        }

        public Guid? DecodeData(string data)
        {
            try
            {
                var base64EncodedBytes = WebEncoders.Base64UrlDecode(data);
                var encodedId = System.Text.Encoding.UTF8.GetString(_dataProtector.Unprotect(base64EncodedBytes));
                var result = Guid.TryParse(encodedId, out var id);
                return result ? id : (Guid?)null;
            }
            catch (FormatException e)
            {
                _logger.LogInformation(e,"Unable to decode data from request");
            }
            catch (CryptographicException e)
            {
                _logger.LogInformation(e, "Unable to decode data from request");
            }

            return null;
        }
    }

    public class DevDataProtectorService : IDataProtectorService
    {
        public string EncodedData(Guid data)
        {
            return data.ToString();
        }

        public Guid? DecodeData(string data)
        {
            return Guid.Parse(data);
        }
    }
}