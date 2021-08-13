namespace MiniFootball.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.Fields;
    using Models.Games;
    using Services.Fields;
    using Services.Games.Models;
    using Services.Users;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameListingServiceModel>();
            CreateMap<Game, GameIdUserIdServiceModel>();

            //CreateMap<GameDetailsServiceModel, GameFormModel>();

            CreateMap<GameDetailsServiceModel, GameEditServiceModel>()
                .ForMember(gE => gE.GameId, cfg => cfg.MapFrom(gD => gD.Id));

            CreateMap<CreateGameSecondStepViewModel, CreateGameLastStepViewModel>();

            CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId));

            CreateMap<Game, GameIdUserIdServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId))
                .ForMember(gDSM => gDSM.GameId, cfg => cfg.MapFrom(g => g.Id));

            CreateMap<Field, FieldServiceModel>();
            CreateMap<Field, FieldListingServiceModel>();
            CreateMap<Field, FieldDetailServiceModel>();
            CreateMap<FieldDetailServiceModel, FieldFormModel>();

            CreateMap<User, UserDetailsServiceModel>();
        }
    }
}
