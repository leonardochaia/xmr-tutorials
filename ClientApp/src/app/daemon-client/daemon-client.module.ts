import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DaemonClientService } from './daemon-client.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    DaemonClientService
  ]
})
export class DaemonClientModule { }
