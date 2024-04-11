using System;

namespace RequestManager.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime DeadlineTime { get; set; }

        public RequestModel(int id, string description, DateTime requestTime, DateTime deadlineTime)
        {
            Id = id;
            Description = description;
            RequestTime = requestTime;
            DeadlineTime = deadlineTime;
        }

        public RequestModel()
        {

        } 
    }
}
