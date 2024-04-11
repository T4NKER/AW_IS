using RequestManager.Models;

public class RequestService
{
    private List<RequestModel> _supportRequests;

    public RequestService()
    {
        _supportRequests = new List<RequestModel>();
        for (int i = 1; i <= 12; i++) {
                _supportRequests.Add(new RequestModel(i, $"Sample Request {i}", DateTime.Now,
                                                     DateTime.Now.AddDays(1)));
        }
    }

    public void CreateRequest(RequestModel request)
    {
        request.Id = _supportRequests.Count > 0 ? _supportRequests.Max(r => r.Id) + 1 : 1;
                    request.RequestTime = DateTime.Now;

                    _supportRequests.Add(request);
    }

    public void DeleteRequest(int id)
    {
        var request = _supportRequests.FirstOrDefault(sr => sr.Id == id);
                if (request != null) {
                    _supportRequests.Remove(request);
                }
    }

    public IEnumerable<RequestModel> GetRequests(int id)
    {
        if (id <= 0) 
        {
            id = 1; 
        }
        int pageSize = 10;
                var activeRequests = _supportRequests.OrderBy(sr => sr.DeadlineTime)
                                         .Skip((id - 1) * pageSize)
                                         .Take(pageSize);
        return activeRequests;
    }
}