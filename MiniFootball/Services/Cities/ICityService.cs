namespace MiniFootball.Services.Cities
{
    using Data.Models;

    public interface ICityService
    {
        City City(string name);
        
        int Create(
            string name, 
            string countryName, 
            int adminId);
    }
}
