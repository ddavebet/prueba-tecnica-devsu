import { Routes } from '@angular/router';
import { InicioComponent } from './components/inicio/inicio.component';
import { MovimientosComponent } from './components/movimientos/movimientos.component';
import { CuentasComponent } from './components/cuentas/cuentas.component';
import { ClientesComponent } from './components/clientes/clientes.component';
import { ReportesComponent } from './components/reportes/reportes.component';

export const routes: Routes = [
  {
    path: '',
    component: InicioComponent,
    children: [
      { path: '', component: ClientesComponent },
      { path: 'clientes', component: ClientesComponent },
      { path: 'cuentas', component: CuentasComponent },
      { path: 'movimientos', component: MovimientosComponent },
      { path: 'reportes', component: ReportesComponent },
    ],
  },
];
