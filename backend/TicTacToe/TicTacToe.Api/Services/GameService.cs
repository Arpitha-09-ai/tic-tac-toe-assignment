using TicTacToe.Api.Enums;
using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services;

public class GameService
{
    private readonly Scoreboard _scoreboard = new();
    private readonly Dictionary<Guid, Game> _games =
        new();

    public Game CreateGame(GameMode mode)
    {
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Mode = mode
        };

        _games.Add(game.Id, game);

        return game;
    }
    public Game? GetGame(Guid gameId)
    {
        _games.TryGetValue(gameId, out var game);

        return game;
    }
    public Game? MakeMove(Guid gameId, string player, int row, int column)
    {
        var game = GetGame(gameId);

        if (game == null)
        {
            return null;
        }

        player = player.ToUpper();

        if (game.Status != GameStatus.InProgress)
        {
            return null;
        }
        if (row < 0 || row > 2 ||
    column < 0 || column > 2)
        {
            return null;
        }
        if (game.Board[row][column] != null)
        {
            return null;
        }

        if (game.CurrentPlayer != player)
        {
            return null;
        }

        game.Board[row][column] = player;

        game.MoveHistory.Add(
            new Move
            {
                MoveNumber = game.MoveHistory.Count + 1,
                Player = player,
                Row = row,
                Column = column
            });

        if (game.Mode == GameMode.Computer &&
    player == "X" &&
    game.Status == GameStatus.InProgress)
        {
            MakeComputerMove(game);
            if (game.Status != GameStatus.InProgress)
            {
                return game;
            }
        }

        if (CheckWinner(game, player))
        {
            game.Status = GameStatus.Won;
            game.Winner = player;

            if (player == "X")
            {
                _scoreboard.XWins++;
            }
            else
            {
                _scoreboard.OWins++;
            }

            return game;
        }

        if (CheckDraw(game))
        {
            game.Status = GameStatus.Draw;
            _scoreboard.Draws++;

            return game;
        }

        if (game.Mode == GameMode.TwoPlayer)
        {
            game.CurrentPlayer =
                game.CurrentPlayer == "X"
                    ? "O"
                    : "X";
        }

        return game;
    }
    private bool CheckWinner(Game game, string player)
    {
        var oldCells = game.WinningCells.ToList();
        game.WinningCells.Clear();
        // Rows
        for (int row = 0; row < 3; row++)
        {
            if (game.Board[row][0] == player &&
                game.Board[row][1] == player &&
                game.Board[row][2] == player)
            {
                game.WinningCells = new List<WinningCell>
            {
                new() { Row = row, Column = 0 },
                new() { Row = row, Column = 1 },
                new() { Row = row, Column = 2 }
            };

                return true;
            }
        }

        // Columns
        for (int col = 0; col < 3; col++)
        {
            if (game.Board[0][col] == player &&
                game.Board[1][col] == player &&
                game.Board[2][col] == player)
            {
                game.WinningCells = new List<WinningCell>
            {
                new() { Row = 0, Column = col },
                new() { Row = 1, Column = col },
                new() { Row = 2, Column = col }
            };

                return true;
            }
        }

        // Main Diagonal
        if (game.Board[0][0] == player &&
            game.Board[1][1] == player &&
            game.Board[2][2] == player)
        {
            game.WinningCells = new List<WinningCell>
        {
            new() { Row = 0, Column = 0 },
            new() { Row = 1, Column = 1 },
            new() { Row = 2, Column = 2 }
        };

            return true;
        }

        // Other Diagonal
        if (game.Board[0][2] == player &&
            game.Board[1][1] == player &&
            game.Board[2][0] == player)
        {
            game.WinningCells = new List<WinningCell>
        {
            new() { Row = 0, Column = 2 },
            new() { Row = 1, Column = 1 },
            new() { Row = 2, Column = 0 }
        };

            return true;
        }
        game.WinningCells = oldCells;
        return false;
    }

    private bool CheckDraw(Game game)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (game.Board[row][col] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }
    public Game? ResetGame(Guid gameId)
    {
        var game = GetGame(gameId);

        if (game == null)
        {
            return null;
        }

        game.Board =
        [
            new string?[3],
        new string?[3],
        new string?[3]
        ];

        game.CurrentPlayer = "X";

        game.Status = GameStatus.InProgress;

        game.Winner = null;

        game.MoveHistory.Clear();

        game.WinningCells.Clear();

        return game;
    }
    public Scoreboard GetScoreboard()
    {
        return _scoreboard;
    }

    public Scoreboard ResetScoreboard()
    {
        _scoreboard.XWins = 0;
        _scoreboard.OWins = 0;
        _scoreboard.Draws = 0;

        return _scoreboard;
    }
    public Game? UndoMove(Guid gameId)
    {
        var game = GetGame(gameId);

        if (game == null)
        {
            return null;
        }

        if (game.MoveHistory.Count == 0)
        {
            return game;
        }

        // Reverse scoreboard if game was already completed
        if (game.Status == GameStatus.Won)
        {
            if (game.Winner == "X")
            {
                _scoreboard.XWins--;
            }
            else if (game.Winner == "O")
            {
                _scoreboard.OWins--;
            }
        }
        else if (game.Status == GameStatus.Draw)
        {
            _scoreboard.Draws--;
        }

        if (game.Mode == GameMode.Computer)
        {
            int movesToRemove = Math.Min(2, game.MoveHistory.Count);

            for (int i = 0; i < movesToRemove; i++)
            {
                var lastMove = game.MoveHistory.Last();

                game.Board[lastMove.Row][lastMove.Column] = null;

                game.MoveHistory.Remove(lastMove);
            }

            game.CurrentPlayer = "X";
        }
        else
        {
            var lastMove = game.MoveHistory.Last();

            game.Board[lastMove.Row][lastMove.Column] = null;

            game.MoveHistory.Remove(lastMove);

            game.CurrentPlayer = lastMove.Player;
        }

        game.Status = GameStatus.InProgress;
        game.Winner = null;
        game.WinningCells.Clear();

        return game;
    }
    private void MakeComputerMove(Game game)
    {
        // 1. Win if possible
        var winningMove = FindWinningMove(game, "O");

        if (winningMove != null)
        {
            PlaceComputerMove(game, winningMove.Value.row, winningMove.Value.col);
            return;
        }

        // 2. Block opponent
        var blockingMove = FindWinningMove(game, "X");

        if (blockingMove != null)
        {
            PlaceComputerMove(game, blockingMove.Value.row, blockingMove.Value.col);
            return;
        }

        // 3. Take center
        if (game.Board[1][1] == null)
        {
            PlaceComputerMove(game, 1, 1);
            return;
        }

        // 4. Take corner
        var corners = new (int row, int col)[]
        {
        (0,0),
        (0,2),
        (2,0),
        (2,2)
        };

        foreach (var corner in corners)
        {
            if (game.Board[corner.row][corner.col] == null)
            {
                PlaceComputerMove(game, corner.row, corner.col);
                return;
            }
        }

        // 5. Any empty cell
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (game.Board[row][col] == null)
                {
                    PlaceComputerMove(game, row, col);
                    return;
                }
            }
        }
    }
    private (int row, int col)? FindWinningMove(Game game, string player)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (game.Board[row][col] == null)
                {
                    game.Board[row][col] = player;

                    bool isWinner = IsWinningBoard(game, player);

                    game.Board[row][col] = null;

                    if (isWinner)
                    {
                        return (row, col);
                    }
                }
            }
        }
       
        return null;
    }
    private void PlaceComputerMove(Game game, int row, int col)
    {
        game.Board[row][col] = "O";

        game.MoveHistory.Add(
            new Move
            {
                MoveNumber = game.MoveHistory.Count + 1,
                Player = "O",
                Row = row,
                Column = col
            });

        game.CurrentPlayer = "X";

        if (CheckWinner(game, "O"))
        {
            game.Status = GameStatus.Won;
            game.Winner = "O";
            _scoreboard.OWins++;
            return;
        }

        if (CheckDraw(game))
        {
            game.Status = GameStatus.Draw;
            _scoreboard.Draws++;
        }
    }
    private bool IsWinningBoard(Game game, string player)
    {
        // Rows
        for (int row = 0; row < 3; row++)
        {
            if (game.Board[row][0] == player &&
                game.Board[row][1] == player &&
                game.Board[row][2] == player)
            {
                return true;
            }
        }

        // Columns
        for (int col = 0; col < 3; col++)
        {
            if (game.Board[0][col] == player &&
                game.Board[1][col] == player &&
                game.Board[2][col] == player)
            {
                return true;
            }
        }

        // Main diagonal
        if (game.Board[0][0] == player &&
            game.Board[1][1] == player &&
            game.Board[2][2] == player)
        {
            return true;
        }

        // Other diagonal
        if (game.Board[0][2] == player &&
            game.Board[1][1] == player &&
            game.Board[2][0] == player)
        {
            return true;
        }

        return false;
    }

}