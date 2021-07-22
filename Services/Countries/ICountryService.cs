namespace MessiFinder.Services.Countries
{
    using System.Collections.Generic;

    public interface ICountryService
    {
        IEnumerable<string> GetAll();
    }
}
