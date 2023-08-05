using Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace API.Middlewares;

public class ValidationError
{
    [JsonPropertyName("field")]
    public string Field { get; }

    public string Message { get; }

    public ValidationError(string field, string message)
    {
        Field = field != string.Empty ? field : null;
        Message = message;
    }
}

public class ValidationResultModel : ErrorResponse<List<ValidationError>>
{
    public ValidationResultModel(ModelStateDictionary modelState)
    {
        Message = "Validation Failed";
        Error = modelState.Keys
            .SelectMany(key => modelState[key].Errors.Select(x =>
            {
                if (x.ErrorMessage.Contains("The JSON value could not be converted"))
                    return new ValidationError(key, $"The {key} contains invalid value");

                return new ValidationError(key, x.ErrorMessage);
            }
            ))
            .ToList();
    }
}

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }
}

