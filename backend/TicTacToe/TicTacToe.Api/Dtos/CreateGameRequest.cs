using TicTacToe.Api.Enums;

namespace TicTacToe.Api.Dtos;

public class CreateGameRequest
{
    public GameMode Mode { get; set; }
}