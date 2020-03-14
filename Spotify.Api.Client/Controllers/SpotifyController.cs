using Spotify.Api.Client.Helpers;
using Spotify.Api.Client.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Spotify.Api.Client.Controllers
{
    /// <summary>
    /// Controller for the Spotify API
    /// </summary>
    public class SpotifyController : Controller
    {
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Search page
        /// </summary>
        // GET /spotify/search
        [HttpGet]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new SearchQueryModel());
        }

        /// <summary>
        /// Returns search results from the Spotify Search API
        /// </summary>
        // POST /spotify/recommendation
        [HttpPost]
        [Route("Recommendation")]
        public async Task<ActionResult> Recommendation(SearchQueryModel searchModel)
        {
            if (ModelState.IsValid)
            {
                return View(await new ApiHelper().Search(searchModel.Query, searchModel.SeedType));
            }

            return View("Search", searchModel);
        }

        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> TopTracks(string spotifyId, string countryCode = "SE")
        {
            if (!string.IsNullOrWhiteSpace(spotifyId))
            {
                return PartialView(await new ApiHelper().TopTracks(spotifyId, countryCode));
            }

            throw new System.Web.HttpException(400, "Bad Request");
        }
    }
}
