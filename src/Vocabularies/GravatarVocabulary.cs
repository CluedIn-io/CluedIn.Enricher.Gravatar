// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravatarVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the GravatarVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CluedIn.ExternalSearch.Providers.Gravatar.Vocabularies
{
    /// <summary>The gravatar vocabulary.</summary>
    public static class GravatarVocabulary
    {
        /// <summary>
        /// Initializes static members of the <see cref="GravatarVocabulary" /> class.
        /// </summary>
        static GravatarVocabulary()
        {
            User = new GravatarUserVocabulary();
        }

        /// <summary>Gets the organization.</summary>
        /// <value>The organization.</value>
        public static GravatarUserVocabulary User { get; private set; }
    }
}