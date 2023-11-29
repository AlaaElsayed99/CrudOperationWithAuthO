using System.ComponentModel.DataAnnotations;

namespace CrudOperation.VM
{
    public class ChangePassword
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }
       

    }
}
