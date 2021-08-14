namespace MiniFootball.Services.Countries
{
    using System.Collections.Generic;

    public interface ICountryService
    {
        List<string> All();
        
        int CountryIdByName(string name);

        void SaveAll();

        string Name(int id);
    }
}
