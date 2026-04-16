using Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IHomeService
    {
        Task<IEnumerable<SearchResultVm>> SearchAsync(string? query);
    }
}
