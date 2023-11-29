using CrudOperation.Models;
using System.ComponentModel.DataAnnotations;

namespace CrudOperation.VM
{
    public class MovieVM
    {
        public int Id { get; set; }
        [Required, StringLength(250)]
        public string Title { get; set; }
        [Range(1,10)]
        public double rate { get; set; }
        public int Year { get; set; }
        [Required, MaxLength(2500)]
        public string StoryLine { get; set; }
        [Display(Name = "Select poster")]
        public byte[]? Poster { get; set; }
        [Display(Name ="Genre")]
        public int GenreId { get; set; }
        public IEnumerable<Genre>? genres { get; set; }
    }
}
