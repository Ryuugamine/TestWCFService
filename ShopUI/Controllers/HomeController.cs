using ShopUI.ServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ShopUI.Controllers
{
    public class HomeController : Controller
    {
        RestServiceClient client = new RestServiceClient();
        public ActionResult Index()
        {
            var resp = client.AllBooks();
            return View(resp.books);
        }

        public ActionResult Book(int id)
        {
            BookResponse result = client.GetBook(id.ToString());

            return View(result.book);
        }

        public ActionResult Auth()
        {
            return View();
        }

        public ActionResult RestoreUser()
        {
            ViewBag.Msg = GlobalVariables.responseMessage;
            GlobalVariables.responseMessage = null;
            return View();
        }

        [HttpPost]
        public ActionResult RestoreUser(string email)
        {
            if (CheckString(email))
            {
                string result = client.RestoreUser(email);
                if (result.Contains("success"))
                {
                    return RedirectToAction("Auth");
                }
                else
                {
                    GlobalVariables.responseMessage = result;
                }
            }
            else
            {
                GlobalVariables.responseMessage = GlobalVariables.EDIT_USER_ERROR;
            }
            return View();
        }

        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBook(Book book)
        {
            if (book.cost.ToString() != null && book.cost > 0 && book.name != null)
            {
                client.NewBook(book);
                return RedirectToAction("Index");
            }
            else
            {
                GlobalVariables.responseMessage = GlobalVariables.EDIT_BOOK_ERROR;
            }
            
            return View();
        }

        public ActionResult MenuFragment()
        {
            return PartialView();
        }

        public ActionResult Buy()
        {
            ViewBag.Books = GlobalVariables.booksNames;
            ViewBag.TotalPayment = GlobalVariables.totalPayment;
            ViewBag.Msg = GlobalVariables.responseMessage;
            GlobalVariables.responseMessage = null;

            return View();
        }

        public ActionResult Pay(int id)
        {
            var resp = client.PayOrder(id.ToString());

            if (!resp.Contains("success"))
            {
                GlobalVariables.responseMessage = resp;
            }

            return RedirectToAction("OrdersList");
        }

        public ActionResult OrdersList()
        {
            OrdersByUser result = client.GetOrdersByUserId(GlobalVariables.currentUser.id.ToString());
            ViewBag.Orders = result.orderData;
            
            return View();
        }

        public ActionResult EditUser()
        {
            ViewBag.Msg = GlobalVariables.responseMessage;
            GlobalVariables.responseMessage = null;
            return View(GlobalVariables.currentUser);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        private bool CheckString(string str)
        {
            if(str==null)
            {
                return false;
            }
            else
            {
                return !str.Contains(" ");
            }
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if(CheckString(user.email) && CheckString(user.firstName) && CheckString(user.lastName))
            {
                user.id = GlobalVariables.currentUser.id;
                string result = client.UpdateUser(user);
                if (result.Contains("success"))
                {
                    GlobalVariables.currentUser.email = user.email;
                    GlobalVariables.currentUser.firstName = user.firstName;
                    GlobalVariables.currentUser.lastName = user.lastName;
                }
                else
                {
                    GlobalVariables.responseMessage = result;
                }
            }
            else
            {
                GlobalVariables.responseMessage = GlobalVariables.EDIT_USER_ERROR;
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            if(CheckString(user.email) && CheckString(user.firstName) && CheckString(user.lastName))
            {
                string result = client.NewUser(user);
                if (result.Contains("created"))
                {
                    return RedirectToAction("Auth");
                }
                else
                {
                    GlobalVariables.responseMessage = result;
                }
            }
            else
            {
                GlobalVariables.responseMessage = GlobalVariables.EDIT_USER_ERROR;
            }

            return View(user);
        }

        public ActionResult DeleteUser(int id)
        {
            var result = client.DeleteUser(id.ToString());
            if (result.Contains("success"))
            {
                GlobalVariables.currentUser = null;

                return RedirectToAction("Auth");
            }
            else
            {
                GlobalVariables.responseMessage = result;
                return RedirectToAction("EditUser");
            }
        }

        public ActionResult EditBook(int id)
        {
            BookResponse result = client.GetBook(id.ToString());
            ViewBag.Msg = GlobalVariables.responseMessage;
            GlobalVariables.responseMessage = null;
            return View(result.book);
        }

        [HttpPost]
        public ActionResult EditBook(int id, Book book)
        {
            book.id = id;
            if (book.cost.ToString()!=null && book.cost>0 && book.name!=null)
            {
                GlobalVariables.responseMessage = client.UpdateBook(book);
            }
            else
            {
                GlobalVariables.responseMessage = GlobalVariables.EDIT_BOOK_ERROR;
            }
            
          
            return View(book);
        }

        public ActionResult DeleteBook(int id)
        {
            var result = client.DeleteBook(id.ToString());
            if (result.Contains("success"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                GlobalVariables.responseMessage = result;
                return RedirectToAction("EditBook");
            }
        }

        [HttpGet]
        public ActionResult CreateOrder()
        {
            if (GlobalVariables.booksInOrder.Count>0) { 
                OrderRequest request = new OrderRequest
                {
                    books = GlobalVariables.booksInOrder.ToArray<int>(),
                    userId = GlobalVariables.currentUser.id,
                    totalPayment = GlobalVariables.totalPayment,
                };

                client.NewOrder(request);

                GlobalVariables.booksInOrder.Clear();
                GlobalVariables.booksNames.Clear();
                GlobalVariables.totalPayment = 0;
                return RedirectToAction("Index");
            }
            else
            {
                GlobalVariables.responseMessage = GlobalVariables.CREATE_ORDER_ERROR;
                return RedirectToAction("Buy");
            }
        }

        [HttpGet]
        public ActionResult AddBook(int id, int cost, string name)
        {
            GlobalVariables.booksInOrder.Add(id);
            GlobalVariables.booksNames.Add(name);
            GlobalVariables.totalPayment += cost;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Auth(Auth authData)
        {
            UserResponse userResp = client.Auth(authData);
            if (userResp.status == (int)GlobalVariables.STATUSES.OK)
            {
                GlobalVariables.currentUser = userResp.user;
            }
            string authResult = userResp.message;
            if (authResult.Contains("Success"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = authResult;
                return View();
            }
        }
    }
}