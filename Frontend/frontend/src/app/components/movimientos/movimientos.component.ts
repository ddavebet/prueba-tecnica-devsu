import { Component, OnInit } from '@angular/core';
import { MovimientoService } from '../../services/movimiento.service';
import { CuentaService } from '../../services/cuenta.service';
import {
  Movimiento,
  AddMovimientoDTO,
  UpdateMovimientoDTO,
} from '../../models/movimiento.model';
import { Cuenta } from '../../models/cuenta.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-movimientos',
  imports: [CommonModule, FormsModule],
  templateUrl: './movimientos.component.html',
  styleUrls: ['./movimientos.component.css'],
})
export class MovimientosComponent implements OnInit {
  movimientos: Movimiento[] = [];
  movimientoSeleccionado: Movimiento = this.getMovimientoVacio();
  nuevoMovimiento: Movimiento = this.getMovimientoVacio();
  filtro: string = '';
  mostrarModalAdd: boolean = false;
  mostrarModalUpdate: boolean = false;
  mostrarModalDelete: boolean = false;
  cuentas: Cuenta[] = [];

  constructor(
    private movimientoService: MovimientoService,
    private cuentaService: CuentaService
  ) {}

  ngOnInit() {
    this.movimientoService.getAll().subscribe({
      next: (data) => (this.movimientos = data),
      error: (err) => console.error('Error al obtener movimientos', err),
    });
    this.cuentaService.getAll().subscribe({
      next: (data) => (this.cuentas = data),
      error: (err) => console.error('Error al obtener cuentas', err),
    });
  }

  get filteredMovimientos(): Movimiento[] {
    if (!this.filtro.trim()) return this.movimientos;
    const term = this.filtro.toLowerCase();
    return this.movimientos.filter(
      (m) =>
        m.cuenta?.numeroCuenta.toLowerCase().includes(term) ||
        m.cuenta?.cliente?.nombre.toLowerCase().includes(term) ||
        m.cuenta?.cliente?.identificacion.toLowerCase().includes(term)
    );
  }

  crearMovimiento() {
    const addData: AddMovimientoDTO = {
      tipoMovimiento: this.nuevoMovimiento.tipoMovimiento,
      valor: this.nuevoMovimiento.valor,
      cuentaId: this.nuevoMovimiento.cuentaId,
    };
    this.movimientoService.add(addData).subscribe({
      next: () => {
        this.mostrarModalAdd = false;
        this.nuevoMovimiento = this.getMovimientoVacio();
        this.movimientoService.getAll().subscribe({
          next: (data) => (this.movimientos = data),
        });
      },
      error: (err) => {
        alert(err.error);
        console.error(err);
      },
    });
  }

  abrirEditarMovimiento(movimiento: Movimiento) {
    this.movimientoSeleccionado = movimiento;
    this.mostrarModalUpdate = true;
  }

  actualizarMovimiento() {
    const id = this.movimientoSeleccionado.movimientoId;
    const updateData: UpdateMovimientoDTO = {
      tipoMovimiento: this.movimientoSeleccionado.tipoMovimiento,
      valor: this.movimientoSeleccionado.valor,
      cuentaId: this.movimientoSeleccionado.cuentaId,
    };
    this.movimientoService.update(id, updateData).subscribe({
      next: () => {
        this.mostrarModalUpdate = false;
        this.movimientoSeleccionado = this.getMovimientoVacio();
        this.movimientoService.getAll().subscribe({
          next: (data) => (this.movimientos = data),
        });
      },
      error: (err) => {
        alert(err.error);
        console.error(err);
      },
    });
  }

  abrirEliminarMovimiento(movimiento: Movimiento) {
    this.movimientoSeleccionado = movimiento;
    this.mostrarModalDelete = true;
  }

  eliminarMovimiento() {
    const id = this.movimientoSeleccionado.movimientoId;
    this.movimientoService.delete(id).subscribe({
      next: () => {
        this.mostrarModalDelete = false;
        this.movimientoSeleccionado = this.getMovimientoVacio();
        this.movimientoService.getAll().subscribe({
          next: (data) => (this.movimientos = data),
        });
      },
      error: (err) => {
        alert(err.error);
        console.error(err);
      },
    });
  }

  getMovimientoVacio(): Movimiento {
    return {
      movimientoId: 0,
      fecha: '',
      tipoMovimiento: '',
      valor: 0,
      saldo: 0,
      cuentaId: 0,
      cuenta: undefined,
    };
  }
}
