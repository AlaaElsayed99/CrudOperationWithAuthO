using CrudOperation.Models;
using CrudOperation.Reprositry;
using CrudOperation.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace CrudOperation.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private long _maxAllowedPosterSize = 1048576;

        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private readonly IToastNotification Toasnoty;
       // private readonly AppDbContext _context;
        IMovieReprositry MovieReprositry;
        IGenreReprositry GenreReprositry;
        public MoviesController(AppDbContext dbContext,IMovieReprositry movieReprositry,IGenreReprositry genreReprositry, IToastNotification _Toasnoty)
        {

            //_context=dbContext;
            MovieReprositry = movieReprositry;
            GenreReprositry=genreReprositry;
            Toasnoty = _Toasnoty;
        }
        public async Task< IActionResult> Index()
        {
            return View(await MovieReprositry.GetAll());
        }
        public async Task<IActionResult> Create()
        {
            
            ViewBag.genre = new SelectList(GenreReprositry.GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieVM movie)
        {
            ViewBag.genre = new SelectList(GenreReprositry.GetAll(), "Id", "Name");
            if (!ModelState.IsValid == true)
            {
                
                return View(movie);
            }
            var Files = Request.Form.Files;
            if (!Files.Any())
            {
                ModelState.AddModelError("Poster","Select any Poster");
                return View(movie);
            }
            var poster = Files.FirstOrDefault();
            var allowedE = new List<string> { ".jpg", ".png" };
            if (!allowedE.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                ModelState.AddModelError("Poster", "Select allowed Extension");
                return View(movie);
            }
            if(poster.Length>1048576)
            {
                ModelState.AddModelError("Poster", "can't be more than 1 mg");
                return View(movie);
            }
            using var datastream = new MemoryStream();
            await poster.CopyToAsync(datastream);

            var movies = new Movie
            {
                Title = movie.Title,
                GenreId= movie.GenreId,
                Year= movie.Year,
                rate=movie.rate,
                StoryLine=movie.StoryLine,
                Poster=datastream.ToArray(),
            };
            MovieReprositry.Add(movies);
            MovieReprositry.save();
            Toasnoty.AddSuccessToastMessage("Succesfully Created");
            return RedirectToAction(nameof(Index));
            

        }
        public async Task<IActionResult> Edit(int? Id)
        {
            ViewBag.genre = new SelectList(GenreReprositry.GetAll(), "Id", "Name");
            
            if (Id == null)
                return BadRequest();
            var movie = MovieReprositry.GetById(Id);
            if (movie == null)
                return NotFound();
            var Movie = new MovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                Year = movie.Year,
                rate = movie.rate,
                StoryLine = movie.StoryLine,
                Poster = movie.Poster
            };
            return View(Movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieVM mv)
        {
            ViewBag.genre = new SelectList(GenreReprositry.GetAll(), "Id", "Name");
            if (!ModelState.IsValid == true)
            {
                return View(mv);
            }


            var movie = MovieReprositry.GetById(mv.Id);
            if (movie == null)
                return BadRequest();
            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await poster.CopyToAsync(dataStream);

                mv.Poster = dataStream.ToArray();

                if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {

                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                    return View(mv);
                }

                if (poster.Length > _maxAllowedPosterSize)
                {

                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return View(mv);
                }
                movie.Poster = dataStream.ToArray();
            }

                movie.Title = mv.Title;
                movie.GenreId = mv.GenreId;
                movie.Year = mv.Year;
                movie.rate = mv.rate;
                movie.StoryLine = mv.StoryLine;
                MovieReprositry.save();
                Toasnoty.AddSuccessToastMessage("Succesfully Edited");
                return RedirectToAction(nameof(Index));
              
        }
        public IActionResult Details(int Id)
        {
           var movie= MovieReprositry.GetById(Id);
             return View (movie);
        }
        public IActionResult Delete(int? id)
        {
            
            MovieReprositry.Delete(id);
            MovieReprositry.save();
            return Ok();

        }
        [HttpPost]
        public async Task<IActionResult> search(string name)
        {
            ViewBag.name=name;
            List<Movie> movies= MovieReprositry.Search(name);
            return View("Index",movies);
        }
    }

}
