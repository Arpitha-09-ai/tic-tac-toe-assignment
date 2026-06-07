using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Dtos;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly GameService _gameService;

    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public IActionResult CreateGame(CreateGameRequest request)
    {
        var game = _gameService.CreateGame(request.Mode);

        return Ok(game);
    }
    [HttpGet("{id}")]
    public IActionResult GetGame(Guid id)
    {
        var game = _gameService.GetGame(id);

        if (game == null)
            return NotFound();

        return Ok(game);
    }
    [HttpPost("{id}/moves")]
    public IActionResult MakeMove(Guid id, MoveRequest request)
    {
        var game = _gameService.MakeMove(
            id,
            request.Player,
            request.Row,
            request.Column);

        if (game == null)
        {
            return BadRequest("Invalid move");
        }

        return Ok(game);
    }
    [HttpPost("{id}/reset")]
    public IActionResult ResetGame(Guid id)
    {
        var game = _gameService.ResetGame(id);

        if (game == null)
        {
            return BadRequest("Game not found");
        }

        return Ok(game);
    }
    [HttpPost("{id}/undo")]
    public IActionResult UndoMove(Guid id)
    {
        var game = _gameService.UndoMove(id);

        if (game == null)
        {
            return BadRequest();
        }

        return Ok(game);
    }
}