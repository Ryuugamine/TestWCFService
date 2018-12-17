using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TestService.Models;

namespace TestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestService.svc or RestService.svc.cs at the Solution Explorer and start debugging.
    public class RestService : IRestService
    {
        public string DeleteUser(string id)
        {
            User user;
            int userId;
            if (int.TryParse(id, out userId))
            {
                using (TablesContext context = new TablesContext())
                {
                    user = context.Users.Find(userId);
                    if (user != null)
                    {
                        if (!user.Deleted)
                        {
                            user.Deleted = true;
                            context.SaveChanges();
                            return Constants.USER_DELETED;
                        }
                        else
                        {
                            return Constants.USER_ALREADY_DELETED;
                        }
                    }
                    else
                    {
                        return Constants.USER_NOT_FOUND;
                    }
                }
            } 
            else
            {
                return Constants.ENTER_INT;
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
                    if (resp.Book!=null)
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

        public OrderResponse GetOrder(string id)
        {
            OrderResponse resp = new OrderResponse();
            int orderId;
            if (int.TryParse(id, out orderId))
            {
                Order order;
                List<int> booksInOrder = new List<int>();
                using (TablesContext context = new TablesContext())
                {
                    order = context.Orders.Find(orderId);
                    if (order != null)
                    {
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                        resp.OrderData = new OrderRequest
                        {
                            Id = order.Id,
                            UserId = order.UserId,
                            TotalPayment = order.TotalPayment,
                            Status = order.Status
                        };
                        

                        var books = from b in context.BooksInOrders where b.OrderId.Equals(orderId) select b;
                        if(books != null && books.Count()>0)
                        {
                            books.ToList<BooksInOrder>().ForEach(delegate (BooksInOrder book)
                            {
                                booksInOrder.Add(book.BookId);
                            });
                            resp.OrderData.Books = booksInOrder;
                        }
                    }
                    else
                    {
                        resp.Status = (int)Constants.STATUSES.ERROR;
                        resp.Message = Constants.ORDER_NOT_FOUND;
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

        public UserResponse GetUser(string id)
        {
            UserResponse resp = new UserResponse();
            int userId;
            if (int.TryParse(id, out userId))
            {
                using (TablesContext context = new TablesContext())
                {
                    resp.User = context.Users.Find(userId);
                    if (resp.User != null)
                    {
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                    }
                    else
                    {
                        resp.Status = (int)Constants.STATUSES.ERROR;
                        resp.Message = Constants.USER_NOT_FOUND;
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

        public Order NewOrder(OrderRequest orderRequest)
        {
            using (TablesContext context = new TablesContext())
            {
                Order order = new Order
                {
                    UserId = orderRequest.UserId,
                    TotalPayment = orderRequest.TotalPayment,
                    Status = orderRequest.Status,
                };

                context.Orders.Add(order);
                context.SaveChanges();
              
                orderRequest.Books.ForEach(delegate (int book)
                {
                    BooksInOrder bookInOrder = new BooksInOrder
                    {
                        OrderId = context.Orders.ToList<Order>().Last().Id,
                        BookId = book
                    };
                    context.BooksInOrders.Add(bookInOrder);
                    context.SaveChanges();
                });
                
                return order;
            }            
        }

        public String NewUser(User user)
        {
            using (TablesContext context = new TablesContext())
            {
                var users = from u in context.Users where u.Email.Equals(user.Email) select u;
                if (users == null || users.Count() == 0)
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    return Constants.USER_CREATED;
                }
                else
                {
                    return Constants.USER_ALREADY_EXISTS;
                }
                    
            }

            
        }

        public string RestoreUser(string email)
        {
            using (TablesContext context = new TablesContext())
            {
                var users = from u in context.Users where u.Email.Equals(email) select u;
                if (users != null)
                {
                    if (users.First().Deleted)
                    {
                        users.First().Deleted = false;
                        context.SaveChanges();
                        return Constants.SUCCESSFUL_RESTORE;
                    }
                    else
                    {
                        return Constants.USER_NOT_DELETED;
                    }
                }
                else
                {
                    return Constants.USER_NOT_FOUND;
                }
                
            }

            
        }
    }
}
