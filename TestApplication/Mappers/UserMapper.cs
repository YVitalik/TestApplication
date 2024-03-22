using TestApplication.Models;

namespace TestApplication.Mappers
{
    public static class UserMapper
    {
        public static User MapFromRandomUser(dynamic input, string sourceId)
        {
            return new User
            {
                FirstName = input.name.first,
                LastName = input.name.last,
                Email = input.email,
                SourceId = sourceId
            };
        }

        public static User MapFromJsonPlaceholder(dynamic input, string sourceId)
        {
            string fullName = input.name;

            string firstName = fullName.Split(' ')[0];
            string lastName = fullName.Split(' ')[1];

            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = input.email,
                SourceId = sourceId
            };
        }

        public static User MapFromDummyJson(dynamic input, string sourceId)
        {
            return new User
            {
                FirstName = input.firstName,
                LastName = input.lastName,
                Email = input.email,
                SourceId = sourceId
            };
        }

        public static User MapFromReqres(dynamic input, string sourceId)
        {
            return new User
            {
                FirstName = input.first_name,
                LastName = input.last_name,
                Email = input.email,
                SourceId = sourceId
            };
        }
    }
}
