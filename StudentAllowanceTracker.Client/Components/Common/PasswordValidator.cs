namespace StudentAllowanceTracker.Client.Components.Common
{
    public static class PasswordValidator
    {
        public static string ValidatePasswordStrength(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Password is required";

            if (value.Length < 8)
                return "Password must be at least 8 characters long";

            if (!value.Any(char.IsDigit))
                return "Password must contain at least one number";

            if (!value.Any(ch => !char.IsLetterOrDigit(ch)))
                return "Password must contain at least one special character";

            return null;
        }

        public static string ValidatePasswordMatch(string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return "Please confirm your password";

            if (confirmPassword != password)
                return "Passwords do not match";

            return null;
        }
    }
}
