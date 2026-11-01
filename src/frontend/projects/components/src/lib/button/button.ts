import { Directive, input } from '@angular/core';

/**
 * Styles a native button or link as a Heirloom pill button (design system: Button). Variants
 * primary (terracotta), secondary (default: outlined ink), inverse (ink, hover forest; for the
 * terracotta band), ghost, link and danger; sizes sm, md and lg; block fills the row; busy sets
 * aria-busy and keeps the name. Hover changes colour only.
 */
@Directive({
  selector: '[btButton]',
  host: {
    class: 'btn',
    '[class.btn--primary]': "btButton() === 'primary'",
    '[class.btn--inverse]': "btButton() === 'inverse'",
    '[class.btn--ghost]': "btButton() === 'ghost'",
    '[class.btn--link]': "btButton() === 'link'",
    '[class.btn--danger]': "btButton() === 'danger'",
    '[class.btn--sm]': "size() === 'sm'",
    '[class.btn--lg]': "size() === 'lg'",
    '[class.btn--block]': 'block()',
    '[attr.aria-busy]': "busy() ? 'true' : null",
  },
})
export class ButtonDirective {
  readonly btButton = input<'' | 'primary' | 'inverse' | 'ghost' | 'link' | 'danger'>('');
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly block = input(false);
  readonly busy = input(false);
}
