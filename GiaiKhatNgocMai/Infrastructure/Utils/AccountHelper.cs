using GiaiKhatNgocMai.Infrastructure.Exceptions;
using GiaiKhatNgocMai.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GiaiKhatNgocMai.Infrastructure.Utils
{
    public class AccountHelper
    {        
        public static void ResetPassword(string newPass, string confirmPass, string currentPass, string toUpdatePass,
                                         bool isReset = false, bool encrypt = false)
        {
            if (newPass != confirmPass) throw new InvalidDataException("Mật khẩu và xác nhận mật khẩu không trùng khớp");
            if (newPass.Length < 6) throw new InvalidDataException();
            if (isReset)
            {
                currentPass = (encrypt) ? SecurityHelper.Encrypt(currentPass) : currentPass;
                if (currentPass != toUpdatePass) throw new Exception();
            }
            toUpdatePass = (encrypt) ? SecurityHelper.Encrypt(newPass) : newPass;
        }

        public static bool ValidateEmail(string email)
        {
            var regx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regx.IsMatch(email);
        }

        public static bool ValidatePhoneNumber(string phone)
        {
            if(phone.Length > 20) return false;
            var regx = new Regex(@"[0-9]{8,20}");
            return regx.IsMatch(phone.Replace(" ",""));
        }
    }
}