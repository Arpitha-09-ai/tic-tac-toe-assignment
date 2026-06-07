using TicTacToe.Api.Enums;
using TicTacToe.Api.Services;

namespace TicTacToe.Tests;

public class GameServiceTests
{

    [Fact]
    public void CreateGame_Should_Create_New_Game()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        Assert.NotNull(game);
        Assert.Equal("X", game.CurrentPlayer);
        Assert.Equal(GameStatus.InProgress, game.Status);
    }
    [Fact]
    public void MakeMove_Should_Place_X_On_Board()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);

        Assert.Equal("X", game.Board[0][0]);
    }
    [Fact]
    public void Move_On_Occupied_Cell_Should_Fail()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);

        var result =
            service.MakeMove(game.Id, "O", 0, 0);

        Assert.Null(result);
    }
    [Fact]
    public void Turn_Should_Switch_After_Valid_Move()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);

        Assert.Equal("O", game.CurrentPlayer);
    }
    [Fact]
    public void Row_Win_Should_Set_Winner()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 1, 0);

        service.MakeMove(game.Id, "X", 0, 1);
        service.MakeMove(game.Id, "O", 1, 1);

        service.MakeMove(game.Id, "X", 0, 2);

        Assert.Equal(GameStatus.Won, game.Status);
        Assert.Equal("X", game.Winner);
    }
    [Fact]
    public void Column_Win_Should_Set_Winner()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 0, 1);

        service.MakeMove(game.Id, "X", 1, 0);
        service.MakeMove(game.Id, "O", 1, 1);

        service.MakeMove(game.Id, "X", 2, 0);

        Assert.Equal(GameStatus.Won, game.Status);
    }
    [Fact]
    public void Diagonal_Win_Should_Set_Winner()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 0, 1);

        service.MakeMove(game.Id, "X", 1, 1);
        service.MakeMove(game.Id, "O", 0, 2);

        service.MakeMove(game.Id, "X", 2, 2);

        Assert.Equal(GameStatus.Won, game.Status);
    }
    [Fact]
    public void Draw_Should_Set_Status()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 0, 1);

        service.MakeMove(game.Id, "X", 0, 2);
        service.MakeMove(game.Id, "O", 1, 1);

        service.MakeMove(game.Id, "X", 1, 0);
        service.MakeMove(game.Id, "O", 1, 2);

        service.MakeMove(game.Id, "X", 2, 1);
        service.MakeMove(game.Id, "O", 2, 0);

        service.MakeMove(game.Id, "X", 2, 2);

        Assert.Equal(GameStatus.Draw, game.Status);
    }
    [Fact]
    public void Reset_Should_Clear_Board()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);

        service.ResetGame(game.Id);

        Assert.Null(game.Board[0][0]);
        Assert.Equal("X", game.CurrentPlayer);
        Assert.Equal(GameStatus.InProgress, game.Status);
        Assert.Null(game.Winner);
        Assert.Empty(game.MoveHistory);
    }
    [Fact]
    public void Undo_Should_Remove_Last_Move()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);

        service.UndoMove(game.Id);

        Assert.Null(game.Board[0][0]);
    }
    [Fact]
    public void Undo_Computer_Should_Remove_Two_Moves()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.Computer);

        service.MakeMove(game.Id, "X", 0, 0);

        service.UndoMove(game.Id);

        Assert.Equal(0, game.MoveHistory.Count);

        Assert.Null(game.Board[0][0]);
        Assert.Null(game.Board[1][1]);
    }
    [Fact]
    public void Scoreboard_Should_Update_After_Win()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 1, 0);

        service.MakeMove(game.Id, "X", 0, 1);
        service.MakeMove(game.Id, "O", 1, 1);

        service.MakeMove(game.Id, "X", 0, 2);

        var scoreboard = service.GetScoreboard();

        Assert.Equal(1, scoreboard.XWins);
    }
    [Fact]
    public void Move_After_Game_Completion_Should_Fail()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 1, 0);

        service.MakeMove(game.Id, "X", 0, 1);
        service.MakeMove(game.Id, "O", 1, 1);

        service.MakeMove(game.Id, "X", 0, 2);

        var result =
            service.MakeMove(game.Id, "O", 2, 2);

        Assert.Null(result);
    }

    [Fact]
    public void Computer_Should_Take_Center_On_First_Move()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.Computer);

        service.MakeMove(game.Id, "X", 0, 0);

        Assert.Equal("O", game.Board[1][1]);
    }
    [Fact]
    public void Computer_Should_Block_Winning_Move()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.Computer);

        game.Board[0][0] = "X";
        game.Board[0][1] = "X";

        var method = typeof(GameService)
            .GetMethod("MakeComputerMove",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        method!.Invoke(service, new object[] { game });

        Assert.Equal("O", game.Board[0][2]);
    }
    [Fact]
    public void ResetScoreboard_Should_Clear_All_Scores()
    {
        var service = new GameService();

        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.Id, "X", 0, 0);
        service.MakeMove(game.Id, "O", 1, 0);

        service.MakeMove(game.Id, "X", 0, 1);
        service.MakeMove(game.Id, "O", 1, 1);

        service.MakeMove(game.Id, "X", 0, 2);

        var scoreboard = service.GetScoreboard();

        Assert.Equal(1, scoreboard.XWins);

        service.ResetScoreboard();

        scoreboard = service.GetScoreboard();

        Assert.Equal(0, scoreboard.XWins);
        Assert.Equal(0, scoreboard.OWins);
        Assert.Equal(0, scoreboard.Draws);
    }

}