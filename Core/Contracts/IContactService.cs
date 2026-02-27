using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IContactService
    {
        Task SaveMessageAsync(string name, string email, string message);
    }
}
