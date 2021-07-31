namespace MessiFinder.Models.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalGames { get; set; }
        
        public int TotalPlaygrounds { get; set; }
        
        public int TotalUsers { get; set; }

        public List<GameIndexViewModel> Games { get; set; }
    }
}
