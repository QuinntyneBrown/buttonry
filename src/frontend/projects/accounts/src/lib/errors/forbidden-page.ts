import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonDirective, CenterStateComponent } from '@buttonry/components';

/** /403: another customer's page, or a founder-only route opened by a customer (mocks pages/forbidden). */
@Component({
  selector: 'bt-forbidden-page',
  imports: [RouterLink, ButtonDirective, CenterStateComponent],
  template: `
    <bt-center-state code="403" heading="You can’t open this">
      <p class="text-muted">{{ message() }}</p>
      <a actions btButton="primary" routerLink="/account">Go to your account</a>
    </bt-center-state>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ForbiddenPage {
  /** `management` when a signed-in customer opened a founder-only route. */
  readonly reason = input<string>('');
  readonly message = computed(() =>
    this.reason() === 'management'
      ? 'Shop management is only for the shop’s founder.'
      : 'It belongs to another customer’s account.',
  );
}
