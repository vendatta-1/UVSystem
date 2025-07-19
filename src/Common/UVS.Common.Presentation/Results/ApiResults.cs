using Microsoft.AspNetCore.Http;
using UVS.Common.Domain;

namespace UVS.Common.Presentation.Results;

public static  class ApiResults
{
        public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Microsoft.AspNetCore.Http.Results.Problem(
            title: GetTitle(result.Error),
            detail: GetDescription(result.Error),
            statusCode: GetStatusCode(result.Error.Type),
            type:GetType(result.Error.Type),
            extensions: GetErrors(result)
            
        );

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            if (result.Error is not ValidationError validationError)
            {
                return null;
            }

            return new Dictionary<string, object?>
            {
                {"errors", validationError.Errors}
            };
        }

        static string GetType(ErrorType type) =>
            type switch
            {
                ErrorType.Validation => "Validation Error",
                ErrorType.Conflict   => "Conflict Error",
                ErrorType.NotFound => "Not Found",
                ErrorType.Failure   => "Failure",
                ErrorType.Problem  => "Problem",
                _=>"An unexpected error occurred"
            };

        static int GetStatusCode(ErrorType type) =>
            type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Problem => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
        
        static string GetTitle(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => error.Code,
                ErrorType.Conflict => error.Code,
                ErrorType.NotFound => error.Code,
                ErrorType.Failure => error.Code,
                _ => "Server Error"
            };

        static string GetDescription(Error error) =>
            error.Type switch
            {
                ErrorType.Conflict => error.Description,
                ErrorType.NotFound => error.Description,
                ErrorType.Failure => error.Description,
                ErrorType.Problem => error.Description,
                ErrorType.Validation => error.Description,
                _ => "Unhandled exception occured"
            };  
        
        
    }
}