import { ChangeDetectionStrategy, Component } from '@angular/core';
import { DialogComponent, VideoPlayerComponent, VideoSource } from '@buttonry/components';

/** The pitch video in a dialog; it plays because the visitor pressed the poster (L2-078, L2-083). */
@Component({
  selector: 'bt-pitch-video-dialog',
  imports: [DialogComponent, VideoPlayerComponent],
  templateUrl: './pitch-video-dialog.html',
  styleUrl: './pitch-video-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PitchVideoDialog {
  readonly sources: VideoSource[] = [
    { src: '/media/buttonry-pitch.mp4', type: 'video/mp4' },
    { src: '/media/buttonry-pitch.webm', type: 'video/webm' },
  ];
}
