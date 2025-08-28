import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Movimiento,
  AddMovimientoDTO,
  UpdateMovimientoDTO,
} from '../models/movimiento.model';

@Injectable({
  providedIn: 'root',
})
export class MovimientoService {
  private apiUrl = 'http://localhost:5001/api/movimientos';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Movimiento[]> {
    return this.http.get<Movimiento[]>(this.apiUrl);
  }

  getById(id: number): Observable<Movimiento> {
    return this.http.get<Movimiento>(`${this.apiUrl}/${id}`);
  }

  add(movimiento: AddMovimientoDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, movimiento);
  }

  update(id: number, movimiento: UpdateMovimientoDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, movimiento);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
