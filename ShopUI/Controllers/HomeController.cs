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
            ViewBag.Books = resp.books;
            return View();
        }

        public ActionResult Book(int id)
        {
            BookResponse result = client.GetBook(id.ToString());

            ViewBag.Name = result.book.name;
            ViewBag.Cost = result.book.cost;
            ViewBag.Description = result.book.description;
            ViewBag.Id = id;

            return View();
        }

        public ActionResult Auth()
        {
            return View();
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
            ViewBag.Id = GlobalVariables.currentUser.id;
            ViewBag.Email = GlobalVariables.currentUser.email;
            ViewBag.FirstName = GlobalVariables.currentUser.firstName;
            ViewBag.LastName = GlobalVariables.currentUser.lastName;
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
                    ViewBag.Email = GlobalVariables.currentUser.email = user.email;
                    ViewBag.FirstName = GlobalVariables.currentUser.firstName = user.firstName;
                    ViewBag.LastName = GlobalVariables.currentUser.lastName = user.lastName;
                }
                else
                {
                    GlobalVariables.responseMessage = result;
                }
            }
            else
            {
                GlobalVariables.responseMessage = "Fields cannot contain spaces or be empty";
            }

            return View();
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
            ViewBag.Name = result.book.name;
            ViewBag.Cost = result.book.cost;
            ViewBag.Description = result.book.description;
            ViewBag.Id = id;

            return View();
        }

        [HttpPost]
        public ActionResult EditBook(int id, Book book)
        {
            book.id = id;
            GlobalVariables.responseMessage = client.UpdateBook(book);
          
            return View();
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
                GlobalVariables.responseMessage = "Add one or more books for create order";
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