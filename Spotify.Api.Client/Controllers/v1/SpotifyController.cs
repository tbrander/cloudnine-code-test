using Microsoft.Web.Http;
using Spotify.Api.Client.Helpers;
using Spotify.Api.Client.Models;
using Spotify.Api.Client.Models.RecommendationModel;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Spotify.Api.Client.Controllers.v1
{
    /// <summary>
    /// Controller for the Spotify API
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("v{version:apiVersion}/Spotify")]
    public class SpotifyController : Controller
    {
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new SearchQueryModel());
        }

        /// <summary>
        /// Returns search results from the Search API
        /// </summary>
        // GET v1/spotify/recommendation
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("Recommendation")]
        public ActionResult Recommendation(SearchQueryModel searchModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Search", searchModel);
            }

            try
            {
                Task<RecommendationViewModel> recommendationTask = Task.Run(async () => await new ApiHelper().Search(searchModel.Query, searchModel.SeedType));
                recommendationTask.Wait();
                
                return View(recommendationTask.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return View();
        }
    }
}
