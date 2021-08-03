namespace MiniFootball.Services.Playgrounds
{
    using System.Collections.Generic;

    public class PlaygroundQueryServiceModel
    {
        public int TotalPlaygrounds { get; set; }

        public IEnumerable<PlaygroundServiceModel> Playgrounds { get; set; }
    }
}
