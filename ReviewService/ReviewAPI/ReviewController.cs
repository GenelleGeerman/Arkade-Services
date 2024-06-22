using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ReviewAPI;

[Route("[controller]")]
[ApiController]
[EnableCors("Gateway")]
public class ReviewController(IReviewService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Review review)
    {
        try
        {
            Review createdReview = await service.Create(review);
            return Ok(createdReview);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            Review review = await service.Get(id);
            return Ok(review);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] Review review)
    {
        try
        {
            Review updatedReview = await service.Update(review);
            return Ok(updatedReview);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool isDeleted = await service.Delete(id);

            if (!isDeleted) return NotFound();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        try
        {
            var reviews = await service.GetByUserId(userId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("game/{gameId}")]
    public async Task<IActionResult> GetByGameId(int gameId)
    {
        try
        {
            var reviews = await service.GetByGameId(gameId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
