import { DaemonClientModule } from './daemon-client.module';

describe('DaemonClientModule', () => {
  let daemonClientModule: DaemonClientModule;

  beforeEach(() => {
    daemonClientModule = new DaemonClientModule();
  });

  it('should create an instance', () => {
    expect(daemonClientModule).toBeTruthy();
  });
});
