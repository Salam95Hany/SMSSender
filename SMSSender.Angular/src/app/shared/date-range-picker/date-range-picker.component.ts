import { Component, ElementRef, EventEmitter, Input, Output, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-date-range-picker',
  imports: [],
  templateUrl: './date-range-picker.component.html',
  styleUrl: './date-range-picker.component.css'
})
export class DateRangePickerComponent implements AfterViewInit, OnDestroy {
  @ViewChild('input', { static: true }) input!: ElementRef
  @Input() placeholder = 'اختر الفترة';
  @Input() closedPeriods: { from: Date, to: Date }[] = [];
  @Output() rangeSelected = new EventEmitter<{ from: Date, to: Date } | null>();
  fpInstance: any;

  ngAfterViewInit(): void {
    this.initPicker();
  }

  openPicker() {
    this.fpInstance.open();
  }

  initPicker() {
    this.fpInstance = flatpickr(this.input.nativeElement, {
      mode: 'range',
      dateFormat: 'Y-m-d',

      disable: this.closedPeriods.map(p => ({
        from: p.from,
        to: p.to
      })),
      clickOpens: false,

      onDayCreate: (dObj, dStr, fp, dayElem) => {
        const date = dayElem.dateObj;

        const isClosed = this.closedPeriods.some(p =>
          date >= p.from && date <= p.to
        );

        if (isClosed) {
          dayElem.classList.add('closed-date');
        }
      },

      onChange: (selectedDates: Date[]) => {
        if (selectedDates.length === 2) {
          this.rangeSelected.emit({
            from: selectedDates[0],
            to: selectedDates[1]
          });
        }

        if (selectedDates.length === 0) {
          this.rangeSelected.emit(null);
        }
      }
    });
  }

  clear() {
    if (this.fpInstance) {
      this.fpInstance.clear();
    }
  }

  ngOnDestroy(): void {
    if (this.fpInstance) {
      this.fpInstance.destroy();
    }
  }
}
