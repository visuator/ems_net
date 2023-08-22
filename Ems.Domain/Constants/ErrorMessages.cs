namespace Ems.Domain.Constants;

public static class ErrorMessages
{
    public static class Swagger
    {
        public const string NoLoginEndpoint = "NO_LOGIN_ENDPOINT";
    }
    public static class StudentRecordSession
    {
        public const string IsNotExists = "STUDENT_RECORD_SESSION_NOT_EXISTS";
    }

    public static class Classroom
    {
        public const string IsNotExists = "CLASSROOM_NOT_EXISTS";
    }

    public static class Lesson
    {
        public const string IsNotExists = "LESSON_NOT_EXISTS";
    }

    public static class Lecturer
    {
        public const string IsNotExists = "LECTURER_NOT_EXISTS";
    }

    public static class Class
    {
        public const string IsNotExists = "CLASS_NOT_EXISTS";
    }

    public static class Job
    {
        public const string NullModel = "JOB_NULL_MODEL";
    }

    public static class Group
    {
        public const string IsNotExists = "GROUP_NOT_EXISTS";
    }

    public static class ClassVersion
    {
        public const string SettingNotExists = "CLASS_VERSION_PUBLICATION_SETTING_NOT_EXISTS";
        public const string ClassDayDoesNotMatch = "CLASS_VERSION_CLASS_DAY_NOT_MATCH";
    }

    public static class System
    {
        public const string InvalidJwt = "INVALID_JWT";
    }

    public static class Account
    {
        public const string IsNotExists = "ACCOUNT_NOT_EXISTS";
        public const string IsNotConfirmed = "ACCOUNT_NOT_CONFIRMED";
        public const string IsLocked = "ACCOUNT_LOCKED";
        public const string InvalidPassword = "ACCOUNT_INVALID_PASSWORD";
        public const string ConfirmationTokenIsNotExists = "ACCOUNT_CONFIRMATION_TOKEN_NOT_EXISTS";
        public const string ConfirmationExpired = "ACCOUNT_CONFIRMATION_EXPIRED";

        public static class External
        {
            public const string AlreadyExists = "EXTERNAL_ACCOUNT_ALREADY_EXISTS";
            public const string IsNotExists = "EXTERNAL_ACCOUNT_NOT_EXISTS";
        }
    }

    public static class Session
    {
        public const string IsRevoked = "SESSION_REVOKED";
    }
}