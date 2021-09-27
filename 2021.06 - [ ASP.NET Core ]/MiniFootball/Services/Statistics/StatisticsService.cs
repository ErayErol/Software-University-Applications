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
            var totalGames = data.Games.Count(g => g.IsPublic);
            var totalFields = data.Fields.Count(f => f.IsPublic);
            var totalUsers = data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalGames = totalGames,
                TotalFields = totalFields,
                TotalUsers = totalUsers,
            };
        }
    }
}
