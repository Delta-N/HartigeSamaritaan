using System;
using System.Collections.Generic;
using System.Net;

namespace RoosterPlanner.Service.DataModels
{
    public interface ITaskResult
    {
        HttpStatusCode StatusCode { get; set; }

        bool Succeeded { get; set; }

        string Message { get; set; }

        Exception Error { get; set; }
    }

    public class TaskResult : ITaskResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public Exception Error { get; set; }

        //Constructor
        public TaskResult()
        {
        }

        //Constructor - Overload
        public TaskResult(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class TaskResult<T> : ITaskResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public Exception Error { get; set; }

        //Constructor
        public TaskResult()
        {
        }

        //Constructor - Overload
        public TaskResult(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public static TaskResult<T> CreateForSuccessResult(T entity)
        {
            return new TaskResult<T> { StatusCode = HttpStatusCode.OK, Succeeded = true, Data = entity };
        }
    }

    public class TaskListResult<T> : ITaskResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public List<T> Data { get; set; }

        public Exception Error { get; set; }

        //Constructor
        public TaskListResult()
        {
        }

        //Constructor - Overload
        public TaskListResult(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public static TaskListResult<T> CreateDefault(string message = null)
        {
            return new TaskListResult<T> { Succeeded = false, StatusCode = HttpStatusCode.NoContent, Data = new List<T>(), Message = message };
        }
    }
}
