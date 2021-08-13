namespace MiniFootball.Services.Statistics
{
    using Data;
    using System.Linq;

    public class StatisticsService : IStatisticsService
    {
        private readonly MiniFootballDbContext data;

        public StatisticsService(MiniFootballDbContext data) 
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalGames = data.Games.Count();
            var totalPlaygrounds = data.Fields.Count();
            var totalUsers = data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalGames = totalGames,
                TotalFields = totalPlaygrounds,
                TotalUsers = totalUsers,
            };
        }
    }
}
