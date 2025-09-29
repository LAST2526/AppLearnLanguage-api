using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Commons
{
    public static class MessageCodes
    {
        public static class Auth
        {
            public const string ERR_INVALID_GOOGLE_TOKEN = "ERR_AUTH_001";
            public const string ERR_INVALID_FACEBOOK_TOKEN = "ERR_AUTH_002";
            public const string SUC_AUTHENTICATION_SUCCESS = "SUC_AUTH_001";
            public const string ERR_USERNAME_NOT_FOUND = "ERR_AUTH_003";
            public const string ERR_PASSWORD_INCORRECT = "ERR_AUTH_004";
            public const string ERR_INVALID_REFRESH_TOKEN = "ERR_AUTH_005";
            public const string ERR_INVALID_TOKEN_OR_USER_NOT_FOUND = "ERR_AUTH_006";
            public const string SUC_LOGIN_SUCCESS = "SUC_AUTH_002";
            public const string SUC_LOGOUT_SUCCESS = "SUC_AUTH_003";
            public const string ERR_USER_ALREADY_EXISTS = "ERR_AUTH_007";
            public const string ERR_COURSE_NOT_EXISTS = "ERR_AUTH_008";
        }

        public static class User
        {
            public const string ERR_USER_NOT_FOUND = "ERR_USER_001";
            public const string SUC_USER_RETRIEVED = "SUC_USER_001";
            public const string ERR_USER_ALREADY_EXISTS = "ERR_USER_002";
            public const string SUC_USER_CREATED = "SUC_USER_002";
            public const string SUC_USER_UPDATED = "SUC_USER_003";
            public const string ERR_OLD_PASSWORD_INCORRECT = "ERR_USER_003";
            public const string SUC_PASSWORD_UPDATED = "SUC_USER_004";
            public const string ERR_RESET_PASSWORD_TOO_OFTEN = "ERR_USER_004";
            public const string MSG_TEMP_PASSWORD = "MSG_USER_001";
            public const string ERR_FAILED_TO_SEND_EMAIL = "ERR_USER_005";
            public const string SUC_TEMP_PASSWORD_SENT = "SUC_USER_005";
            public const string ERR_COURSE_NOT_EXISTS = "ERR_USER_006";
            public const string ERR_MAIL_DOB_NOT_MATCH = "ERR_USER_007";
        }

        public static class Topic
        {
            public const string SUC_TOPIC_RETRIEVED = "SUC_TOPIC_001";
            public const string ERR_TOPIC_NOT_FOUND = "ERR_TOPIC_001";
        }

        public static class Grammar
        {
            public const string SUC_GRAMMAR_RETRIEVED = "SUC_TOPIC_001";
            public const string ERR_GRAMMAR_NOT_FOUND = "ERR_TOPIC_001";
        }

        public static class Conversation
        {
            public const string SUC_CONVERSATION_RETRIEVED = "SUC_TOPIC_001";
            public const string ERR_CONVERSATION_NOT_FOUND = "ERR_TOPIC_001";
        }

        public static class QrCode
        {
            public const string SUC_QRCODE_RETRIEVED = "SUC_QRCODE_001";
            public const string ERR_QRCODE_NOT_FOUND = "ERR_QRCODE_001";
        }

        public static class Flashcard
        {
            public const string SUC_FLASHCARD_RETRIEVED = "SUC_FLASHCARD_001";
            public const string ERR_FLASHCARD_NOT_FOUND = "ERR_FLASHCARD_001";
            public const string ERR_FLASHCARD_MEMBER_NOT_FOUND = "ERR_FLASHCARD_001";
        }

        public static class BookInstance
        {
            public const string ERR_BOOKINSTANCE_MEMBER_NOT_FOUND = "ERR_BOOKINSTANCE_001";
            public const string ERR_BOOKINSTANCE_INVALID_INSTANCE_CODE = "ERR_BOOKINSTANCE_002";
            public const string ERR_BOOKINSTANCE_ALREADY_USED = "ERR_BOOKINSTANCE_003";
            public const string SUC_BOOKINSTANCE_REDEEMED_OK = "SUC_BOOKINSTANCE_001";
        }

        public static class Common
        {
            public const string ERR_INTERNAL_SERVER = "ERR_COMMON_001";
            public const string EMAIL_TEMP_PASSWORD_SUBJECT = "EMAIL_TEMP_PASSWORD_SUBJECT";
            public const string EMAIL_TEMP_PASSWORD_BODY = "EMAIL_TEMP_PASSWORD_BODY";
            public const string EMAIL_COURSE_REG_CONFIRM_SUBJECT = "EMAIL_COURSE_REG_CONFIRM_SUBJECT";
            public const string EMAIL_COURSE_REG_CONFIRM_BODY = "EMAIL_COURSE_REG_CONFIRM_BODY";
        }
    }
}
