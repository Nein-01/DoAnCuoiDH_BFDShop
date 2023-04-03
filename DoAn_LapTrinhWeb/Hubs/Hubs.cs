using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Hubs
{
    public class hitCounter : Hub
    {
        private static int clientCounter = 0;

        public override System.Threading.Tasks.Task OnConnected()
        {
            clientCounter++;
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            clientCounter--;
            return base.OnDisconnected(stopCalled);
        }
        public void SendCounter()
        {
            //string message;
            // Call the recalculateOnlineUsers method to update clients
            //message = string.Format(""+clientCounter);
            Clients.All.recalculateOnlineUsers(clientCounter);
        }
    }
    public class Notifi:Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}