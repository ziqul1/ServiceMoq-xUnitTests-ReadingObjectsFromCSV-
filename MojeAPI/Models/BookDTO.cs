using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojeAPI.Models
{
    public class BookDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
