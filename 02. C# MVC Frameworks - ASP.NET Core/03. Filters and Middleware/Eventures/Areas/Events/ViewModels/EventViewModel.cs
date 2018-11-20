namespace Eventures.Areas.Event.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EventViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [MinLength(10, ErrorMessage = "Name should be minimum 10 symbols long.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Place")]
        public string Place { get; set; }

        [Required]
        [Display(Name = "Start")]
        [DataType(DataType.DateTime, ErrorMessage = "Incorrect data format")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "End")]
        [DataType(DataType.DateTime, ErrorMessage = "Incorrect data format")]
        public DateTime End { get; set; }

        [Required]
        [Display(Name = "Total Tickets")]
        [Range(0, int.MaxValue, ErrorMessage = "Total Tickets should be a positive number.")]
        public int TotalTickets { get; set; }

        [Required]
        [Display(Name = "Price Per Ticket")]
        [DataType(DataType.Currency, ErrorMessage = "Ticket Price must be a number.")]
        public decimal TicketPrice { get; set; }
    }
}
