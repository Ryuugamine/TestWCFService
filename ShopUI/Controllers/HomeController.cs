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

        public ActionResult Auth()
        {
            return View();
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
                return result;
            }
        }
    }
}