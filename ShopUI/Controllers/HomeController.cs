using ShopUI.Models;
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

        public ActionResult Index()
        {
            ViewBag.Books = GetAllBooks().Books;
            return View();
        }

        public ActionResult Book(int id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/get_book/"+id);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var resp = reader.ReadToEnd();
                BookResponse result = new JavaScriptSerializer().Deserialize<BookResponse>(resp);
                ViewBag.Name = result.Book.Name;
                ViewBag.Cost = result.Book.Cost;
                ViewBag.Description = result.Book.Description;
                ViewBag.Id = id;
            }

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
            
            return View();
        }

        public ActionResult EditUser()
        {
            ViewBag.Msg = GlobalVariables.responseMessage;
            GlobalVariables.responseMessage = null;
            ViewBag.Id = GlobalVariables.currentUser.Id;
            ViewBag.Email = GlobalVariables.currentUser.Email;
            ViewBag.FirstName = GlobalVariables.currentUser.FirstName;
            ViewBag.LastName = GlobalVariables.currentUser.LastName;
            return View();
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            user.Id = GlobalVariables.currentUser.Id;
            string result = UpdateUser(user);
            if (result.Contains("success"))
            {
                ViewBag.Email = GlobalVariables.currentUser.Email = user.Email;
                ViewBag.FirstName = GlobalVariables.currentUser.FirstName = user.FirstName;
                ViewBag.LastName = GlobalVariables.currentUser.LastName = user.LastName;
            }
            else
            {
                GlobalVariables.responseMessage = result;
            }
            
            return View();
        }

        public ActionResult DeleteUser(int id)
        {
            string result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/delete_user/" + id);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
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

        //TODO понять как заново прокинуть сюда id при редиректе(удаление/изменение)
        public ActionResult EditBook(int id)
        {
            ViewBag.Msg = GlobalVariables.responseMessage;
            GlobalVariables.responseMessage = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/get_book/" + id);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var resp = reader.ReadToEnd();
                BookResponse result = new JavaScriptSerializer().Deserialize<BookResponse>(resp);
                ViewBag.Name = result.Book.Name;
                ViewBag.Cost = result.Book.Cost;
                ViewBag.Description = result.Book.Description;
                ViewBag.Id = id;
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditBook(int id, Book book)
        {
            book.Id = id;
            GlobalVariables.responseMessage = UpdateBook(book);
          
            return View();
        }

        public ActionResult DeleteBook(int id)
        {
            string result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/delete_book/" + id);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
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
            OrderRequest request = new OrderRequest
            {
                Books = GlobalVariables.booksInOrder,
                UserId = GlobalVariables.currentUser.Id,
                TotalPayment = GlobalVariables.totalPayment,
                Status = true
            };
            
            SendNewOrderRequest(request);

            GlobalVariables.booksInOrder.Clear();
            GlobalVariables.booksNames.Clear();
            GlobalVariables.totalPayment = 0;
            return RedirectToAction("Index");
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
            string authResult = SendAuthRequest(authData);
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

        public AllBooksResponse GetAllBooks()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/get_all_books");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var resp = reader.ReadToEnd();
                AllBooksResponse result = new JavaScriptSerializer().Deserialize<AllBooksResponse>(resp);
                return result;
            }
        }

        public string SendAuthRequest(Auth data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/auth");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                MemoryStream stream = new MemoryStream();
                new DataContractJsonSerializer(typeof(Auth)).WriteObject(stream, data);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string json = sr.ReadToEnd();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                UserResponse userResp = new JavaScriptSerializer().Deserialize<UserResponse>(result);
                if (userResp.Status == (int)GlobalVariables.STATUSES.OK)
                {
                    GlobalVariables.currentUser = userResp.User;
                }
                return userResp.Message;
            }
        }


        public string UpdateUser(User data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/update_user");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                MemoryStream stream = new MemoryStream();
                new DataContractJsonSerializer(typeof(User)).WriteObject(stream, data);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string json = sr.ReadToEnd();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }


        public string UpdateBook(Book data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/update_book");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                MemoryStream stream = new MemoryStream();
                new DataContractJsonSerializer(typeof(Book)).WriteObject(stream, data);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string json = sr.ReadToEnd();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        public string SendNewOrderRequest(OrderRequest data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:55/RestService.svc/new_order");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                MemoryStream stream = new MemoryStream();
                new DataContractJsonSerializer(typeof(OrderRequest)).WriteObject(stream, data);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                string json = sr.ReadToEnd();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Order orderResp = new JavaScriptSerializer().Deserialize<Order>(result);
                //if (orderResp.Status == (int)GlobalVariables.STATUSES.OK)
                //{
                //    GlobalVariables.currentUser = userResp.User;
                //}
                return "success";
            }
        }

    }
}