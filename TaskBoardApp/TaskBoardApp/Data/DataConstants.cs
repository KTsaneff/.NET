namespace TaskBoardApp.Data
{
    public static class DataConstants
    {
        //User
        public const int UserFirstNameMinLength = 1;
        public const int UserFirstNameMaxLength = 20;

        public const int UserLastNameMinLength = 2;
        public const int UserLastNameMaxLength = 30;

        public const int UserUsernameMinLength = 3;
        public const int UserUsernameMaxLength = 18;

        //Task
        public const int MinTaskTitle = 5;
        public const int MaxTaskTitle = 70;

        public const int MinTaskDescription = 10;
        public const int MaxTaskDescription = 1000;

        //Board
        public const int MinBoardName = 3;
        public const int MaxBoardName = 30;
    }
}
