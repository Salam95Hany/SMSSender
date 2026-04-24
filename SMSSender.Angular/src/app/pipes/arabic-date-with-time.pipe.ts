import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arabicDateWithTime'
})
export class ArabicDateWithTimePipe implements PipeTransform {

  transform(value: string | Date): string {
    if (!value) return '-';

    const date = new Date(value);

    const datePart = date.toLocaleDateString('ar-EG-u-nu-latn', {
      day: 'numeric',
      month: 'long',
      year: 'numeric'
    });

    const hours = date.getHours();
    const minutes = date.getMinutes();

    if (hours === 0 && minutes === 0) {
      return datePart;
    }

    const timePart = date.toLocaleTimeString('ar-EG-u-nu-latn', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true
    });

    return `${datePart} - ${timePart}`;
  }

}
