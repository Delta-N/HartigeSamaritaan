using System;

namespace RoosterPlanner.Service.Helpers
{
    [Serializable]
    public class RecordNotFoundException :Exception
    {
        public string RecordId { get; }

        public RecordNotFoundException()
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }

        public RecordNotFoundException(string message, string recordId):this(message)
        {
            RecordId = recordId;
        }
    }
}