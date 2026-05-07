import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeAgoToday'
})
export class TimeAgoTodayPipe implements PipeTransform {

   transform(value: string | Date): string {
    if (!value) return '';

    const date = new Date(value);
    const now = new Date();

    const diffMs = now.getTime() - date.getTime();
    const minutes = Math.floor(diffMs / 60000);
    const hours = Math.floor(diffMs / 3600000);

    if (diffMs < 0) return 'الآن';

    if (minutes < 1)
      return 'الآن';

    if (minutes < 60)
      return `منذ ${minutes} دقيقة`;

    return `منذ ${hours} ساعة`;
  }

}
