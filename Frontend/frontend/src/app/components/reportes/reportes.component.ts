import { Component, OnInit } from '@angular/core';
import { ClienteService } from '../../services/cliente.service';
import { Cliente, EstadoCuentaDTO } from '../../models/cliente.model';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reportes',
  imports: [CommonModule, FormsModule],
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css'],
})
export class ReportesComponent implements OnInit {
  clientes: Cliente[] = [];
  detalles: EstadoCuentaDTO[] = [];
  clienteId?: number;
  fechaInicio?: Date;
  fechaFin?: Date;

  constructor(private clienteService: ClienteService) {}

  ngOnInit() {
    this.clienteService.getAll().subscribe({
      next: (data) => (this.clientes = data),
      error: (err) => console.error('Error al obtener clientes', err),
    });
  }

  consultar(clienteId: number, fechaInicio: Date, fechaFin: Date) {
    this.clienteService
      .getEstadoCuenta(
        clienteId,
        new DatePipe('en-US').transform(fechaInicio, 'yyyy-MM-dd')!,
        new DatePipe('en-US').transform(fechaFin, 'yyyy-MM-dd')!
      )
      .subscribe({
        next: (data) => (this.detalles = data),
        error: (err) => console.error('Error al obtener estado de cuenta', err),
      });
  }

  descargar(clienteId: number, fechaInicio: Date, fechaFin: Date) {
    this.clienteService
      .getEstadoCuentaPdf(
        clienteId,
        new DatePipe('en-US').transform(fechaInicio, 'yyyy-MM-dd')!,
        new DatePipe('en-US').transform(fechaFin, 'yyyy-MM-dd')!
      )
      .subscribe({
        next: (base64pdf: any) => {
          const link = document.createElement('a');
          link.href = 'data:application/pdf;base64,' + base64pdf.pdf;
          link.download = 'estado_cuenta.pdf';
          link.click();
        },
        error: (err) => console.error('Error al descargar el PDF', err),
      });
  }
}
