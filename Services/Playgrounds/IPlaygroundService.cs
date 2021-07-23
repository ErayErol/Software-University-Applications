namespace MessiFinder.Services.Playgrounds
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Games;
    using Models.Playgrounds;

    public interface IPlaygroundService
    {
        void Create(PlaygroundCreateFormModel playgroundModel, int adminId);

        bool CheckForSamePlayground(PlaygroundCreateFormModel playgroundModel);

        public PlaygroundAllQueryModel All(PlaygroundAllQueryModel query);

        bool IsExist(PlaygroundListingViewModel gamePlaygroundModel);

        IEnumerable<PlaygroundListingViewModel> GetPlaygroundViewModels(string town, string country);
    }
}
