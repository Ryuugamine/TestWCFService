using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using TestService.Models;

namespace TestService.Network
{
    public partial class NetworkLogic : IRestService
    {
        public OrdersByUser GetOrdersByUserId(string id)
        {
            OrdersByUser resp = new OrdersByUser();
            int userId;
            if (int.TryParse(id, out userId))
            {
                resp.OrderData = new List<OrderData>();
                using (TablesContext context = new TablesContext())
                {
                    var orders = from o in context.Orders where o.UserId.Equals(userId) select o;
                    if (orders != null && orders.Count() > 0)
                    {
                        orders.ToList<Order>().ForEach(delegate (Order order)
                        {
                            List<Book> booksInOrder = new List<Book>();
                            OrderData current = new OrderData
                            {
                                Id = order.Id,
                                UserId = order.UserId,
                                TotalPayment = order.TotalPayment,
                                Status = order.Status
                            };

                            var books = from b in context.BooksInOrders where b.OrderId.Equals(current.Id) select b;
                            if (books != null && books.Count() > 0)
                            {
                                books.ToList<BooksInOrder>().ForEach(delegate (BooksInOrder book)
                                {
                                    booksInOrder.Add(context.Books.Find(book.BookId));
                                });
                            }
                            current.Books = booksInOrder;
                            resp.OrderData.Add(current);
                        });
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                    }
                    else
                    {
                        resp.Status = (int)Constants.STATUSES.ERROR;
                        resp.Message = Constants.NO_ORDERS;
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
                List<Book> booksInOrder = new List<Book>();
                using (TablesContext context = new TablesContext())
                {
                    order = context.Orders.Find(orderId);
                    if (order != null)
                    {
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                        resp.OrderData = new OrderData
                        {
                            Id = order.Id,
                            UserId = order.UserId,
                            TotalPayment = order.TotalPayment,
                            Status = order.Status
                        };


                        var books = from b in context.BooksInOrders where b.OrderId.Equals(orderId) select b;
                        if (books != null && books.Count() > 0)
                        {
                            books.ToList<BooksInOrder>().ForEach(delegate (BooksInOrder book)
                            {
                                booksInOrder.Add(context.Books.Find(book.BookId));
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

        public string PayOrder(string id)
        {
            int orderId;
            if (int.TryParse(id, out orderId))
            {
                Order order;
                using (TablesContext context = new TablesContext())
                {
                    order = context.Orders.Find(orderId);
                    if (order != null)
                    {
                        if (!order.Status)
                        {
                            order.Status = true;
                            context.SaveChanges();
                            return Constants.ORDER_PAYED;
                        }
                        else
                        {
                            return Constants.ORDER_ALREADY_PAYED;
                        }
                    }
                    else
                    {
                        return Constants.ORDER_NOT_FOUND;
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