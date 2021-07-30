namespace MessiFinder.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.Games;
    using Models.Home;
    using Services.Games.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<GameDetailsServiceModel, GameFormModel>();
            this.CreateMap<Game, GameIndexViewModel>();

            this.CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Admin.UserId));
        }
    }
}
