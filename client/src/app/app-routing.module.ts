import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthModule as AppAuthModule } from './modules/auth/auth.module';
import { MainModule } from './modules/main/main.module';

const routes: Routes = [
  {
    path: 'main',
    loadChildren: () =>
      import('./modules/main/main.module').then((m) => m.MainModule),
  },
  {
    path: '',
    redirectTo: 'main',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./modules/auth/auth.module').then((m) => m.AuthModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes), AppAuthModule, MainModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
