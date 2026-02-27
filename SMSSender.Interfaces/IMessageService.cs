using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface IMessageService
    {
        bool GetMessageFiltered(string Message);
    }
}
