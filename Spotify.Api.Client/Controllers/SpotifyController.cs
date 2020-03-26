using Spotify.Api.Web.Models;
using Spotify.Api.Service.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using Spotify.Api.Web.Extensions;
using System;

namespace Spotify.Api.Web.Controllers
{
    /// <summary>
    /// Controller for the Spotify API
    /// </summary>
    public class SpotifyController : Controller
    {
        private ISpotifyService _spotifyService;
        public SpotifyController(ISpotifyService spotifyService)
        {
            this._spotifyService = spotifyService;
        }

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
                try
                {
                    return View(await _spotifyService.SearchAsync(searchModel.Query, searchModel.SeedType).ToViewModel());
                }
                catch (Exception e)
                {

                    TempData["ErrorMessage"] = e.Message;
                    return View("Error");
                }
            }

            return View("Search", searchModel);
        }


        /// <summary>
        /// returns toptracks for the specified spotifyId and country
        /// </summary>
        /// <param name="spotifyId">Returns toptracks for this id</param>
        /// <param name="countryCode">returns toptracks in the country</param>
        /// <returns></returns>
        [HttpGet]
        [Route("TopTracks")]
        public async Task<ActionResult> TopTracks(string spotifyId, string countryCode = "SE")
        {
            if (!string.IsNullOrWhiteSpace(spotifyId))
            {
                try
                {
                    return PartialView(await _spotifyService.TopTracksAsync(spotifyId, countryCode).ToViewModel());
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return View("Error");
                }
            }

            TempData["ErrorMessage"] = "Something went wrong, try again.";

            return View("Error");
        }
    }
}
