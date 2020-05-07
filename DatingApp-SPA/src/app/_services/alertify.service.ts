import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable()
export class AlertifyService {
  constructor() {}

  confirm(message: string, okCallback: () => any) {
    // using alertify to respond to an okcallback event
    alertify.confirm(message, (e: any) => {
      // if the user has started an event, ie clicked a button
      if (e) {
        okCallback();
      } else {
      }
    });
  }

  success(message: string) {
    alertify.success(message);
  }

  message(message: string) {
    alertify.message(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  error(message: string) {
    alertify.error(message);
  }
}
