import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TutorialListComponent } from './tutorial-list/tutorial-list.component';
import { MatCardModule, MatIconModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    FlexLayoutModule,
  ],
  declarations: [
    TutorialListComponent,
  ],
  exports: [
    TutorialListComponent
  ]
})
export class TutorialsModule { }
