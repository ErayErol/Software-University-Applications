namespace MiniFootball.Services.Countries
{
    using System.Collections.Generic;

    public interface ICountryService
    {
        IEnumerable<string> All();
    }
}
