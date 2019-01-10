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

        public static string DEF_PASS = "pass";
        public static string WRONG_USER_DATA = "The username or password you entered is incorrect";

        public static string ORDER_NOT_FOUND = "Order not found...";
        public static string NO_ORDERS = "This user did not order anything.";
        public static string ORDER_PAYED = "Order successfully payed";
        public static string ORDER_ALREADY_PAYED = "Order has already been payed";
        
        public static string BOOK_NOT_FOUND = "Book not found...";
        public static string BOOK_ALREADY_DELETED = "Book has already been deleted.";
        public static string BOOK_NOT_DELETED = "Book with this ID has not been deleted.";
        public static string BOOK_DELETED = "Book successfully deleted.";
        public static string BOOK_CANT_DELETED = "Book can not been deleted, becouse already contains in one or more orders.";
        public static string BOOK_RESTORED = "Book successfully restored";
        public static string BOOK_UPDATED = "Book successfully updated";

        public static string USER_NOT_DELETED = "The user with this email has not been deleted.";
        public static string USER_DELETED = "User successfully deleted.";
        public static string USER_CANT_DELETED = "User can not been deleted, becouse already have one or more orders.";
        public static string USER_NOT_FOUND = "User not found...";
        public static string USER_ALREADY_EXISTS = "User with this email already exists.";
        public static string USER_CREATED = "User created.";
        public static string USER_ALREADY_DELETED = "User has already been deleted.";
        public static string USER_RESTORED = "User successfully restored";
        public static string USER_UPDATED = "User successfully updated";
    }
}