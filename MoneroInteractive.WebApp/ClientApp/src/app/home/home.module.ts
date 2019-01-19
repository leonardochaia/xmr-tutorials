import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TutorialsModule } from '../tutorials/tutorials.module';
import { MatCardModule, MatIconModule, MatButtonModule } from '@angular/material';
import { HomeComponent } from './home.component';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,

    TutorialsModule,
  ],
  declarations: [
    HomeComponent,
  ],
})
export class HomeModule { }
