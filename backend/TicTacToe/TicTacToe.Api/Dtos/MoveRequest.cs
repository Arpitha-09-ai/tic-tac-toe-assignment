namespace TicTacToe.Api.Dtos;

public class MoveRequest
{
    public string Player { get; set; } = string.Empty;

    public int Row { get; set; }

    public int Column { get; set; }
}