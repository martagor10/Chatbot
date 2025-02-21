import { MessageKind } from './message-kind';
import { MessageRating } from './message-rating';

export interface ApiMessage {
  id: string;
  createdAt: Date;
  kind: MessageKind;
  text: string;
  rating?: MessageRating;
}

export interface LocalMessage extends ApiMessage {
  inProgress?: boolean;
}
