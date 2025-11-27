using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IMediator Mediator => field ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
