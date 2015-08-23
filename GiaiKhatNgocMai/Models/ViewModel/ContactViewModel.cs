using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class ContactViewModel : ViewModelBase
    {
        public ContactViewModel()
        {
            Navigations.Add("Liên hệ", "");
        }
    }
}