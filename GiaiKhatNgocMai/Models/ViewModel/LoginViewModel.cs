using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
        public bool IsAccountCreated { get; set; }

    }
}