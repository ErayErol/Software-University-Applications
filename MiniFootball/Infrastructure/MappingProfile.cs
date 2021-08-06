namespace MiniFootball.Infrastructure
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
            this.CreateMap<Game, GameDeleteServiceModel>();
            this.CreateMap<GameDetailsServiceModel, GameFormModel>();

            this.CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId));

            this.CreateMap<Game, GameDeleteServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId));
        }
    }
}
