using CrudOperation.Models;

namespace CrudOperation.Reprositry
{
    public interface IReprositryReports
    {
        List<Reports> GetAll();
        void Delete(int id);
        void Save();
        Reports GetById(int id);
        void Create(Reports report);
    }

}
