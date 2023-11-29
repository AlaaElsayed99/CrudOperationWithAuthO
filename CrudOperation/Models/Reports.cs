using System.ComponentModel.DataAnnotations;

namespace CrudOperation.Models
{
    public class Reports
    {
         public int Id { get; set; }
        [MaxLength(2000)]
        public string Report { get; set; }
        public string Email { get; set; }

    }
}
