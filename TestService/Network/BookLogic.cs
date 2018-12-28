using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Activation;
using System.Web;
using TestService.Models;

namespace TestService.Network
{
    public partial class NetworkLogic : IRestService
    {
        public Book NewBook(Book book)
        {
            using (TablesContext context = new TablesContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
            }

            return book;
        }

        public AllBooksResponse AllBooks()
        {
            using (TablesContext context = new TablesContext())
            {
                AllBooksResponse resp = new AllBooksResponse();
                resp.Status = 0;
                resp.Message = "Success";
                resp.Books = context.Books.ToList<Book>();
                return resp;
            }
        }

        public BookResponse GetBook(string id)
        {
            BookResponse resp = new BookResponse();
            int bookId;
            if (int.TryParse(id, out bookId))
            {
                using (TablesContext context = new TablesContext())
                {
                    resp.Book = context.Books.Find(bookId);
                    if (resp.Book != null && !resp.Book.Deleted)
                    {
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                    }
                    else
                    {
                        resp.Book = null;
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

        public String UpdateBook(Book updateData)
        {
            using (TablesContext context = new TablesContext())
            {
                Book book = context.Books.Find(updateData.Id);
                if (book != null && !book.Deleted)
                {
                    book.Name = updateData.Name;
                    book.Cost = updateData.Cost;
                    book.Description = updateData.Description;
                    context.SaveChanges();
                    return Constants.BOOK_UPDATED;
                }
                else
                {
                    return Constants.BOOK_NOT_FOUND;
                }

            }
        }

        public string DeleteBook(string id)
        {
            int bookId;
            Book book;
            if (int.TryParse(id, out bookId))
            {
                using (TablesContext context = new TablesContext())
                {
                    book = context.Books.Find(bookId);
                    if (book != null)
                    {
                        if (!book.Deleted)
                        {
                            var books = from b in context.BooksInOrders where b.BookId.Equals(bookId) select b;
                            if (books != null && books.Count()>0)
                            {
                                return Constants.BOOK_CANT_DELETED;
                            }
                            else
                            {
                                book.Deleted = true;
                                context.SaveChanges();
                                return Constants.BOOK_DELETED;
                            }
                        }
                        else
                        {
                            return Constants.BOOK_ALREADY_DELETED;
                        }                        
                    }
                    else
                    {
                        return Constants.BOOK_NOT_FOUND;
                    }
                }
            }
            else
            {
                return Constants.ENTER_INT;
            }
            
        }

        public string RestoreBook(string id)
        {
            int bookId;
            Book book;
            if (int.TryParse(id, out bookId))
            {
                using (TablesContext context = new TablesContext())
                {
                    book = context.Books.Find(bookId);
                    if (book.Deleted)
                    {
                        book.Deleted = false;
                        context.SaveChanges();
                        return Constants.BOOK_RESTORED;
                    }
                    else
                    {
                        return Constants.BOOK_NOT_DELETED; ;
                    }                    
                }
            }
            else
            {
                return Constants.ENTER_INT;
            }
            
        }
        
    }
}