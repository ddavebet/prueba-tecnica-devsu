import { Cliente } from './cliente.model';

export interface Cuenta {
  cuentaId: number;
  numeroCuenta: string;
  tipoCuenta: string;
  saldoInicial: number;
  clienteId: number;
  estado: boolean;
  cliente?: Cliente;
}

export interface AddCuentaDTO {
  numeroCuenta: string;
  tipoCuenta: string;
  saldoInicial: number;
  clienteId: number;
}

export interface UpdateCuentaDTO {
  numeroCuenta: string;
  tipoCuenta: string;
  saldoInicial: number;
  clienteId: number;
  estado: boolean;
}
