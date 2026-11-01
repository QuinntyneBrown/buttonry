import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonDirective, CenterStateComponent } from '@buttonry/components';

/** The page for any unknown address (mocks pages/not-found). */
@Component({
  selector: 'bt-not-found-page',
  imports: [RouterLink, ButtonDirective, CenterStateComponent],
  template: `
    <bt-center-state code="404" heading="Page not found">
      <p class="text-muted">The link may be old or mistyped.</p>
      <a actions btButton="primary" routerLink="/">Back to shop</a>
    </bt-center-state>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotFoundPage {}
