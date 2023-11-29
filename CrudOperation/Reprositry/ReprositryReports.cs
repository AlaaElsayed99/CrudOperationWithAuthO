using CrudOperation.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CrudOperation.Reprositry
{
    public class ReprositryReports : IReprositryReports
    {
        public AppDbContext Context { get; set; }
        public ReprositryReports(AppDbContext context)
        {
            Context = context;
        }



        public List<Reports> GetAll()
        {
            return Context.Reports.ToList();
        }
        public void Delete(int id)
        {
            var report = Context.Reports.FirstOrDefault(r => r.Id == id);
            
                Context.Reports.Remove(report);
            
           
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        public Reports GetById(int id)
        {
            return Context.Reports.FirstOrDefault(s => s.Id == id);
        }
        public void Create(Reports report)
        {
            
            Context.Reports.Add(report);
        }

    }
}
