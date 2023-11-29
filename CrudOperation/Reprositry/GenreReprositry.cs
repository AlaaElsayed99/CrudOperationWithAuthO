using CrudOperation.Models;
using System.ComponentModel;

namespace CrudOperation.Reprositry
{
    public class GenreReprositry: IGenreReprositry
    {
        AppDbContext _context;
        public GenreReprositry(AppDbContext context)
        {

            _context = context;

        }
        public List<Genre> GetAll()
        {
            return _context.Genres.OrderBy(s=>s.Name).ToList();
        }
        public void Add(Genre genre)
        {
            _context.Genres.Add(genre);

        }
        public void Update(Genre genre)
        {

        }
        public void Delete(Genre genre)
        {
            _context.Remove(genre);
        }

        public void save()
        {
            _context.SaveChanges(); 
        }

    }
}
