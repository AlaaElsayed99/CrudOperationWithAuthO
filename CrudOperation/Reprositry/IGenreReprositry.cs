using CrudOperation.Models;

namespace CrudOperation.Reprositry
{
    public interface IGenreReprositry
    {
        List<Genre> GetAll();
        void Add(Genre genre);
        void Update(Genre genre);
        public void Delete(Genre genre);
        public void save();
    }
}
