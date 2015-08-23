using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaiKhatNgocMai.Infrastructure.Interfaces
{
    public interface ISerializer
    {
        string SerializeToString<T>(T value);

        T Deserialize<T>(string value);
    }
}
