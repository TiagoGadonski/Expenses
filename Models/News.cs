using System.ComponentModel.DataAnnotations;

namespace Expenses.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        [StringLength(200)]
        public string VideoUrl { get; set; }

        [Required]
        public int NewsCategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }
    }
}
