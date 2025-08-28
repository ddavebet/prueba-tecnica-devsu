export interface Cliente {
  personaId: number;
  nombre: string;
  genero: string;
  edad: number;
  identificacion: string;
  direccion: string;
  telefono: string;
  contrasena: string;
  estado: boolean;
}

export interface AddClienteDTO {
  nombre: string;
  genero: string;
  edad: number;
  identificacion: string;
  direccion: string;
  telefono: string;
  contrasena: string;
}

export interface UpdateClienteDTO {
  nombre: string;
  genero: string;
  edad: number;
  identificacion: string;
  direccion: string;
  telefono: string;
  contrasena: string;
  estado: boolean;
}

export interface EstadoCuentaDTO {
  numeroCuenta: string;
  saldo: number;
  totalDebito: number;
  totalCredito: number;
}
