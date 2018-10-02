using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Application.WebApi.SignalR.Interfaces
{
    public interface IMessageHub
    {
        Task Add(string value);

        Task Delete(string value);
    }
}
