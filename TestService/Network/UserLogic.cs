using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Activation;
using System.Web;
using TestService;
using TestService.Models;

namespace TestService.Network
{
    public partial class NetworkLogic : IRestService
    {
        public string Auth(Auth data)
        {
            if (data.Password.Equals(Constants.DEF_PASS))
            {
                using (TablesContext context = new TablesContext())
                {
                    var users = from u in context.Users where u.Email.Equals(data.Email) select u;
                    if (users != null && users.Count() > 0)
                    {
                        return Constants.SUCCESS;
                    }
                    else
                    {
                        return Constants.WRONG_USER_DATA;
                    }
                }
            }
            else
            {
                return Constants.WRONG_USER_DATA;
            }
        }

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
                            var orders = from o in context.Orders where o.UserId.Equals(userId) select o;
                            if (orders != null && orders.Count() > 0)
                            {
                                return Constants.USER_CANT_DELETED;
                            }
                            else
                            {
                                user.Deleted = true;
                                context.SaveChanges();
                                return Constants.USER_DELETED;
                            }                            
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

        public UserResponse GetUser(string id)
        {
            UserResponse resp = new UserResponse();
            int userId;
            if (int.TryParse(id, out userId))
            {
                using (TablesContext context = new TablesContext())
                {
                    resp.User = context.Users.Find(userId);
                    if (resp.User != null && !resp.User.Deleted)
                    {
                        resp.Status = (int)Constants.STATUSES.OK;
                        resp.Message = Constants.SUCCESS;
                    }
                    else
                    {
                        resp.Status = (int)Constants.STATUSES.ERROR;
                        resp.User = null;
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

        public String UpdateUser(User updateData)
        {
            using (TablesContext context = new TablesContext())
            {
                User user = context.Users.Find(updateData.Id);
                if (user != null && !user.Deleted)
                {
                    user.FirstName = updateData.FirstName;
                    user.LastName = updateData.LastName;
                    user.Email = updateData.Email;
                    context.SaveChanges();
                    return Constants.USER_UPDATED;
                }
                else
                {
                    return Constants.USER_NOT_FOUND;
                }

            }
        }

        public string RestoreUser(string email)
        {
            using (TablesContext context = new TablesContext())
            {
                var users = from u in context.Users where u.Email.Equals(email) select u;
                if (users != null && users.Count()>0)
                {
                    if (users.First().Deleted)
                    {
                        users.First().Deleted = false;
                        context.SaveChanges();
                        return Constants.USER_RESTORED;
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