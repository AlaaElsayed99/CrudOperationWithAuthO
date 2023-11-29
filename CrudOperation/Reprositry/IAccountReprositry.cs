using CrudOperation.Models;

namespace CrudOperation.Reprositry
{
    public interface IAccountReprositry
    {
        bool Find(string username, string password);
        void create(Account account);
        Account Get(string username, string password);
        void save();
    }
}
