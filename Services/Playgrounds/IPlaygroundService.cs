namespace MessiFinder.Services.Playgrounds
{
    using Models.Games;
    using Models.Playgrounds;
    using System.Collections.Generic;

    public interface IPlaygroundService
    {
        void Create(PlaygroundCreateFormModel playgroundModel, int adminId);

        bool CheckForSamePlayground(PlaygroundCreateFormModel playgroundModel);

        public PlaygroundAllQueryModel All(PlaygroundAllQueryModel query);

        bool IsExist(PlaygroundListingViewModel gamePlaygroundModel);

        IEnumerable<PlaygroundListingViewModel> PlaygroundViewModels(string town, string country);
    }
}
