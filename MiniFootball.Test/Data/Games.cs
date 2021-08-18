namespace MiniFootball.Test.Data
{
    using MiniFootball.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class Games
    {
        public static IEnumerable<Game> TenPublicGames()
            => Enumerable
                .Range(0, 10)
                .Select(i => new Game
                {
                    Field = new Field(),
                    IsPublic = true,
                });
    }
}
