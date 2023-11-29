using CrudOperation.Models;
using CrudOperation.VM;

namespace CrudOperation.Reprositry
{
    public interface IMovieReprositry
    {
         Task<List<Movie>> GetAll();
        Movie GetById(int? id);
        void Delete(int? id);
        void save();
        void Add(Movie vm);
        List<Movie> Search(string name);
    }
}
