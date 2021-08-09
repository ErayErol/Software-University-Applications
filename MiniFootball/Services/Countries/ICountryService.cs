namespace MiniFootball.Services.Countries
{
    using System.Collections.Generic;

    public interface ICountryService
    {
        List<string> All();
        
        void SaveAll();

        string Name(int id);
    }
}
