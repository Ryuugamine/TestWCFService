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
    }
}