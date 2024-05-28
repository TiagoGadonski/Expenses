using System.ComponentModel.DataAnnotations;

namespace Expenses.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTime ReminderDateTime { get; set; }

        public bool IsTriggered { get; set; } = false;
    }
}
