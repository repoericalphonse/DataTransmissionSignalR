using Microsoft.AspNetCore.SignalR;

namespace Host.Hubs
{
    public class CopyDataHub : Hub
    {
        public async Task CopyData(string message)
        {
            await Clients.All.SendAsync("DataCopied", message);
        }

        public async Task PasteData(string message)
        {
            await Clients.All.SendAsync("Data Copied", message);
        }
    }
}
