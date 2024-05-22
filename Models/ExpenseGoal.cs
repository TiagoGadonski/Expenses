using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models
{
    public class ExpenseGoal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TargetAmount { get; set; }
    }
}
