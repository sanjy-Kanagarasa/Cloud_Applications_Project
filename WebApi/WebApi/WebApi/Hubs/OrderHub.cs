using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.SignalR;
using WebApi.Models.Repositories;

namespace WebApi.Hubs
{
    public class OrderHub : Hub
    {

        private readonly OrderContext sessionContext;
        private readonly IUsersRepository userRepo;


        public OrderHub(OrderContext context, IUsersRepository userRepo)
        {
            sessionContext = context;
            this.userRepo = userRepo;
        }

        public async override Task OnConnectedAsync()
        {
            var email = Context.Connection.GetHttpContext().Request.Query["email"];

            // Check if email already exists in session table
            var session = sessionContext.Sessions.SingleOrDefault(s => s.Email == email);
            if (session != null)
            {
                sessionContext.Sessions.Remove(session);
                await sessionContext.SaveChangesAsync();
            }

            await Clients.All.InvokeAsync("Debug", $"{Context.ConnectionId}: {email}");
            await sessionContext.Sessions.AddAsync(new Session { ConnectionID = Context.ConnectionId, Email = email });
            await sessionContext.SaveChangesAsync();
            await base.OnConnectedAsync();
            return;
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //await Clients.All.InvokeAsync("Debug", $"{Context.ConnectionId}");
            var connectionID = Context.ConnectionId;
            var session = sessionContext.Sessions.FirstOrDefault(s => s.ConnectionID == connectionID);
            if (session != null)
            {
                sessionContext.Sessions.Remove(session);
                await sessionContext.SaveChangesAsync();
            }
            await base.OnDisconnectedAsync(exception);
            return;
        }

    }
}
