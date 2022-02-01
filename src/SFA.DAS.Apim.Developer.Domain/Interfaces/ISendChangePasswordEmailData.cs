using System;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface ISendChangePasswordEmailData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ChangePasswordUrl { get; set; }
    }
}