namespace MiniFootball.Services.Countries
{
    using System.Collections.Generic;
    using Data.Models;

    public interface ICountryService
    {
        List<string> All();
        
        void SaveAll();

        string Name(int id);

        Country Country(string name);
        
        IEnumerable<string> Cities(string countryName);
    }
}
