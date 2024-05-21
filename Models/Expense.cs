using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("CategoryFinance")]
        public int CategoryId { get; set; }
        public CategoryFinance Category { get; set; }

        public int? Installments { get; set; }
        public int? CurrentInstallment { get; set; }
        public bool IsPaidThisMonth { get; set; }
        public DateTime? LastPaymentDate { get; set; }
    }
}
