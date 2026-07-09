using Microsoft.AspNetCore.Identity;

namespace AspIdentityOrnek.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
    }
}
