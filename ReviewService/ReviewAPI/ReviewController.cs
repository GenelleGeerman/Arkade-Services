using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using IAuthorizationService = BusinessLayer.Interfaces.IAuthorizationService;

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
            if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                return Unauthorized("Authorization header missing");
            }

            string? tokenString = token.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(tokenString)) return Unauthorized("Invalid token or token is empty.");
            review.Token = tokenString;
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
            if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                return Unauthorized("Authorization header missing");
            }

            string? tokenString = token.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(tokenString)) return Unauthorized("Invalid token or token is empty.");
            review.Token = tokenString;
            Review updatedReview = await service.Update(review);
            return Ok(updatedReview);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{reviewId}")]
    [Authorize]
    public async Task<IActionResult> Delete(int reviewId)
    {
        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                return Unauthorized("Authorization header missing");
            }

            string? tokenString = token.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(tokenString)) return Unauthorized("Invalid token or token is empty.");

            bool isDeleted = await service.Delete(tokenString, reviewId);

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
