namespace Library.Common
{
    public static class ValidationConstants
    {
        //Paths
        public const string ErrorPath = "/Home/Error";
        public const string LoginPath = "/User/Login";

        //User
        public const int UserPasswordMinLength = 5;
        public const int UserPasswordMaxLength = 20;
        public const int UserUsernameMinLength = 5;
        public const int UserUsernameMaxLength = 20;
        public const int UserEmailMinLength = 10;
        public const int UserEmailMaxLength = 60;

        //Book
        public const int BookTitleMinLength = 10;
        public const int BookTitleMaxLength = 50;
        public const int BookAuthorMinLength = 5;
        public const int BookAuthorMaxLength = 50;
        public const int BookDescriptionMinLength = 5;
        public const int BookDescriptionMaxLength = 5000;
        public const string BookRatingMinValue = "0.00";
        public const string BookRatingMaxValue = "10.00";

        //Category
        public const int CategoryNameMinLength = 5;
        public const int CategoryNameMaxLength = 50;
    }
}
