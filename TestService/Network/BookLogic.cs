using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestService.Models;

namespace TestService.Network
{
    public partial class NetworkLogic : IRestService
    {
        public BookResponse GetBook(string id)
        {
            BookResponse resp = new BookResponse();
            int bookId;
            if (int.TryParse(id, out bookId))
            {
                using (TablesContext context = new TablesContext())
                {
                    resp.Book = context.Books.Find(bookId);
                    if (resp.Book != null)
                    {
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                    }
                    else
                    {
                        resp.Status = (int)Constants.STATUSES.ERROR;
                        resp.Message = Constants.BOOK_NOT_FOUND;
                    }
                }
            }
            else
            {
                resp.Status = (int)Constants.STATUSES.ERROR;
                resp.Message = Constants.ENTER_INT;
            }

            return resp;
        }

        public Book NewBook(Book book)
        {
            using (TablesContext context = new TablesContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
            }

            return book;
        }
    }
}