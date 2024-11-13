using System.Collections.Generic;
using System.Net;

namespace Common
{
    public interface IOperationResult
    {
        string Error { get; set; }

        string Message { get; set; }

        HttpStatusCode StatusCode { get; set; }

        bool Success { get; }

    }

    public interface IOperationResult<T> : IOperationResult
    {
        T Result { get; }
    }

    public interface IOperationResultList<T> : IOperationResult
    {
        IEnumerable<T> Result { get; }
    }

}
