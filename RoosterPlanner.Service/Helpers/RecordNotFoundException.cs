using System;

namespace RoosterPlanner.Service.Helpers
{
    /// <summary>
    /// Use this exception whenever a record is not found.
    /// </summary>
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