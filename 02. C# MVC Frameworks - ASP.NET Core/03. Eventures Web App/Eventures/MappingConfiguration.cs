namespace Eventures
{
    using AutoMapper;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Models;
    using Eventures.Models.ViewModels;

    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<RegisterViewModel, ApplicationUser>();
            CreateMap<Event, EventViewModel>();
        }
    }
}
