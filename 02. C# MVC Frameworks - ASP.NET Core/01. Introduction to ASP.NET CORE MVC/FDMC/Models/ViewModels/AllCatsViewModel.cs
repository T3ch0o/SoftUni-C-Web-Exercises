namespace FDMC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class AllCatsViewModel
    {
        public IQueryable<Cat> Cats { get; set; }
    }
}