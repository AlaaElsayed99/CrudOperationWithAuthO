using CrudOperation.Models;

namespace CrudOperation.Reprositry
{
    public class AccountReprositry : IAccountReprositry
    {
        AppDbContext Context;
        public AccountReprositry(AppDbContext _Context)
        {
           Context = _Context;
        }
        public void create(Account account)
        {
            Context.Accounts.Add(account);

        }

        public bool Find(string username, string password)
        {
            Account acc = Context.Accounts.FirstOrDefault(s => s.UserName == username && s.Password == password);
            if (acc != null)
            {
                return true;
            }
            return false;
        }
        public Account Get(string username, string password)
        {
            return Context.Accounts.FirstOrDefault(s => s.UserName == username && s.Password == password);
        }

        public void save()
        {
            Context.SaveChanges();
        }
    }
}
