using System;
using System.Collections.Generic;
using System.Linq;
using RequestManager.Models;

public class RequestService
{
    private List<RequestModel> _supportRequests = new List<RequestModel>();

    public RequestService()
    {
        CreateSupportRequests();
    }

    private void CreateSupportRequests()
    {
    DateTime now = DateTime.Now;
    _supportRequests = Enumerable.Range(1, 12)
                                 .Select(i => new RequestModel(i, $"Sample Request {i}", now, now.AddDays(1)))
                                 .ToList();

    _supportRequests.Add(new RequestModel(13, "Request in the past", now.AddHours(-1), now.AddHours(-1)));
    _supportRequests.Add(new RequestModel(14, "Request within one hour", now.AddMinutes(30), now.AddMinutes(30)));
    }

    public void CreateRequest(RequestModel request)
    {
        ValidateRequest(request);

        request.Id = _supportRequests.Count > 0 ? _supportRequests.Max(r => r.Id) + 1 : 1;
        request.RequestTime = DateTime.Now;

        _supportRequests.Add(request);
    }

    public void DeleteRequest(int id)
    {
        var request = _supportRequests.FirstOrDefault(sr => sr.Id == id);
        if (request != null)
        {
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

    public IEnumerable<RequestModel> GetAllRequests()
    {
        return _supportRequests;
    }

    private void ValidateRequest(RequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ArgumentException("Description cannot be empty");
        }

        if (request.DeadlineTime <= DateTime.Now.AddHours(1))
        {
            throw new ArgumentException("Deadline time cannot be before current time or within one hour");
        }
    }
}
