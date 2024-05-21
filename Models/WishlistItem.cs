using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }

        [ForeignKey("CategoryWishlist")]
        public int CategoryId { get; set; }
        public CategoryWishlist Category { get; set; }

        public bool IsPurchased { get; set; }
    }
}
