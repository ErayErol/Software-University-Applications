namespace MiniFootball.Services.Cities
{
    public interface ICityService
    {
        int CityIdByName(string name);
        
        int Create(
            string name, 
            string countryName, 
            int adminId);
    }
}
