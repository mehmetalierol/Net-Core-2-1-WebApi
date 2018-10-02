using Company.Application.WebApi.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Application.WebApi.SignalR.Hubs
{
    public class MessageHub : Hub<IMessageHub>
    {
        public async Task Add(string value) => await Clients.All.Add(value);

        public async Task Delete(string value) => await Clients.All.Delete(value);
    }
}
