namespace MiniFootball.Test.Data
{
    using System;
    using MiniFootball.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class Games
    {
        public static Game NewGame()
            => new Game
            {
                Id = "asdasdasdasdasdasdasd",
                Time = 20,
                NumberOfPlayers = 12,
                Date = DateTime.Now.Date.AddDays(1),
                Description = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                FieldId = 1,
                Ball = true,
                FacebookUrl = "https://www.youtube.com/watch?v=5c5m8Su7l1c&list=PL9s7A_reW2Si6zlpekLWSayvHP0-Buh62&index=24&t=5389s",
                Goalkeeper = false,
                Jerseys = true,
                AdminId = 1,
            };

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
