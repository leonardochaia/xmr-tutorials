import { Injectable, Inject, OnDestroy } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { AppStatusDto } from './app-status.model';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AppStatusService implements OnDestroy {

  public get status$() {
    return this.statusSubject.asObservable();
  }

  private connection: HubConnection;

  private statusSubject = new Subject<AppStatusDto>();

  constructor() {

    this.connection = new HubConnectionBuilder()
      .withUrl(environment.apiUrl + 'hubs/app-status')
      .build();

    // TODO: error handling with angular?
    this.connection.start().catch(console.error);

    this.connection.on('statusChanged', (newStatus: AppStatusDto) => {
      console.log(newStatus);
      this.statusSubject.next(newStatus);
    });
  }

  public ngOnDestroy() {
    this.connection.off('statusChange');
    this.statusSubject.complete();
  }
}
