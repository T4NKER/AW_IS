using Lib.AspNetCore.ServerSentEvents;
using RequestManager.Models;
public class SSEService
{
    private readonly IServerSentEventsService _serverSentEventsService;

    public SSEService(IServerSentEventsService serverSentEventsService)
    {
        _serverSentEventsService = serverSentEventsService;
    }

    public async Task SendEventAsync(ServerSentEvent eventData)
    {
        await _serverSentEventsService.SendEventAsync(eventData);
    }


}
