using System.ComponentModel.DataAnnotations;

namespace AzureToDo.ApiService.Entities
{
    public sealed class SupportTicket
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
