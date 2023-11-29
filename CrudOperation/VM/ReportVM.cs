using System.ComponentModel.DataAnnotations;

namespace CrudOperation.VM
{
    public class ReportVM
    {
        [MaxLength(2000)]
        public string Report { get; set; }
        public string Email { get; set; }
    }
}
