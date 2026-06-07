
import { CommonModule } from '@angular/common';
import { GameService } from '../../services/game';
import { Component, ChangeDetectorRef, OnInit } from '@angular/core';


@Component({
  selector: 'app-game',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './game.html',
  styleUrl: './game.css'
})
export class Game implements OnInit {

currentGame: any = {
  board: [
    [null, null, null],
    [null, null, null],
    [null, null, null]
  ],
  moveHistory: [],
  currentPlayer: 'X',
  mode: 0,
  status: 0
};

scoreboard: any = null;

  constructor(
    private gameService: GameService,
    private cdr: ChangeDetectorRef
) {}

ngOnInit() {
  this.loadScoreboard();
}

  createGame(mode: number) {

    this.gameService.createGame(mode)
      .subscribe({
   next: (response: any) => {
  this.currentGame = response;

  this.cdr.detectChanges();

  console.log(this.currentGame);
},
        error: (error) => {
          console.error(error);
        }
      });

  }

  cellClicked(row: number, column: number) {

if (this.currentGame.status !== 0) {
    return;
  }
  if (this.currentGame.board[row][column]) {
    console.log('Already occupied');
    return;
  }

  this.gameService.makeMove(
    this.currentGame.id,
    {
      player: this.currentGame.currentPlayer,
      row,
      column
    }
  ).subscribe({
   next: (response: any) => {
  this.currentGame = response;
if (
    this.currentGame.status === 1 ||
    this.currentGame.status === 2
  ) {
    this.loadScoreboard();
  }
  this.cdr.detectChanges();
},
    error: (error) => {
      console.log(error.error);
    }
  });
}

undo() {

  this.gameService.undo(this.currentGame.id)
    .subscribe({
      next: (response: any) => {

        this.currentGame = response;
  this.loadScoreboard();
        this.cdr.detectChanges();

      },
      error: (error) => {
        console.error(error);
      }
    });

}
resetGame() {

  this.gameService.resetGame(this.currentGame.id)
    .subscribe({
      next: (response: any) => {

        this.currentGame = response;

        this.cdr.detectChanges();

      },
      error: (error) => {
        console.error(error);
      }
    });

}
loadScoreboard() {

  this.gameService.getScoreboard()
    .subscribe({
      next: (response: any) => {

        this.scoreboard = response;

        this.cdr.detectChanges();

      },
      error: (error) => {
        console.error(error);
      }
    });

}
isWinningCell(row: number, column: number): boolean {

  if (!this.currentGame?.winningCells) {
    return false;
  }

  return this.currentGame.winningCells.some(
    (cell: any) =>
      cell.row === row &&
      cell.column === column
  );
}
resetScoreboard() {
  this.gameService.resetScoreboard()
    .subscribe((response: any) => {
      this.scoreboard = response;
      this.loadScoreboard();
    });
}
}