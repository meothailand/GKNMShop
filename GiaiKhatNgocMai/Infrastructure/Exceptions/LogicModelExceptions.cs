using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Infrastructure.Exceptions
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException():base("Dữ liệu nhập không đúng")
        {
        }

        public InvalidDataException(string msg) : base(msg)
        {

        }
    }
}