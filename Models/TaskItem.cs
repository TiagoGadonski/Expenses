using System.ComponentModel.DataAnnotations;

namespace Expenses.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int TaskCategoryId { get; set; }
        public TaskCategory TaskCategory { get; set; }

        [Required]
        public int TaskColumnId { get; set; }
        public TaskColumn TaskColumn { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
