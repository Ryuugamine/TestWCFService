using ShopUI.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopUI
{
    public static class GlobalVariables
    {
        public static List<int> booksInOrder = new List<int>();
        public static List<string> booksNames = new List<string>();
        public enum STATUSES { ERROR = 1, OK = 0 };
        public static User currentUser;
        public static int totalPayment = 0;
        public static string responseMessage;

        public const string EDIT_USER_ERROR = "Fields can not contain spaces or be empty";
        public const string EDIT_BOOK_ERROR = "Fields can not be empty and the price can not be less than or equal to zero ";
        public const string CREATE_ORDER_ERROR = "Add one or more books for create order";
    }
}