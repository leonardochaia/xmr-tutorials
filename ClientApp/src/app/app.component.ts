import { Component, Renderer2, OnDestroy } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnDestroy {

  protected currentTheme: string;

  constructor(private overlayContainer: OverlayContainer,
    private renderer: Renderer2) {

    this.addBaseClasses();

    this.currentTheme = 'app-theme';

    // Set the theme to the overlay
    // neded for Material modals/overlays
    const classList = overlayContainer.getContainerElement().classList;
    classList.add(this.currentTheme);

    // Set the class to the document
    renderer.addClass(document.documentElement, this.currentTheme);
  }

  public ngOnDestroy() {
    // This is done mostly for HMR.
    this.clearThemeClasses();
    this.clearBaseClasses();
  }

  private clearThemeClasses() {
    if (this.currentTheme) {
      const classList = this.overlayContainer.getContainerElement().classList;
      classList.remove(this.currentTheme);
      this.renderer.removeClass(document.documentElement, this.currentTheme);
    }
  }

  /**
   * Adds classes needed despite of the theme being used
   */
  private addBaseClasses() {
    const classList = this.overlayContainer.getContainerElement().classList;
    const base = 'mat-typography';
    classList.add(base);
    this.renderer.addClass(document.documentElement, base);
    this.renderer.addClass(document.body, 'app-body-bg');
  }

  private clearBaseClasses() {
    const classList = this.overlayContainer.getContainerElement().classList;
    const base = 'mat-typography';
    classList.remove(base);
    this.renderer.removeClass(document.documentElement, base);
    this.renderer.removeClass(document.body, 'app-body-bg');
  }
}
