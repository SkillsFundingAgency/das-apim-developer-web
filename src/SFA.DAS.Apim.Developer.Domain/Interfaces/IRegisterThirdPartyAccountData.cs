using System;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IRegisterThirdPartyAccountData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmUrl { get; set; }
    }
}