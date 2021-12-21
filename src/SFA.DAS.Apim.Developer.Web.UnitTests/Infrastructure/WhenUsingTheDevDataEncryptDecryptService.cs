using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenUsingTheDevDataEncryptDecryptService
    {
        [Test, MoqAutoData]
        public void Then_Encrypted_Data_Is_Returned_As_Is(
            Guid id,
            DevDataProtectorService service)
        {
            var actual = service.EncodedData(id);

            actual.Should().Be(id.ToString());
        }

        [Test, MoqAutoData]
        public void Then_Decrypted_Data_Is_Returned_As_Is(
            Guid? id,
            DevDataProtectorService service)
        {
            var actual = service.DecodeData(id.ToString());

            actual.Should().Be(id);
        }
    }
}