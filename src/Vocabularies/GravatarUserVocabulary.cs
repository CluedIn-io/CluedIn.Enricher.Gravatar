// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravatarUserVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the GravatarUserVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Gravatar.Vocabularies
{
    /// <summary>The gravatar user vocabulary.</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class GravatarUserVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarUserVocabulary"/> class.
        /// </summary>
        public GravatarUserVocabulary()
        {
            this.VocabularyName = "Gravatar User Profile";
            this.KeyPrefix      = "gravatar.userProfile";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Infrastructure.User;

            this.AboutMe                    = this.Add(new VocabularyKey("aboutMe"));
            this.CurrentLocation            = this.Add(new VocabularyKey("currentLocation",         VocabularyKeyDataType.GeographyLocation));
            this.DisplayName                = this.Add(new VocabularyKey("displayName"));
            this.Hash                       = this.Add(new VocabularyKey("hash",                                                                VocabularyKeyVisibility.Hidden));
            this.Id                         = this.Add(new VocabularyKey("id",                                                                  VocabularyKeyVisibility.Hidden));
            this.NameFamilyName             = this.Add(new VocabularyKey("nameFamilyName"));
            this.NameGivenName              = this.Add(new VocabularyKey("nameGivenName"));
            this.NameFormatted              = this.Add(new VocabularyKey("nameFormatted",           VocabularyKeyDataType.PersonName));
            this.PreferredUsername          = this.Add(new VocabularyKey("preferredUsername"));
            this.RequestHash                = this.Add(new VocabularyKey("requestHash",                                                         VocabularyKeyVisibility.Hidden));
            this.ProfileBackgroundColor     = this.Add(new VocabularyKey("profileBackgroundColor"));
            this.ProfileBackgroundUrl       = this.Add(new VocabularyKey("profileBackgroundUrl",    VocabularyKeyDataType.Uri, VocabularyKeyVisibility.Hidden));
            this.ProfileUrl                 = this.Add(new VocabularyKey("profileUrl",              VocabularyKeyDataType.Uri, VocabularyKeyVisibility.HiddenInFrontendUI));
            this.ThumbnailUrl               = this.Add(new VocabularyKey("thumbnailUrl",            VocabularyKeyDataType.Uri, VocabularyKeyVisibility.Hidden));
            this.Email                      = this.Add(new VocabularyKey("email",                   VocabularyKeyDataType.Email));
            this.Emails                     = this.Add(new VocabularyKey("emails",                  VocabularyKeyDataType.Email));

            this.SocialFacebook             = this.Add(new VocabularyKey("socialFacebook", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialFoursquare           = this.Add(new VocabularyKey("socialFoursquare", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialGoogle               = this.Add(new VocabularyKey("socialGoogle", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialLinkedIn             = this.Add(new VocabularyKey("socialLinkedIn", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialTwitter              = this.Add(new VocabularyKey("socialTwitter", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialYouTube              = this.Add(new VocabularyKey("socialYouTube", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialBlogger              = this.Add(new VocabularyKey("socialBlogger", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialFlickr               = this.Add(new VocabularyKey("socialFlickr", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialGoodReads            = this.Add(new VocabularyKey("socialGoodReads", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialTripIt               = this.Add(new VocabularyKey("socialTripIt", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialTumblr               = this.Add(new VocabularyKey("socialTumblr", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialVimeo                = this.Add(new VocabularyKey("socialVimeo", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialWordPress            = this.Add(new VocabularyKey("socialWordPress", VocabularyKeyVisibility.HiddenInFrontendUI));
            this.SocialYahoo                = this.Add(new VocabularyKey("socialYahoo", VocabularyKeyVisibility.HiddenInFrontendUI));

            this.CurrencyBitcoin            = this.Add(new VocabularyKey("currencyBitcoin"));
            this.CurrencyLitecoin           = this.Add(new VocabularyKey("currencyLitecoin"));
            this.CurrencyDogecoin           = this.Add(new VocabularyKey("currencyDogecoin"));

            this.MessagingAIM               = this.Add(new VocabularyKey("messagingAIM"));
            this.MessagingYahoo             = this.Add(new VocabularyKey("messagingYahoo"));
            this.MessagingIcq               = this.Add(new VocabularyKey("messagingIcq"));
            this.MessagingGtalk             = this.Add(new VocabularyKey("messagingGtalk"));
            this.MessagingSkype             = this.Add(new VocabularyKey("messagingSkype"));

            this.PhoneNumberMobile          = this.Add(new VocabularyKey("phoneNumberMobile",       VocabularyKeyDataType.PhoneNumber));
            this.PhoneNumberHome            = this.Add(new VocabularyKey("phoneNumberHome",         VocabularyKeyDataType.PhoneNumber));
            this.PhoneNumberWork            = this.Add(new VocabularyKey("phoneNumberWork",         VocabularyKeyDataType.PhoneNumber));

            this.AddMapping(this.NameFamilyName, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.LastName);
            this.AddMapping(this.NameGivenName, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.FirstName);
            this.AddMapping(this.NameFormatted, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.FullName);
            this.AddMapping(this.CurrentLocation, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Location);
            this.AddMapping(this.Email, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email);
            this.AddMapping(this.Emails, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.EmailAddresses);

            this.AddMapping(this.SocialFacebook, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialFacebook);
            this.AddMapping(this.SocialFoursquare, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialFoursquare);
            this.AddMapping(this.SocialGoogle, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialGooglePlus);
            this.AddMapping(this.SocialLinkedIn, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialLinkedIn);
            this.AddMapping(this.SocialTwitter, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialTwitter);
            this.AddMapping(this.SocialYouTube, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialYouTube);
            this.AddMapping(this.SocialBlogger, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialBlogger);
            this.AddMapping(this.SocialFlickr, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialFlickr);
            this.AddMapping(this.SocialGoodReads, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialGoodReads);
            this.AddMapping(this.SocialTripIt, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialTripIt);
            this.AddMapping(this.SocialTumblr, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialTumblr);
            this.AddMapping(this.SocialVimeo, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialVimeo);
            this.AddMapping(this.SocialWordPress, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialWordPress);
            this.AddMapping(this.SocialYahoo, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialYahoo);

            this.AddMapping(this.MessagingAIM, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.MessagingAIM);
            this.AddMapping(this.MessagingYahoo, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.MessagingYahoo);
            this.AddMapping(this.MessagingIcq, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.MessagingICQ);
            this.AddMapping(this.MessagingGtalk, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.MessagingGoogleTalk);
            this.AddMapping(this.MessagingSkype, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.MessagingSkype);

            this.AddMapping(this.PhoneNumberMobile, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.MobileNumber);
            this.AddMapping(this.PhoneNumberHome, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomePhoneNumber);
            this.AddMapping(this.PhoneNumberWork, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.PhoneNumber);

            //this.AddMapping(this.Domain, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);
        }

        public VocabularyKey AboutMe { get; protected set; }
        public VocabularyKey CurrentLocation { get; protected set; }
        public VocabularyKey DisplayName { get; protected set; }
        public VocabularyKey Hash { get; protected set; }
        public VocabularyKey Id { get; protected set; }
        public VocabularyKey NameFamilyName { get; protected set; }
        public VocabularyKey NameGivenName { get; protected set; }
        public VocabularyKey NameFormatted { get; protected set; }
        public VocabularyKey PreferredUsername { get; protected set; }
        public VocabularyKey RequestHash { get; protected set; }
        public VocabularyKey ProfileBackgroundColor { get; protected set; }
        public VocabularyKey ProfileBackgroundUrl { get; protected set; }
        public VocabularyKey ProfileUrl { get; protected set; }
        public VocabularyKey ThumbnailUrl { get; protected set; }
        public VocabularyKey Email { get; protected set; }
        public VocabularyKey Emails { get; protected set; }

        public VocabularyKey SocialFacebook { get; protected set; }
        public VocabularyKey SocialFoursquare { get; protected set; }
        public VocabularyKey SocialGoogle { get; protected set; }
        public VocabularyKey SocialLinkedIn { get; protected set; }
        public VocabularyKey SocialTwitter { get; protected set; }
        public VocabularyKey SocialYouTube { get; protected set; }
        public VocabularyKey SocialBlogger { get; protected set; }
        public VocabularyKey SocialFlickr { get; protected set; }
        public VocabularyKey SocialGoodReads { get; protected set; }
        public VocabularyKey SocialTripIt { get; protected set; }
        public VocabularyKey SocialTumblr { get; protected set; }
        public VocabularyKey SocialVimeo { get; protected set; }
        public VocabularyKey SocialWordPress { get; protected set; }
        public VocabularyKey SocialYahoo { get; protected set; }

        public VocabularyKey CurrencyBitcoin { get; protected set; }
        public VocabularyKey CurrencyLitecoin { get; protected set; }
        public VocabularyKey CurrencyDogecoin { get; protected set; }

        public VocabularyKey MessagingAIM { get; protected set; }
        public VocabularyKey MessagingYahoo { get; protected set; }
        public VocabularyKey MessagingIcq { get; protected set; }
        public VocabularyKey MessagingGtalk { get; protected set; }
        public VocabularyKey MessagingSkype { get; protected set; }

        public VocabularyKey PhoneNumberMobile { get; protected set; }
        public VocabularyKey PhoneNumberHome { get; protected set; }
        public VocabularyKey PhoneNumberWork { get; protected set; }
    }
}
