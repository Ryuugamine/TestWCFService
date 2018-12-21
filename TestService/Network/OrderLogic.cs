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
                        if (books != null && books.Count() > 0)
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