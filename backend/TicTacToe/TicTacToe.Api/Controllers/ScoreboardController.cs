using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/scoreboard")]
public class ScoreboardController : ControllerBase
{
    private readonly GameService _gameService;

    public ScoreboardController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public IActionResult GetScoreboard()
    {
        return Ok(_gameService.GetScoreboard());
    }

    [HttpPost("reset")]
    public IActionResult ResetScoreboard()
    {
        return Ok(_gameService.ResetScoreboard());
    }
}