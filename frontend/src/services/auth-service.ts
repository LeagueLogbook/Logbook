"use strict";

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
    
    private storageServiceKey: string = "auth_token";
    
    constructor(private logbookApi: LogbookApi, private storageService: StorageService, private oauthHelper: OAuthHelper, private languageService: LanguageService, private urlHelper: UrlHelper) {
    }
    
    public logout() : Promise<void> {
        this.storageService.removeItem(this.storageServiceKey);
        return Promise.resolve(null);
    }
    
    public get isLoggedIn() : boolean {
        return this.storageService.getItem(this.storageServiceKey) !== null;
    }
    
    public get currentUserId() : boolean {
        
        if (this.isLoggedIn === false)
            return false;
        
        let token: JsonWebToken = this.storageService.getItem(this.storageServiceKey);
        let decoded = jwt.decode(token.token, "", true);
        
        return decoded.UserId;
    }
    
    public loginLogbook(emailAddress: string, password: string) : Promise<JsonWebToken> {
        return this.logbookApi.authenticationApi.loginLogbook(emailAddress, password);
    }
    
    public register(emailAddress: string, password: string) : Promise<void> {
        return this.logbookApi.authenticationApi.register(emailAddress, password, this.languageService.userLanguage);
    }
    
    public async loginMicrosoft() : Promise<void> {
        let loginUrl = await this.logbookApi.authenticationApi.getMicrosoftLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Microsoft", loginUrl, config.socialLoginRedirectUrl);
        let code = this.urlHelper.getParameter(loggedInUrl, "code");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginMicrosoft(code, config.socialLoginRedirectUrl);
        
        this.saveToken(jsonWebToken);
    }
    
    public async loginFacebook() : Promise<void> {
        let loginUrl = await this.logbookApi.authenticationApi.getFacebookLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Facebook", loginUrl, config.socialLoginRedirectUrl);
        let code = this.urlHelper.getParameter(loggedInUrl, "code");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginFacebook(code, config.socialLoginRedirectUrl);
        
        this.saveToken(jsonWebToken);
    }
    
    public async loginGoogle() : Promise<void> {
        let loginUrl = await this.logbookApi.authenticationApi.getGoogleLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Google", loginUrl, config.socialLoginRedirectUrl);
        let code = this.urlHelper.getParameter(loggedInUrl, "code");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginGoogle(code, config.socialLoginRedirectUrl);
        
        this.saveToken(jsonWebToken);
    }
    
    public async loginTwitter() : Promise<void> {        
        let loginUrl = await this.logbookApi.authenticationApi.getTwitterLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Twitter", loginUrl.url, config.socialLoginRedirectUrl);
        let verifier = this.urlHelper.getParameter(loggedInUrl, "oauth_verifier");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginTwitter(verifier, loginUrl.payload);
        
        this.saveToken(jsonWebToken);
    }
    
    private saveToken(token: JsonWebToken) : void {
        this.storageService.setItem(this.storageServiceKey, token);
    }
}