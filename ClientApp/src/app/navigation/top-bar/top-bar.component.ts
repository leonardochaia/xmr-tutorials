import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppStatusService } from 'src/app/app-status/app-status.service';
import { AppStatusDto } from 'src/app/app-status/app-status.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.scss']
})
export class TopBarComponent implements OnInit, OnDestroy {

  public currentStatus: AppStatusDto;

  private destroyedSubject = new Subject();

  constructor(private readonly appStatus: AppStatusService) { }

  public ngOnInit() {
    this.appStatus.status$
      .pipe(takeUntil(this.destroyedSubject))
      .subscribe(newStatus => {
        this.currentStatus = newStatus;
      });
  }

  public ngOnDestroy(): void {
    this.destroyedSubject.next();
    this.destroyedSubject.complete();
  }
}
