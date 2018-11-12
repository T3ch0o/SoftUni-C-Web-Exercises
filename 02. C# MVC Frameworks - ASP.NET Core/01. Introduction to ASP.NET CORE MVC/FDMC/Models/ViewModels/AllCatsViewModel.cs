namespace FDMC.Models.ViewModels
{
    using System.Collections.Generic;

    public class AllCatsViewModel
    {
        public ICollection<Cat> Cats { get; set; }
    }
}