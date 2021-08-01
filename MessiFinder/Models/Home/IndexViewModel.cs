namespace MessiFinder.Models.Home
{
    using System.Collections.Generic;
    using Services.Games.Models;

    public class IndexViewModel
    {
        public int TotalGames { get; set; }
        
        public int TotalPlaygrounds { get; set; }
        
        public int TotalUsers { get; set; }

        public List<GameListingServiceModel> Games { get; set; }
    }
}
