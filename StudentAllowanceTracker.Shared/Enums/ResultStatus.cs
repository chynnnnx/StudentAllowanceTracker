using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResultStatus
    {
        Success,
        Updated,
        NotFound,
        AlreadyExists,
        ValidationError,
        Failed,
        Unauthorized
    }
}
