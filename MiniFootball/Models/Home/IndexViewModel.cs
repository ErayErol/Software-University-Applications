namespace MiniFootball.Models.Home
{
    using System.Collections.Generic;
    using Services.Games.Models;

    public class IndexViewModel
    {
        public int TotalGames { get; set; }
        
        public int TotalFields { get; set; }
        
        public int TotalUsers { get; set; }

        public IList<GameListingServiceModel> Games { get; set; }
    }
}
