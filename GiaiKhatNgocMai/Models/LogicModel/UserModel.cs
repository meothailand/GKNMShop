using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class UserModel
    {
        public UserModel()
        {
            News = new List<NewsModel>();
            Orders = new List<OrderModel>();
        }
        public UserModel(User user):this()
        {
            ModelObjectHelper.CopyObject(user, this);
            if (user.Orders != null)
            {
                foreach (var o in user.Orders)
                {
                    Orders.Add(new OrderModel(o));
                }
            }

            if (user.News != null)
            {
                foreach (var n in user.News)
                {
                    News.Add(new NewsModel(n));
                }
            }
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public bool IsActive { get; set; }
        public List<NewsModel> News { get; private set; }
        public List<OrderModel> Orders { get; private set; }
        public void SetEmail(string email)
        {
            email = email.Trim();
            if (!AccountHelper.ValidateEmail(email)) throw new Exception();
            Email = email;
        }
        public void ResetPassword(string newPass, string confirmPass, string currentPass, bool isReset)
        {
            AccountHelper.ResetPassword(newPass, confirmPass, currentPass, this.Password, isReset, true);
        }
        public User ToEntity()
        {
            var user = new User();
            ModelObjectHelper.CopyObject(this, user);
            return user;
        }
    }
}