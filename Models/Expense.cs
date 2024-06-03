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

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public int? Installments { get; set; }
        public int? CurrentInstallment { get; set; }
        public bool IsPaidThisMonth { get; set; }
        public DateTime? LastPaymentDate { get; set; }

        [NotMapped]
        public bool IsPaidForCurrentMonth
        {
            get
            {
                // Considera que o status de pagamento deve ser false no primeiro dia do mês
                if (DateTime.Now.Day == 1)
                {
                    return false;
                }

                return LastPaymentDate.HasValue &&
                       LastPaymentDate.Value.Month == DateTime.Now.Month &&
                       LastPaymentDate.Value.Year == DateTime.Now.Year;
            }
        }
    }

}
