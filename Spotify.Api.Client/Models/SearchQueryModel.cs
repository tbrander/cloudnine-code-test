using System.ComponentModel.DataAnnotations;

namespace Spotify.Api.Client.Models
{
    /// <summary>
    /// Search query model
    /// </summary>
    public class SearchQueryModel
    {
        [Required]
        public string Query { get; set; }

        /// <summary>
        /// The type of recommendation (artist, genre or track)
        /// </summary>
        [Required]
        public string SeedType { get; set; }
    }
}