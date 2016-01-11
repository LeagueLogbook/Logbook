export class LanguageService {
    get userLanguage() {
        return navigator.language || navigator.userLanguage;
    } 
}