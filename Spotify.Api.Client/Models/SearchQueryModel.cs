using System.ComponentModel.DataAnnotations;

namespace Spotify.Api.Client.Models
{
    public class SearchQueryModel
    {
        [Required]
        [Display(Name = "Search query")]
        public string Query { get; set; }

        [Required]
        [Display(Name = "Get recommendations for")]
        public string SeedType { get; set; }
    }
}