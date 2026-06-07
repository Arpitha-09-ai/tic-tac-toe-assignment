\# Tic Tac Toe



\## Project Overview



This project is a full-stack Tic Tac Toe application built using ASP.NET Core Web API and Angular.



The application supports both Two Player mode and Computer mode, including move history tracking, undo functionality, winner detection, scoreboard management, game reset, and automated unit testing.



\---



\## Tech Stack



\### Backend



\* ASP.NET Core Web API

\* C#

\* .NET 8

\* xUnit



\### Frontend



\* Angular

\* TypeScript

\* HTML

\* CSS



\---



\## Features Implemented



\### Game Modes



\* Two Player Mode

\* Computer Mode



\### Gameplay Features



\* Create New Game

\* Make Moves

\* Winner Detection

\* Draw Detection

\* Move History

\* Undo Functionality

\* Reset Game



\### Computer AI Strategy



The computer follows the below decision order:



1\. Win if possible

2\. Block opponent winning move

3\. Take center square

4\. Take available corner

5\. Take first available square



\### Scoreboard



\* Track X Wins

\* Track O Wins

\* Track Draws

\* Reset Scoreboard



\### User Interface



\* Current Player Display

\* Winner Display

\* Draw Display

\* Winner Cell Highlighting

\* Move History Panel

\* Scoreboard Panel

\* Board Disabled After Game Completion



\---



\## Application Screenshot



Add screenshots inside:



```text

screenshots/

```



Example:



```text

screenshots/game-ui.png

```



Then reference it:



```md

!\[Game UI](screenshots/game-ui.png)

```



\---



\## API Endpoint Summary



| Method | Endpoint          | Description       |

| ------ | ----------------- | ----------------- |

| POST   | /games            | Create a new game |

| GET    | /games/{id}       | Get game details  |

| POST   | /games/{id}/moves | Make a move       |

| POST   | /games/{id}/undo  | Undo move         |

| POST   | /games/{id}/reset | Reset game        |

| GET    | /scoreboard       | Get scoreboard    |

| POST   | /scoreboard/reset | Reset scoreboard  |



\---



\## Running the Backend



Navigate to the backend project:



```bash

cd backend/TicTacToe.Api

```



Restore packages:



```bash

dotnet restore

```



Run the application:



```bash

dotnet run

```



The API will start on the configured ASP.NET Core URL.



\---



\## Running the Frontend



Navigate to the Angular project:



```bash

cd Frontend/tic-tac-toe

```



Install dependencies:



```bash

npm install

```



Run Angular:



```bash

ng serve

```



Open the application:



```text

http://localhost:4200

```



\---



\## Running Unit Tests



Navigate to the test project:



```bash

cd backend/TicTacToe.Tests

```



Run tests:



```bash

dotnet test

```



\---



\## Unit Test Coverage



The project includes automated tests for:



\* Create Game

\* Valid Move

\* Invalid Move

\* Turn Switching

\* Row Win

\* Column Win

\* Diagonal Win

\* Draw Detection

\* Reset Game

\* Undo Two Player

\* Undo Computer

\* Scoreboard Update

\* Reset Scoreboard

\* Computer Move Selection

\* Computer Blocking Logic

\* Move After Game Completion



\---



\## AI Assistance Summary



AI Tool Used:



\* ChatGPT



Areas Assisted:



\* Project planning

\* API design suggestions

\* Angular UI suggestions

\* Unit testing examples

\* Debugging guidance

\* Documentation assistance



Manual Implementation:



\* Backend game logic

\* Computer AI implementation

\* Scoreboard management

\* Undo functionality

\* Angular frontend integration

\* Winner highlighting

\* Testing and validation



All generated code was reviewed, modified, tested, and validated manually before submission.



\---



\## Design Decisions



\### Undo Behaviour



Two Player Mode:



\* Undo removes the most recent move.



Computer Mode:



\* Undo removes both the player's move and the computer's move.



\### Scoreboard Behaviour



If a completed game is undone, the scoreboard is adjusted to maintain consistency with the current game state.



\### Data Storage



Game state and scoreboard data are stored in memory and reset when the API restarts.



\### Computer AI



A rule-based approach was chosen instead of Minimax to keep the implementation simple while still satisfying the assignment requirements.



\---



\## Assumptions



\* Board size is fixed at 3x3.

\* Player X always starts first.

\* Computer always plays as O.

\* Only one move can be played per turn.

\* Scoreboard data is stored in memory.

\* No authentication is required.



\---



\## Known Limitations



\* Data is not persisted to a database.

\* Game state is lost when the API restarts.

\* Multiplayer networking is not supported.

\* Only a single difficulty level is implemented for the computer player.



\---



\## Future Improvements



\* Database persistence

\* Multiple AI difficulty levels

\* Minimax-based AI strategy

\* Responsive mobile UI

\* User accounts and statistics

\* Multiplayer over network

\* Game history persistence



\---



\## How to Verify the Application



\### Two Player Mode



\* Create a new two-player game

\* Play until a player wins

\* Verify winner detection

\* Verify move history

\* Verify scoreboard update

\* Verify undo functionality



\### Computer Mode



\* Create a new computer game

\* Verify computer makes automatic moves

\* Verify blocking behaviour

\* Verify winning behaviour

\* Verify undo removes both moves



\### Scoreboard



\* Verify wins are counted

\* Verify draws are counted

\* Verify reset scoreboard functionality



\### Tests



Run:



```bash

dotnet test

```



Verify all tests pass successfully.



