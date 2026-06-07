import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class GameService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  createGame(mode: number) {
    return this.http.post(
      `${this.apiUrl}/games`,
      { mode }
    );
  }

  getGame(id: string) {
    return this.http.get(
      `${this.apiUrl}/games/${id}`
    );
  }

  makeMove(id: string, request: any) {
    return this.http.post(
      `${this.apiUrl}/games/${id}/moves`,
      request
    );
  }

  undo(id: string) {
    return this.http.post(
      `${this.apiUrl}/games/${id}/undo`,
      {}
    );
  }

  resetGame(id: string) {
    return this.http.post(
      `${this.apiUrl}/games/${id}/reset`,
      {}
    );
  }

  getScoreboard() {
    return this.http.get(
      `${this.apiUrl}/scoreboard`
    );
  }

  resetScoreboard() {
    return this.http.post(
      `${this.apiUrl}/scoreboard/reset`,
      {}
    );
  }
}