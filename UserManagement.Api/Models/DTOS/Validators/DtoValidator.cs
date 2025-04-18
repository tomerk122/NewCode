namespace UserManagement.Api.Models.DTOS.DtoValidator
{
    public static class DtoValidator
    {
        public static List<string> ValidateCreateUserDto(CreateUserDto userDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(userDto.UserName))
            {
                errors.Add("UserName cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                errors.Add("Password cannot be null or empty.");
            }

            if (!string.IsNullOrWhiteSpace(userDto.Email) && !userDto.Email.Contains("@"))
            {
                errors.Add("Email must be a valid email address.");
            }

            if (!string.IsNullOrWhiteSpace(userDto.Phone) && userDto.Phone.Length < 10)
            {
                errors.Add("Phone must contain at least 10 digits.");
            }

            return errors;
        }
    
        public static List<string> ValidateSearchUserDto(SearchUserDto searchUserDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(searchUserDto.FirstName) && string.IsNullOrWhiteSpace(searchUserDto.LastName))
            {
                errors.Add("At least one of FirstName or LastName must be provided.");
            }

            return errors;
        }


        public static List<string> ValidateCredentialsDto(Credentials credtDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(credtDto.UserName))
            {
                errors.Add("UserName cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(credtDto.Password))
            {
                errors.Add("Password cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(credtDto.Company))
            {
                errors.Add("Company cannot be null or empty.");
            }


            return errors;
        }
    }
}
