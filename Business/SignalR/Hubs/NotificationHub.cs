using DataAccess.Entities.Section;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.SignalR.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task BroadcastSectionData(List<Section> data) =>
       await Clients.All.SendAsync("broadcastsectiondata", data);
    }
}
