import {Pipe, PipeTransform} from '@angular/core';
import {MessageRating} from '../models/message-rating';

@Pipe({
  name: 'rating'
})
export class RatingPipe implements PipeTransform {
  transform(value: MessageRating | undefined): string {
    switch (value) {
      case MessageRating.Negative:
        return 'Bad';
      case MessageRating.Positive:
        return 'Ok';
      default:
        return '-';
    }
  }
}
