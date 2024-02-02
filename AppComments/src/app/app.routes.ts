import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { ThreadsComponent } from './layout/Pages/threads/threads.component';
import { LogInComponent } from './layout/Pages/log-in/log-in.component';
import { SingUpComponent } from './layout/Pages/sing-up/sing-up.component';
import { ThreadComponent } from './layout/Pages/thread/thread.component';
import { NewThreadComponent } from './layout/Pages/new-thread/new-thread.component';
import { EditAccountComponent } from './layout/Pages/edit-account/edit-account.component';

export const routes: Routes = [{
    path:'',
    component:LayoutComponent,
    children: [
        {path:'threads', component:ThreadsComponent},
        {path:'singup', component:SingUpComponent},
        {path:'login', component:LogInComponent},
        {path:'thread/:id', component:ThreadComponent},
        {path:'newthread', component:NewThreadComponent},
        {path:'account', component:EditAccountComponent},
        {path:'', component:ThreadsComponent},
        {path:'**', component:ThreadsComponent}
    ]
}];
// need to add route guards.