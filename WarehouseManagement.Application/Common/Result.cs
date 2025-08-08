using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.Common
{
    public class Result
    {
        public bool IsSucces { get; }
        public string? Error { get; }

        protected Result(bool isSucces, string? error)
        {
            IsSucces = isSucces;
            Error = error;
        }

        public static Result Success() => new(true, string.Empty);
        public static Result Failure(string error) => new(false, error);
    }
    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, string? error, T? value) : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(true, null, value);
        public static new Result<T> Failure(string error) => new(false, error, default);
    }
}
