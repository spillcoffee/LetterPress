export class Datum<T> {
  state: unknown;
  set(key: keyof T, value: unknown) {
    this.state[key] = value;
  }
  get(key: keyof T) {
    return this.state[key];
  }
}