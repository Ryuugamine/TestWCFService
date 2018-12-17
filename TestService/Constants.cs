using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestService
{
    public class Constants
    {
        public enum STATUSES { ERROR = 1, OK = 0};

        public static string ENTER_INT = "Please, enter int value.";
        public static string SUCCESS = "Success.";
        public static string SUCCESSFUL_RESTORE = "User successfully restored";

        public static string ORDER_NOT_FOUND = "Order not found...";
        public static string BOOK_NOT_FOUND = "Book not found...";

        public static string USER_NOT_DELETED = "The user with this email has not been deleted";
        public static string USER_DELETED = "User successfully deleted.";
        public static string USER_NOT_FOUND = "User not found...";
        public static string USER_ALREADY_EXISTS = "User with this email already exists.";
        public static string USER_CREATED = "User created.";
        public static string USER_ALREADY_DELETED = "User has already been deleted.";
    }
}