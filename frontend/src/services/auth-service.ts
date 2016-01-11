import {autoinject} from 'aurelia-framework';
import {LogbookApi} from '../api/logbook-api';
import {JsonWebToken} from '../api/models/json-web-token';
import {StorageService} from './storage-service';
import {LanguageService} from './language-service';
import {OAuth2Helper} from '../helper/oauth2Helper';
import config from '../config';
import * as crypto from 'crypto-js';

@autoinject()
export class AuthService {
    
    storageServiceKey: string = "auth_token";
    
    constructor(public logbookApi : LogbookApi, public storageService : StorageService, public oauth2Helper: OAuth2Helper, public languageService: LanguageService) {
    }
    
    get isLoggedIn() {
        return this.storageService.getItem(this.storageServiceKey) !== null;
    }
    
    loginLogbook(emailAddress: string, password: string) : Promise<JsonWebToken> {
        return this.logbookApi.authenticationApi.loginLogbook(emailAddress, password);
    }
    
    register(emailAddress: string, password: string) : Promise<void> {
        return this.logbookApi.authenticationApi.register(emailAddress, password, this.languageService.userLanguage);
    }
    
    loginMicrosoft() : Promise<void> {
        return this.logbookApi.authenticationApi
            .getMicrosoftLoginUrl(config.socialLoginRedirectUrl)
            .then(url => {                
                return this.oauth2Helper
                    .showOAuth2Popup("Microsoft", url, config.socialLoginRedirectUrl)
                    .then(code => {
                        return this.logbookApi.authenticationApi
                            .loginMicrosoft(code, config.socialLoginRedirectUrl)
                            .then(token => {
                                this.storageService.setItem(this.storageServiceKey, token); 
                            });
                    });
            });
    }
}