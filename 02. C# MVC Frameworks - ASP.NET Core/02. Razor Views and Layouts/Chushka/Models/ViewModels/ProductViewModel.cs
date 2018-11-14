namespace Chushka.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public string SelectedFoodType { get; set; }
    }
}
