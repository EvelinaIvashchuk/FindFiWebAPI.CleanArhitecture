using AutoMapper;
using FindFi.CL.Application.Reviews.Commands;
using FindFi.CL.Application.Reviews.Queries;
using FindFi.CL.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WebAPI.Common;
using WebAPI.DTOs.Reviews;
namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class MetricsController : ApiControllerBase
{
    [HttpGet("reviews-count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> GetReviewsCount(CancellationToken ct)
    {
        return await Mediator.Send(new GetReviewsCountQuery(), ct);
    }
}