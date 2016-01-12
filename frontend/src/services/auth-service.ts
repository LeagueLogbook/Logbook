import {autoinject} from 'aurelia-framework';
import {LogbookApi} from 'api/logbook-api';
import {JsonWebToken} from 'api/models/json-web-token';
import {StorageService} from 'services/storage-service';
import {LanguageService} from 'services/language-service';
import {OAuthHelper} from 'helper/oauth-helper';
import {UrlHelper} from 'helper/url-helper';
import config from 'config';
import * as crypto from 'crypto-js';
import * as jwt from 'jwt-simple';

@autoinject()
export class AuthService {
    
    storageServiceKey: string = "auth_token";
    
    constructor(private logbookApi: LogbookApi, private storageService: StorageService, private oauthHelper: OAuthHelper, private languageService: LanguageService, private urlHelper: UrlHelper) {
    }
    
    logout() : Promise<void> {
        this.storageService.removeItem(this.storageServiceKey);
        return Promise.resolve(null);
    }
    
    get isLoggedIn() : boolean {
        return this.storageService.getItem(this.storageServiceKey) !== null;
    }
    
    get currentUserId() : boolean {
        
        if (this.isLoggedIn === false)
            return false;
        
        let token: JsonWebToken = this.storageService.getItem(this.storageServiceKey);
        let decoded = jwt.decode(token.token, "", true);
        
        return decoded.UserId;
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
            .then(url => this.oauthHelper.showOAuthPopup("Microsoft", url, config.socialLoginRedirectUrl))
            .then(url => this.urlHelper.getParameter(url, "code"))                    
            .then(code => this.logbookApi.authenticationApi.loginMicrosoft(code, config.socialLoginRedirectUrl))
            .then(token => this.saveToken(token));
    }
    
    loginFacebook() : Promise<void> {
        return this.logbookApi.authenticationApi
            .getFacebookLoginUrl(config.socialLoginRedirectUrl)
            .then(url => this.oauthHelper.showOAuthPopup("Facebook", url, config.socialLoginRedirectUrl))
            .then(url => this.urlHelper.getParameter(url, "code"))
            .then(code => this.logbookApi.authenticationApi.loginFacebook(code, config.socialLoginRedirectUrl))
            .then(token => this.saveToken(token));
    }
    
    loginGoogle() : Promise<void> {
        return this.logbookApi.authenticationApi
            .getGoogleLoginUrl(config.socialLoginRedirectUrl)
            .then(url => this.oauthHelper.showOAuthPopup("Google", url, config.socialLoginRedirectUrl))
            .then(url => this.urlHelper.getParameter(url, "code"))
            .then(code => this.logbookApi.authenticationApi.loginGoogle(code, config.socialLoginRedirectUrl))
            .then(token => this.saveToken(token));
    }
    
    loginTwitter() : Promise<void> {        
        let payload: string;
        
        return this.logbookApi.authenticationApi
            .getTwitterLoginUrl(config.socialLoginRedirectUrl)
            .then(url => {
                payload = url.payload;
                return this.oauthHelper.showOAuthPopup("Twitter", url.url, config.socialLoginRedirectUrl);
            })
            .then(url => this.urlHelper.getParameter(url, "oauth_verifier"))
            .then(verifier => this.logbookApi.authenticationApi.loginTwitter(verifier, payload))
            .then(token => this.saveToken(token));
    }
    
    private saveToken(token: JsonWebToken) {
        this.storageService.setItem(this.storageServiceKey, token);
    }
}