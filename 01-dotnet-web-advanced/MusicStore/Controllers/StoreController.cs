using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicStore.Filters;
using MusicStore.Models;
using MusicStore.Services;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IMusicService _musicService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(IOptions<AppSettings> options, IMusicService musicService, ILogger<StoreController> logger)
        {
            _appSettings = options.Value;
            //_musicService = musicService.First();
            _musicService = musicService;
            _logger = logger;
        }

        // GET: /Store/
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var genres = await _musicService.BrowseGenres();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco
        public async Task<IActionResult> Browse(string genre)
        {
            // Retrieve Genre genre and its Associated associated Albums albums from database
            var genreModel = await _musicService.BrowseGenre(genre);

            _logger.LogInformation($"Service created at {_musicService.Created}");

            if (genreModel == null)
            {
                return NotFound();
            }

            return View(genreModel);
        }

        [ServiceFilter(typeof(MyResultFilter))]
        [ServiceFilter(typeof(AuditTrailsActionFilter), Order = 2)]
        [ServiceFilter(typeof(OrderActionFilter), Order = 1)]
        [ServiceFilter(typeof(MyCustomActionFilter), Order = 3)]
        public async Task<IActionResult> Details(
            [FromServices] IMemoryCache cache,
            int id)
        {
            if (id == 55555)
            {
                throw new Exception($"This song (Id={id}) is prohibit, contact Admin to know the reasons");
                //_logger.LogError($"Exception: This song (Id={id}) is prohibit, contact Admin for more");
                //return View("~/Views/Shared/Error.cshtml");
            }

            _logger.LogInformation("File Album detail");
            Album album = await _musicService.GetAlbumDetailAsync(cache, id);
            
            if (album == null)
            {
                _logger.LogError("Can't found album");
                return NotFound();
            }

            return View(album);
        }
    }
}
