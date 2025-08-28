import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cuenta, AddCuentaDTO, UpdateCuentaDTO } from '../models/cuenta.model';

@Injectable({
  providedIn: 'root',
})
export class CuentaService {
  private apiUrl = 'http://localhost:5001/api/cuentas';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Cuenta[]> {
    return this.http.get<Cuenta[]>(this.apiUrl);
  }

  getById(id: number): Observable<Cuenta> {
    return this.http.get<Cuenta>(`${this.apiUrl}/${id}`);
  }

  add(cuenta: AddCuentaDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, cuenta);
  }

  update(id: number, cuenta: UpdateCuentaDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, cuenta);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
