using BookStoreApp.API.Models.Author;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book
{
    public class BookCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? Year { get; set; }

        public string? Isbn { get; set; }

        [Required]
        [StringLength(150)]
        public string? Summary { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }
    }

    public class BookUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? Year { get; set; }

        public string? Isbn { get; set; }

        [Required]
        [StringLength(150)]
        public string? Summary { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }
    }

    public class BookReturnDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? Year { get; set; }

        public string? Isbn { get; set; }

        [Required]
        [StringLength(150)]
        public string? Summary { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }

        public virtual AuthorReturnDto? Author { get; set; }
    }
}
