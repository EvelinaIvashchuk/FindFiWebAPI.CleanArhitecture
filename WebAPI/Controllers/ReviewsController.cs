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
public sealed class ReviewsController(IMapper mapper) : ApiControllerBase
{
    private static string MakeEtag(Review review) => $"W/\"{review.UpdatedAt.Ticks}\"";

    [HttpGet]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 20, [FromQuery] long? cursor = null, CancellationToken ct = default)
    {
        IReadOnlyList<Review> items;
        if (cursor is null)
        {
            items = await Mediator.Send(new GetAllReviewsQuery(skip, take), ct);
        }
        else
        {
            var fetch = Math.Clamp(take * 2, 1, 400);
            var batch = await Mediator.Send(new GetAllReviewsQuery(0, fetch), ct);
            items = batch.Where(r => r.UpdatedAt.Ticks > cursor.Value)
                .OrderByDescending(r => r.CreatedAt)
                .Take(take)
                .ToList();
        }

        var dtos = mapper.Map<IReadOnlyList<ReviewDto>>(items);
        if (items.Count > 0)
        {
            var next = items[^1].UpdatedAt.Ticks.ToString();
            Response.Headers["X-Next-Cursor"] = next;
        }
        return Ok(dtos);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken ct)
    {
        var review = await Mediator.Send(new GetReviewByIdQuery(id), ct);
        if (review is null) return NotFound();
        var dto = mapper.Map<ReviewDto>(review);
        Response.Headers[HeaderNames.ETag] = MakeEtag(review);
        return Ok(dto);
    }

    [HttpGet("by-listing/{listingId}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByListing([FromRoute] int listingId, [FromQuery] int skip = 0, [FromQuery] int take = 20, [FromQuery] long? cursor = null, CancellationToken ct = default)
    {
        IReadOnlyList<Review> items;
        if (cursor is null)
        {
            items = await Mediator.Send(new GetReviewsByListingQuery(listingId, skip, take), ct);
        }
        else
        {
            var fetch = Math.Clamp(take * 2, 1, 400);
            var batch = await Mediator.Send(new GetReviewsByListingQuery(listingId, 0, fetch), ct);
            items = batch.Where(r => r.UpdatedAt.Ticks > cursor.Value)
                         .OrderByDescending(r => r.CreatedAt)
                         .Take(take)
                         .ToList();
        }

        var dtos = mapper.Map<IReadOnlyList<ReviewDto>>(items);
        if (items.Count > 0)
        {
            var next = items[^1].UpdatedAt.Ticks.ToString();
            Response.Headers["X-Next-Cursor"] = next;
        }
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest request, CancellationToken ct)
    {
        var id = await Mediator.Send(mapper.Map<CreateReviewCommand>(request), ct);
        var created = await Mediator.Send(new GetReviewByIdQuery(id), ct);
        if (created is null) return Problem("Створено відгук, але не вдалося його завантажити", statusCode: StatusCodes.Status500InternalServerError);
        var dto = mapper.Map<ReviewDto>(created);
        Response.Headers[HeaderNames.ETag] = MakeEtag(created);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateReviewRequest request, CancellationToken ct)
    {
        var current = await Mediator.Send(new GetReviewByIdQuery(id), ct);
        if (current is null) return NotFound();

        if (!Request.Headers.TryGetValue(HeaderNames.IfMatch, out var ifMatch) || string.IsNullOrWhiteSpace(ifMatch))
            return BadRequest("Відсутній заголовок If-Match для оптимістичної конкурентності");

        var currentEtag = MakeEtag(current);
        if (!string.Equals(ifMatch.ToString(), currentEtag, StringComparison.Ordinal))
            return Conflict(new { message = "Конкурентний конфлікт: застарілий ETag" });

        var ok = await Mediator.Send(new UpdateReviewCommand(id, request.Title, request.Text, request.IsVisible, request.AddPhotos, request.RemovePhotos), ct);
        if (!ok) return Problem("Не вдалося оновити відгук", statusCode: StatusCodes.Status500InternalServerError);

        var updated = await Mediator.Send(new GetReviewByIdQuery(id), ct);
        if (updated is null) return NotFound();
        var dto = mapper.Map<ReviewDto>(updated);
        Response.Headers[HeaderNames.ETag] = MakeEtag(updated);
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteReviewCommand(id), ct);
        return NoContent();
    }
}
