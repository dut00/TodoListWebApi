using System.ComponentModel.DataAnnotations;

namespace TodoListWebApi.Entities
{
    public class TodoTask
    {
        [Key]
        public uint Id { get; set; }
        public uint UserId { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }


        public DateTime? DeadlineDate { get; set; }

        public bool IsDone { get; set; } = false;
    }
}
