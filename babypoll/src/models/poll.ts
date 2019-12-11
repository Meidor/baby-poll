import { IEntry } from "./entry";

export interface IPoll {
  pollId: string;
  name: string;
  url: string;
  dueDate: string;
  entries: IEntry[];
}
