// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravatarResult.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the GravatarResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Hyldahl.GravatarClient;

namespace CluedIn.ExternalSearch.Providers.Gravatar.Model
{
    /// <summary>The gravatar result.</summary>
    public class GravatarResult
    {
        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarResult"/> class.
        /// </summary>
        public GravatarResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarResult"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="profile">The profile.</param>
        public GravatarResult(string email, ProfileEntry profile)
        {
            this.Email   = email;
            this.Profile = profile;
        }

        /**********************************************************************************************************
         * PROPERTIES
         **********************************************************************************************************/

        public string Email { get; set; }
        public ProfileEntry Profile { get; set; }
    }
}
