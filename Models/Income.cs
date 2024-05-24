using System.ComponentModel.DataAnnotations;

namespace Expenses.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; } // Chave primária

        [Required]
        public string Source { get; set; } // Fonte da receita, como salário, investimento, etc.

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; } // Valor da receita
        [StringLength(2)]
        public string DateReceived { get; set; } // Data em que a receita foi recebida

        public string Description { get; set; } // Descrição adicional ou notas sobre a receita

    }
}
