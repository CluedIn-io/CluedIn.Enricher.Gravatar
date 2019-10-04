// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravatarExternalSearchProvider.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the GravatarExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Providers.Gravatar.Model;
using CluedIn.ExternalSearch.Providers.Gravatar.Vocabularies;

using Hyldahl.GravatarClient;

using MailAddressUtility = CluedIn.Core.MailAddressUtility;

namespace CluedIn.ExternalSearch.Providers.Gravatar
{
    /// <summary>The gravatar external search provider.</summary>
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class GravatarExternalSearchProvider : ExternalSearchProviderBase
    {
        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarExternalSearchProvider" /> class.
        /// </summary>
        public GravatarExternalSearchProvider()
            : base(Constants.ExternalSearchProviders.GravatarId, EntityType.Person, EntityType.Infrastructure.User, EntityType.Infrastructure.Contact)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        /// <summary>Builds the queries.</summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns>The search queries.</returns>
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            if (!this.Accepts(request.EntityMetaData.EntityType))
                yield break;

            var existingResults = request.GetQueryResults<GravatarResult>(this).ToList();

            Func<string, bool> emailFilter = value => string.IsNullOrEmpty(value)
                                                   || value.IsGuid() 
                                                   || value.IsNumber() 
                                                   || !MailAddressUtility.IsValid(value)
                                                   || existingResults.Any(r => string.Equals(r.Data.Email, value, StringComparison.InvariantCultureIgnoreCase));

            // Query Input
            var entityType      = request.EntityMetaData.EntityType;
            var email           = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, null);
            var emailAddresses  = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.EmailAddresses, new HashSet<string>()).ToHashSet();

            if (!string.IsNullOrEmpty(request.EntityMetaData.Name))
                emailAddresses.Add(request.EntityMetaData.Name);
            if (!string.IsNullOrEmpty(request.EntityMetaData.DisplayName))
                emailAddresses.Add(request.EntityMetaData.DisplayName);
            if (email != null)
                emailAddresses.AddRange(email);

            request.EntityMetaData.Aliases.ForEach(a => emailAddresses.Add(a));

            if (emailAddresses != null)
            {
                var values = emailAddresses.SelectMany(v => v.Split(new[] { ",", ";", "|" }, StringSplitOptions.RemoveEmptyEntries)).ToHashSet();

                foreach (var value in values.Where(v => !emailFilter(v)))
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Identifier, value);
            }
        }

        /// <summary>Executes the search.</summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The results.</returns>
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query)
        {
            var email = query.QueryParameters[ExternalSearchQueryParameter.Identifier].FirstOrDefault();

            if (string.IsNullOrEmpty(email))
                yield break;

            var client  = new GravatarService();

            var profile = client.GetProfile(email);

            if (profile != null)
                yield return new ExternalSearchQueryResult<GravatarResult>(query, new GravatarResult(email, profile));
        }

        /// <summary>Builds the clues.</summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The clues.</returns>
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<GravatarResult>();

            var code = this.GetOriginEntityCode(resultItem);

            var clue = new Clue(code, context.Organization);
            clue.Data.OriginProviderDefinitionId = this.Id;

            this.PopulateMetadata(context, clue.Data.EntityData, resultItem);
            this.DownloadPreviewImage(context, resultItem.Data.Profile.ThumbnailUrl, clue);

            return new[] { clue };
        }

        /// <summary>Gets the primary entity metadata.</summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The primary entity metadata.</returns>
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<GravatarResult>();
            return this.CreateMetadata(context, resultItem);
        }

        /// <summary>Gets the preview image.</summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The preview image.</returns>
        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return this.DownloadPreviewImageBlob<GravatarResult>(context, result, r => string.Format("{0}{1}", r.Data.Profile.ThumbnailUrl, "s=200"));
        }

        /// <summary>Creates the metadata.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(ExecutionContext context, IExternalSearchQueryResult<GravatarResult> resultItem)
        {
            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(context, metadata, resultItem);

            return metadata;
        }

        /// <summary>Gets the origin entity code.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The origin entity code.</returns>
        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<GravatarResult> resultItem)
        {
            return new EntityCode(EntityType.Infrastructure.User, this.GetCodeOrigin(), resultItem.Data.Profile.Id.ToLowerInvariant());
        }

        /// <summary>Gets the code origin.</summary>
        /// <returns>The code origin</returns>
        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("gravatar");
        }

        /// <summary>Populates the metadata.</summary>
        /// <param name="context">The context.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(ExecutionContext context, IEntityMetadata metadata, IExternalSearchQueryResult<GravatarResult> resultItem)
        {
            var profile = resultItem.Data.Profile;

            var code = this.GetOriginEntityCode(resultItem);

            var name = profile.Name;

            metadata.EntityType             = EntityType.Infrastructure.User;
            metadata.Name                   = profile.DisplayName ?? profile.PreferredUsername;
            metadata.DisplayName            = (name != null ? name.Formatted : profile.DisplayName) ?? profile.DisplayName ?? profile.PreferredUsername;
            metadata.OriginEntityCode       = code;
            metadata.Uri                    = profile.ProfileUrl;
            metadata.Description            = profile.AboutMe;

            metadata.Codes.Add(code);
            metadata.Codes.Add(new EntityCode(EntityType.Infrastructure.User, this.GetCodeOrigin(), profile.Hash.ToLowerInvariant()));
            metadata.Codes.Add(new EntityCode(EntityType.Infrastructure.User, CodeOrigin.CluedIn.CreateSpecific("email"), resultItem.Data.Email.ToLowerInvariant()));

            // Aliases
            if (!string.IsNullOrEmpty(profile.DisplayName))
                metadata.Aliases.Add(profile.DisplayName);

            if (!string.IsNullOrEmpty(profile.PreferredUsername))
                metadata.Aliases.Add(profile.PreferredUsername);

            if (name != null)
            {
                if (!string.IsNullOrEmpty(name.Formatted))
                    metadata.Aliases.Add(name.Formatted);

                if (!string.IsNullOrEmpty(name.GivenName) && !string.IsNullOrEmpty(name.FamilyName))
                    metadata.Aliases.Add(name.GivenName + " " + name.FamilyName);
            }

            var emails = new HashSet<string>();
            {
                emails.Add(resultItem.Data.Email);

                if (profile.Emails != null)
                    emails.AddRange(profile.Emails.Select(e => e.Value));
            }

            metadata.Properties[GravatarVocabulary.User.AboutMe]                    = profile.AboutMe;
            metadata.Properties[GravatarVocabulary.User.CurrentLocation]            = profile.CurrentLocation;
            metadata.Properties[GravatarVocabulary.User.DisplayName]                = profile.DisplayName;
            metadata.Properties[GravatarVocabulary.User.Hash]                       = profile.Hash;
            metadata.Properties[GravatarVocabulary.User.Id]                         = profile.Id;
            metadata.Properties[GravatarVocabulary.User.NameFamilyName]             = profile.PrintIfAvailable(p => p.Name, n => n.FamilyName);
            metadata.Properties[GravatarVocabulary.User.NameGivenName]              = profile.PrintIfAvailable(p => p.Name, n => n.GivenName);
            metadata.Properties[GravatarVocabulary.User.NameFormatted]              = profile.PrintIfAvailable(p => p.Name, n => n.Formatted);
            metadata.Properties[GravatarVocabulary.User.PreferredUsername]          = profile.PreferredUsername;
            metadata.Properties[GravatarVocabulary.User.RequestHash]                = profile.RequestHash;
            metadata.Properties[GravatarVocabulary.User.ProfileBackgroundColor]     = profile.PrintIfAvailable(p => p.ProfileBackground, bg => bg.Color);
            metadata.Properties[GravatarVocabulary.User.ProfileBackgroundUrl]       = profile.PrintIfAvailable(p => p.ProfileBackground, bg => bg.Url);
            metadata.Properties[GravatarVocabulary.User.ProfileUrl]                 = profile.ProfileUrl.PrintIfAvailable();
            metadata.Properties[GravatarVocabulary.User.ThumbnailUrl]               = profile.ThumbnailUrl.PrintIfAvailable();
            metadata.Properties[GravatarVocabulary.User.Email]                      = resultItem.Data.Email;

            foreach (var account in profile.Accounts ?? new List<ProfileAccount>())
            {
                switch (account.Shortname)
                {
                    case "facebook":
                        metadata.Properties[GravatarVocabulary.User.SocialFacebook]     = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "foursquare":
                        metadata.Properties[GravatarVocabulary.User.SocialFoursquare]   = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "google":
                        metadata.Properties[GravatarVocabulary.User.SocialGoogle]       = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "linkedin":
                        metadata.Properties[GravatarVocabulary.User.SocialLinkedIn]     = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "twitter":
                        metadata.Properties[GravatarVocabulary.User.SocialTwitter]      = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "youtube":
                        metadata.Properties[GravatarVocabulary.User.SocialYouTube]      = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "blogger":
                        metadata.Properties[GravatarVocabulary.User.SocialBlogger]      = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "flickr":
                        metadata.Properties[GravatarVocabulary.User.SocialFlickr]       = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "goodreads":
                        metadata.Properties[GravatarVocabulary.User.SocialGoodReads]    = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "tripit":
                        metadata.Properties[GravatarVocabulary.User.SocialTripIt]       = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "tumblr":
                        metadata.Properties[GravatarVocabulary.User.SocialTumblr]       = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "vimeo":
                        metadata.Properties[GravatarVocabulary.User.SocialVimeo]        = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "wordpress":
                        metadata.Properties[GravatarVocabulary.User.SocialWordPress]    = account.Url.PrintIfAvailable() ?? account.Username;
                        break;
                    case "yahoo":
                        metadata.Properties[GravatarVocabulary.User.SocialYahoo]        = account.Url.PrintIfAvailable() ?? account.Username;
                        break;

                    default:
                        context.Log.Warn(new { profile }, () => "Unknown Gravatar Account Type: {0}".FormatWith(account.Shortname));
                        break;
                }
            }

            foreach (var currency in profile.Currency ?? new List<ProfileCurrency>())
            {
                switch (currency.Type)
                {
                    case "bitcoin":
                        metadata.Properties[GravatarVocabulary.User.CurrencyBitcoin]    = currency.Value;
                        break;
                    case "litecoin":
                        metadata.Properties[GravatarVocabulary.User.CurrencyLitecoin]   = currency.Value;
                        break;
                    case "dogecoin":
                        metadata.Properties[GravatarVocabulary.User.CurrencyDogecoin]   = currency.Value;
                        break;

                    default:
                        context.Log.Warn(new { profile }, () => "Unknown Gravatar Currency Type: {0}".FormatWith(currency.Type));
                        break;
                }
            }

            foreach (var email in profile.Emails ?? new List<ProfileEmail>())
                metadata.Codes.Add(new EntityCode(EntityType.Infrastructure.User, CodeOrigin.CluedIn.CreateSpecific("email"), email.Value.ToLowerInvariant()));

            foreach (var account in profile.Ims ?? new List<ProfileIMAccount>())
            {
                if (MailAddressUtility.IsValid(account.Value))
                    emails.Add(account.Value);

                switch (account.Type)
                {
                    case "aim":
                        metadata.Properties[GravatarVocabulary.User.MessagingAIM]     = account.Value;
                        break;
                    case "yahoo":
                        metadata.Properties[GravatarVocabulary.User.MessagingYahoo]   = account.Value;
                        break;
                    case "icq":
                        metadata.Properties[GravatarVocabulary.User.MessagingIcq]     = account.Value;
                        break;
                    case "gtalk":
                        metadata.Properties[GravatarVocabulary.User.MessagingGtalk]   = account.Value;
                        break;
                    case "skype":
                        metadata.Properties[GravatarVocabulary.User.MessagingSkype]   = account.Value;
                        break;

                    default:
                        context.Log.Warn(new { profile }, () => "Unknown Gravatar IM account Type: {0}".FormatWith(account.Type));
                        break;
                }
            }

            foreach (var phoneNumber in profile.PhoneNumbers ?? new List<ProfilePhoneNumber>())
            {
                switch (phoneNumber.Type)
                {
                    case "mobile":
                        metadata.Properties[GravatarVocabulary.User.PhoneNumberMobile] = phoneNumber.Value;
                        break;
                    case "home":
                        metadata.Properties[GravatarVocabulary.User.PhoneNumberHome]   = phoneNumber.Value;
                        break;
                    case "work":
                        metadata.Properties[GravatarVocabulary.User.PhoneNumberWork]   = phoneNumber.Value;
                        break;

                    default:
                        context.Log.Warn(new { profile }, () => "Unknown Gravatar Phone Number Type: {0}".FormatWith(phoneNumber.Type));
                        break;
                }
            }

            // profile.Photos;

            foreach (var profileUrl in profile.Urls ?? new List<ProfileUrl>())
                if (profileUrl.Value != null)
                    metadata.ExternalReferences.Add(profileUrl.Value);

            metadata.Properties[GravatarVocabulary.User.Emails]                     = string.Join(";", emails);
        }
    }
}
