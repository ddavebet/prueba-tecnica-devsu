import { Component, OnInit } from '@angular/core';
import { CuentaService } from '../../services/cuenta.service';
import { ClienteService } from '../../services/cliente.service';
import {
  Cuenta,
  AddCuentaDTO,
  UpdateCuentaDTO,
} from '../../models/cuenta.model';
import { Cliente } from '../../models/cliente.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cuentas',
  imports: [CommonModule, FormsModule],
  templateUrl: './cuentas.component.html',
  styleUrls: ['./cuentas.component.css'],
})
export class CuentasComponent implements OnInit {
  cuentas: Cuenta[] = [];
  cuentaSeleccionada: Cuenta = this.getCuentaVacia();
  nuevaCuenta: Cuenta = this.getCuentaVacia();
  filtro: string = '';
  mostrarModalAdd: boolean = false;
  mostrarModalUpdate: boolean = false;
  mostrarModalDelete: boolean = false;
  clientes: Cliente[] = [];

  constructor(
    private cuentaService: CuentaService,
    private clienteService: ClienteService
  ) {}

  ngOnInit() {
    this.cuentaService.getAll().subscribe({
      next: (data) => (this.cuentas = data),
      error: (err) => console.error('Error al obtener cuentas', err),
    });
    this.clienteService.getAll().subscribe({
      next: (data) => (this.clientes = data),
      error: (err) => console.error('Error al obtener clientes', err),
    });
  }

  get filteredCuentas(): Cuenta[] {
    if (!this.filtro.trim()) return this.cuentas;
    const term = this.filtro.toLowerCase();
    return this.cuentas.filter((c) =>
      c.numeroCuenta.toLowerCase().includes(term)
    );
  }

  crearCuenta() {
    const addData: AddCuentaDTO = {
      numeroCuenta: this.nuevaCuenta.numeroCuenta,
      tipoCuenta: this.nuevaCuenta.tipoCuenta,
      saldoInicial: this.nuevaCuenta.saldoInicial,
      clienteId: this.nuevaCuenta.clienteId,
    };
    this.cuentaService.add(addData).subscribe({
      next: () => {
        this.mostrarModalAdd = false;
        this.nuevaCuenta = this.getCuentaVacia();
        this.cuentaService.getAll().subscribe({
          next: (data) => (this.cuentas = data),
        });
      },
      error: (err) => {
        alert('Error al crear cuenta');
        console.error(err);
      },
    });
  }

  abrirEditarCuenta(cuenta: Cuenta) {
    this.cuentaSeleccionada = cuenta;
    this.mostrarModalUpdate = true;
  }

  actualizarCuenta() {
    const id = this.cuentaSeleccionada.cuentaId;
    const updateData: UpdateCuentaDTO = {
      numeroCuenta: this.cuentaSeleccionada.numeroCuenta,
      tipoCuenta: this.cuentaSeleccionada.tipoCuenta,
      saldoInicial: this.cuentaSeleccionada.saldoInicial,
      clienteId: this.cuentaSeleccionada.clienteId,
      estado: this.cuentaSeleccionada.estado,
    };
    this.cuentaService.update(id, updateData).subscribe({
      next: () => {
        this.mostrarModalUpdate = false;
        this.cuentaSeleccionada = this.getCuentaVacia();
        this.cuentaService.getAll().subscribe({
          next: (data) => (this.cuentas = data),
        });
      },
      error: (err) => {
        alert('Error al actualizar cuenta');
        console.error(err);
      },
    });
  }

  abrirEliminarCuenta(cuenta: Cuenta) {
    this.cuentaSeleccionada = cuenta;
    this.mostrarModalDelete = true;
  }

  eliminarCuenta() {
    const id = this.cuentaSeleccionada.cuentaId;
    this.cuentaService.delete(id).subscribe({
      next: () => {
        this.mostrarModalDelete = false;
        this.cuentaSeleccionada = this.getCuentaVacia();
        this.cuentaService.getAll().subscribe({
          next: (data) => (this.cuentas = data),
        });
      },
      error: (err) => {
        alert('Error al eliminar cuenta');
        console.error(err);
      },
    });
  }

  getCuentaVacia(): Cuenta {
    return {
      cuentaId: 0,
      numeroCuenta: '',
      tipoCuenta: '',
      saldoInicial: 0,
      estado: true,
      clienteId: 0,
      cliente: undefined,
    };
  }
}
