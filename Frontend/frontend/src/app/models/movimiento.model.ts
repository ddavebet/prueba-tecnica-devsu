import { Cuenta } from './cuenta.model';

export interface Movimiento {
  movimientoId: number;
  fecha: string;
  tipoMovimiento: string;
  valor: number;
  saldo: number;
  cuentaId: number;
  cuenta?: Cuenta;
}

export interface AddMovimientoDTO {
  tipoMovimiento: string;
  valor: number;
  cuentaId: number;
}

export interface UpdateMovimientoDTO {
  tipoMovimiento: string;
  valor: number;
  cuentaId: number;
}
