namespace MessiFinder.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.Games;
    using Services.Games.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Game, GameListingServiceModel>();
            this.CreateMap<GameDetailsServiceModel, GameFormModel>();

            this.CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId));
        }
    }
}
