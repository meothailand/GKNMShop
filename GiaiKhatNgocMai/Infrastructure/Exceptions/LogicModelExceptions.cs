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

    public class InvalidRoleTypeException : Exception
    {
        public InvalidRoleTypeException()
            : base("This role type does not exist.")
        {
        }

        public InvalidRoleTypeException(string msg)
            : base(msg)
        {

        }
    }
}