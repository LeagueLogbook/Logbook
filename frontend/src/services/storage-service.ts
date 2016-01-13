"use strict";

export class StorageService {
    public setItem(key: string, value: Object) {
        localStorage.setItem(key, JSON.stringify(value));
    }
    public getItem(key: string) {
        return JSON.parse(localStorage.getItem(key));
    }
    public removeItem(key: string) {
        localStorage.removeItem(key);
    }
}
