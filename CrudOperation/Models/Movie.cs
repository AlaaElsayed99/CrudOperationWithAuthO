using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudOperation.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; }
        public double rate { get; set; }
        public int Year { get; set; }
        [Required, MaxLength(2500)]
        public string StoryLine { get ; set; }
        public byte[]? Poster { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre? Genres { get; set; }
    }
}
