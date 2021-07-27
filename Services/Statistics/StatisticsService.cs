namespace MessiFinder.Services.Statistics
{
    using Data;
    using System.Linq;

    public class StatisticsService : IStatisticsService
    {
        private readonly MessiFinderDbContext data;

        public StatisticsService(MessiFinderDbContext data)
        {
            this.data = data;
        }


        public StatisticsServiceModel Total()
        {
            var totalGames = this.data.Games.Count();
            var totalPlaygrounds = this.data.Playgrounds.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalGames = totalGames,
                TotalPlaygrounds = totalPlaygrounds,
                TotalUsers = totalUsers,
            };
        }
    }
}
