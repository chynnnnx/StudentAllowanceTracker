using StudentAllowanceTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Enums;
using System.Collections.Generic;

namespace StudentAllowanceTracker.Shared.Responses
{
    public class Result<T>
    {
        public bool Success => Status == ResultStatus.Success;
        public ResultStatus Status { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public static Result<T> Ok(T? data = default) =>
            new Result<T> { Status = ResultStatus.Success, Data = data };

        public static Result<T> Fail(ResultStatus status, IEnumerable<string> errors) =>
            new Result<T> { Status = status, Errors = errors };

        public static Result<T> Fail(ResultStatus status, string error) =>
            new Result<T> { Status = status, Errors = new List<string> { error } };
    }
}
