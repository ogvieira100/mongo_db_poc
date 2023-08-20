using MediatR;

namespace mongo_api.Models
{
    public abstract class Event :  INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
