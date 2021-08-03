namespace MiniFootball.Data
{
    public class DataConstants
    {
        public class ErrorMessages
        {
            public const string Empty = "Cannot be empty!";
            
            public const string Range = "Must be in range {2} and {1}";
        }

        public class User
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 40;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Game
        {
            public const int NumberOfPlayersMin = 8;
            public const int NumberOfPlayersMax = 22;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 200;

            public const int PlacesMin = 8;
            public const int PlacesMax = 22;
        }

        public class Field
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;

            public const int CountryMinLength = 1;
            public const int CountryMaxLength = 56;

            public const int TownMinLength = 1;
            public const int TownMaxLength = 85;

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

        public class Player
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 26;
        }
    }
}
