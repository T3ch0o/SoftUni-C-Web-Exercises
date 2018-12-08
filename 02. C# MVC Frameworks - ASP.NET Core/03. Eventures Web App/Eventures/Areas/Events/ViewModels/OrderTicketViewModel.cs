namespace Eventures.Areas.Events.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class OrderTicketViewModel
    {
        public string EventId { get; set; }

        [Required]
        [Display(Name = "Tickets")]
        [Range(0, int.MaxValue, ErrorMessage = "Tickets should be a positive number.")]
        public int Tickets { get; set; }
    }
}
