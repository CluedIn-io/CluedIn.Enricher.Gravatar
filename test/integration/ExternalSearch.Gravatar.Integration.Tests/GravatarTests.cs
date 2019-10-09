// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravatarTests.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the GravatarTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Messages.Processing;
using CluedIn.ExternalSearch.Providers.Gravatar;
using CluedIn.Testing.Base.ExternalSearch;
using Moq;
using Xunit;

namespace ExternalSearch.Gravatar.Integration.Tests
{
    public class GravatarTests : BaseExternalSearchTest<GravatarExternalSearchProvider>
    {
        [Theory(Skip = "Failed Mock exception. GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        [InlineData("anncluedin", "ann@cluedin.com")]
        public void TestClueProduction(string userName, string email)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, email);

            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                Name = userName,
                EntityType = EntityType.Person,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);
            Assert.True(clues.Count > 0);
        }

        [Theory]
        [InlineData("asd asd", "asd123@asd123.com")]
        [InlineData("", "asd123@asd123.com")]
        [InlineData("asd asd", "")]
        [InlineData("", "")]
        [InlineData(null, "asd123@asd123.com")]
        [InlineData("asd asd", null)]
        [InlineData(null, null)]
        [Trait("Category", "slow")]
        public void TestNoClueProduction(string name, string email)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, email);

            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                Name = name,
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);
            Assert.True(clues.Count == 0);
        }
    }
}