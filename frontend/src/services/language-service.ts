"use strict";

export class LanguageService {
    public get userLanguage() {
        return navigator.language || navigator.userLanguage;
    } 
}
