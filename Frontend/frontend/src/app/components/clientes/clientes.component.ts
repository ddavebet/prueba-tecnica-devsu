import { Component, OnInit } from '@angular/core';
import { ClienteService } from '../../services/cliente.service';
import {
  Cliente,
  AddClienteDTO,
  UpdateClienteDTO,
} from '../../models/cliente.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-clientes',
  imports: [CommonModule, FormsModule],
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css'],
})
export class ClientesComponent implements OnInit {
  clientes: Cliente[] = [];
  clienteSeleccionado: Cliente = this.getClienteVacio();
  nuevoCliente: Cliente = this.getClienteVacio();
  filtro: string = '';
  mostrarModalAdd: boolean = false;
  mostrarModalUpdate: boolean = false;
  mostrarModalDelete: boolean = false;

  constructor(private clienteService: ClienteService) {}

  ngOnInit() {
    this.clienteService.getAll().subscribe({
      next: (data) => (this.clientes = data),
      error: (err) => console.error('Error al obtener clientes', err),
    });
  }

  get filteredClientes(): Cliente[] {
    if (!this.filtro.trim()) return this.clientes;
    const term = this.filtro.toLowerCase();
    return this.clientes.filter(
      (c) =>
        c.nombre.toLowerCase().includes(term) ||
        c.identificacion.toLowerCase().includes(term)
    );
  }

  crearCliente() {
    const addData: AddClienteDTO = {
      nombre: this.nuevoCliente.nombre,
      genero: this.nuevoCliente.genero,
      edad: this.nuevoCliente.edad,
      identificacion: this.nuevoCliente.identificacion,
      direccion: this.nuevoCliente.direccion,
      telefono: this.nuevoCliente.telefono,
      contrasena: this.nuevoCliente.contrasena,
    };
    this.clienteService.add(addData).subscribe({
      next: () => {
        this.mostrarModalAdd = false;
        this.nuevoCliente = this.getClienteVacio();
        this.clienteService.getAll().subscribe({
          next: (data) => (this.clientes = data),
        });
      },
      error: (err) => {
        alert('Error al crear cliente');
        console.error(err);
      },
    });
  }

  abrirEditarCliente(cliente: Cliente) {
    this.clienteSeleccionado = cliente;
    this.mostrarModalUpdate = true;
  }

  actualizarCliente() {
    const id = this.clienteSeleccionado.personaId;
    const updateData: UpdateClienteDTO = {
      nombre: this.clienteSeleccionado.nombre,
      genero: this.clienteSeleccionado.genero,
      edad: this.clienteSeleccionado.edad,
      identificacion: this.clienteSeleccionado.identificacion,
      direccion: this.clienteSeleccionado.direccion,
      telefono: this.clienteSeleccionado.telefono,
      contrasena: this.clienteSeleccionado.contrasena,
      estado: this.clienteSeleccionado.estado,
    };
    this.clienteService.update(id, updateData).subscribe({
      next: () => {
        this.mostrarModalUpdate = false;
        this.clienteSeleccionado = this.getClienteVacio();
        this.clienteService.getAll().subscribe({
          next: (data) => (this.clientes = data),
        });
      },
      error: (err) => {
        alert('Error al actualizar cliente');
        console.error(err);
      },
    });
  }

  abrirEliminarCliente(cliente: Cliente) {
    this.clienteSeleccionado = cliente;
    this.mostrarModalDelete = true;
  }

  eliminarCliente() {
    const id = this.clienteSeleccionado.personaId;
    this.clienteService.delete(id).subscribe({
      next: () => {
        this.mostrarModalDelete = false;
        this.clienteSeleccionado = this.getClienteVacio();
        this.clienteService.getAll().subscribe({
          next: (data) => (this.clientes = data),
        });
      },
      error: (err) => {
        alert('Error al eliminar cliente');
        console.error(err);
      },
    });
  }

  getClienteVacio(): Cliente {
    return {
      personaId: 0,
      nombre: '',
      genero: '',
      edad: 0,
      identificacion: '',
      direccion: '',
      telefono: '',
      contrasena: '',
      estado: true,
    };
  }
}
