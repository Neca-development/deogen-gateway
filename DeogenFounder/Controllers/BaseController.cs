using DeogenFounder.Common.DTO;
using DeogenFounder.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DeogenFounder.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected static ApiResponse<T> FormatResponse<T>(T data, int status = 200)
    {
        var result = new ApiResponse<T>
        {
            Data = data,
            Status = status
        };

        return result;
    }
    
    protected static async Task EnsureIsValid<TV, TD>(TD dto) where TV : AbstractValidator<TD>, new()
    {
        var validator = new TV();
        var validationRes = await validator.ValidateAsync(dto);
        if (!validationRes.IsValid)
        {
            ThrowValidationError(validationRes.Errors);
        }
    }

    private static void ThrowValidationError(IEnumerable<ValidationFailure> failures)
    {
        var errors = failures.Select(x => x.ErrorMessage).ToList();
        throw new BadRequestException(errors);
    }
}
