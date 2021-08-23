namespace MiniFootball.Data
{
    public class DataConstants
    {
        public const string PhoneNumber = "Phone Number";

        public const string ImageUrl = "Image URL";
        
        public const string FacebookUrl = "Facebook URL";

        public class ErrorMessages
        {
            public const string Empty = "Cannot be empty!";
            
            public const string Url = "URL is incorrect!";

            public const string Range = "Must be in range {2} and {1}";
        }

        public class User
        {
            public const string Image = "Image";
            public const string FirstName = "First Name";
            public const string LastName = "Last Name";
            public const string NickName = "Nick Name";


            public const int FullNameMinLength = 2;
            public const int FullNameMaxLength = 40;
            
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Country
        {
            public const string CountryName = "Country";

            public const string SelectCountry = "Select Country:";
        }

        public class City
        {
            public const string CityName = "City";
        }

        public class Game
        {
            public const string AvailablePlaces = "Available Places";
            
            public const string SelectDate = "Select Date:";
            
            public const string SelectTime = "Set time:";
            
            public const string NumberOfPlayers = "Number of players";

            public const int NumberOfPlayersMin = 8;
            public const int NumberOfPlayersMax = 22;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 200;

            public const int PlacesMin = 8;
            public const int PlacesMax = 22;

            public const int PhoneNumberMinLength = 8;
            public const int PhoneNumberMaxLength = 15;

            public const int TimeMin = 1;
            public const int TimeMax = 24;
        }

        public class Field
        {
            public const string SelectField = "Select Field:";
            
            public const string FieldName = "Field";
            
            public const string FieldImage = "Image";

            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;

            public const int CountryMinLength = 1;
            public const int CountryMaxLength = 56;

            public const int CityMinLength = 1;
            public const int CityMaxLength = 85;

            public const int AddressMinLength = 1;
            public const int AddressMaxLength = 100;

            public const int PhoneNumberMinLength = 8;
            public const int PhoneNumberMaxLength = 15;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;
        }

        public class Admin
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;

            public const int PhoneNumberMinLength = 8;
            public const int PhoneNumberMaxLength = 15;
        }
    }
}
