import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Cliente,
  AddClienteDTO,
  UpdateClienteDTO,
  EstadoCuentaDTO,
} from '../models/cliente.model';

@Injectable({
  providedIn: 'root',
})
export class ClienteService {
  private apiUrl = 'http://localhost:5001/api/clientes';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(this.apiUrl);
  }

  getById(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.apiUrl}/${id}`);
  }

  getEstadoCuenta(
    id: number,
    fechaInicio: string,
    fechaFin: string
  ): Observable<EstadoCuentaDTO[]> {
    const fechas = `${fechaInicio}_${fechaFin}`;
    return this.http.get<EstadoCuentaDTO[]>(
      `${this.apiUrl}/${id}/reportes?fechas=${fechas}`
    );
  }

  getEstadoCuentaPdf(
    id: number,
    fechaInicio: string,
    fechaFin: string
  ): Observable<string> {
    const fechas = `${fechaInicio}_${fechaFin}`;
    return this.http.get<string>(
      `${this.apiUrl}/${id}/reportes/pdf?fechas=${fechas}`
    );
  }

  add(cliente: AddClienteDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, cliente);
  }

  update(id: number, cliente: UpdateClienteDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, cliente);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
