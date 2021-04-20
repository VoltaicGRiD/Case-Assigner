using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Case_Assigner
{
    public class CaseHub : Hub
    {
        public async Task AddCase()
        {
            Debug.WriteLine("Hub Function Successful");
            await Clients.All.logCase();
        }
    }
}