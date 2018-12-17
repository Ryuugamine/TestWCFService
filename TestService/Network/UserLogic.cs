using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestService;
using TestService.Models;

namespace TestService.Network
{
    public partial class NetworkLogic : IRestService
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