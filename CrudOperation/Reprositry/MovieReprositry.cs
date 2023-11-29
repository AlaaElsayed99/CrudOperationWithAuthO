using CrudOperation.Models;
using CrudOperation.VM;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperation.Reprositry
{
    public class MovieReprositry:IMovieReprositry
    {
        AppDbContext _context;
        public Guid id { get; set; }
        public MovieReprositry(AppDbContext dbContext)
        {
            _context = dbContext;
            id= Guid.NewGuid();
        }
        public async Task<List<Movie>> GetAll()
        {
            return  _context.Movies.OrderByDescending(s=>s.rate).ToList();
        }
        public Movie GetById(int? Id)
        {
            var movie= _context.Movies.Include(s=>s.Genres).FirstOrDefault(x => x.Id == Id);
            return movie;
        }
        public void Delete(int? id )
        {
            var movie = _context.Movies.FirstOrDefault(s => s.Id == id);
            _context.Movies.Remove(movie);
            
                
        }
        public void save()
        {
            _context.SaveChanges();
        }
        public void Add(Movie vm)
        {
            _context.Movies.Add(vm);
        }
        public List<Movie> Search(string name)
        {
            var List= _context.Movies.Where(s=>s.Title.Contains(name)).ToList();
            return List;
        }
        
        
    }
}
