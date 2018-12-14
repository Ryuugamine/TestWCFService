using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
            int.TryParse(id, out userId);
            using (TablesContext context = new TablesContext())
            {
                user = context.Users.Find(userId);
                if (user != null)
                {
                    if (!user.deleted)
                    {
                        user.deleted = true;
                        context.SaveChanges();
                        return "User successfully deleted.";
                    }
                    else
                    {
                        return "User has already been deleted.";
                    }
                }
                else
                {
                    return "User not found...";
                }
            }
        }

        public Book GetBook(string id)
        {
            Book book;
            int bookId;
            int.TryParse(id, out bookId);
            using (TablesContext context = new TablesContext())
            {
                book = context.Books.Find(bookId);
            }

            return book;
        }

        public User GetUser(string id)
        {
            User user;
            int userId;
            int.TryParse(id, out userId);
            using (TablesContext context = new TablesContext())
            {
                user = context.Users.Find(userId);
            }

            return user;
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

        public User NewUser(User user)
        {
            using (TablesContext context = new TablesContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return user;
        }

        public string RestoreUser(string email)
        {
            using (TablesContext context = new TablesContext())
            {
                var users = from u in context.Users where u.email.Equals(email) select u;
                if (users != null)
                {
                    if (users.First().deleted)
                    {
                        users.First().deleted = false;
                        context.SaveChanges();
                        return "User successfully restored";
                    }
                    else
                    {
                        return "The user with this email has not been deleted";
                    }
                }
                else
                {
                    return "User with such email not found...";
                }
                
            }

            
        }
    }
}
