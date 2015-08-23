using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class CustomerLoginModel
    {
        public int Id { get; private set; }
        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public bool IsLoggedIn { get; private set; }
        public bool IsLoginError { get; private set; }

        public CustomerLoginModel()
        {
            IsLoggedIn = false;
            IsLoginError = false;
        }
        public void SetLogin(int id, string customerName, string customerEmail)
        {
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            Id = id;
            IsLoggedIn = true;
            IsLoginError = false;
        }

        public void SetError()
        {
            CustomerName = null;
            CustomerEmail = null;
            Id = 0;
            IsLoggedIn = false;
            IsLoginError = true;
        }
    }
}