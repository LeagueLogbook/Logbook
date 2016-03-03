"use strict";

import {autoinject} from "aurelia-framework";
import {LogbookApi} from "api/logbook-api";
import {JsonWebTokenModel} from "api/models/authentication/json-web-token-model";
import {StorageService} from "services/storage-service";
import {LanguageService} from "services/language-service";
import {OAuthHelper} from "helper/oauth-helper";
import {UrlHelper} from "helper/url-helper";
import config from "config";
import * as jwt from "jwt-simple";

@autoinject()
export class AuthService {
    
    public get token() : JsonWebTokenModel {
        return this.storageService.getItem("auth_token");
    }
    public set token(token: JsonWebTokenModel) {
        this.storageService.setItem("auth_token", token);
    }    
    
    public get isLoggedIn() : boolean {
        return this.token !== null;
    }
    
    public get currentUserId() : boolean {        
        if (this.isLoggedIn === false) {
            return false;
        }
        
        let decoded = jwt.decode(this.token.token, "", true);        
        return decoded.UserId;
    }
    
    public constructor(private logbookApi: LogbookApi, private storageService: StorageService, private oauthHelper: OAuthHelper, private languageService: LanguageService, private urlHelper: UrlHelper) {
    }
    
    public logout() : Promise<void> {
        this.token = null;
        return Promise.resolve(null);
    }
    
    public async loginLogbook(emailAddress: string, password: string) : Promise<void> {
        let jsonWebToken = await this.logbookApi.authenticationApi.loginLogbook(emailAddress, password);
        
        this.token = jsonWebToken;
    }
    
    public register(emailAddress: string, password: string) : Promise<void> {
        return this.logbookApi.authenticationApi.register(emailAddress, password, this.languageService.userLanguage);
    }
    
    public async loginMicrosoft() : Promise<void> {
        let loginUrl = await this.logbookApi.authenticationApi.getMicrosoftLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Microsoft", loginUrl, config.socialLoginRedirectUrl);
        let code = this.urlHelper.getParameter(loggedInUrl, "code");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginMicrosoft(code, config.socialLoginRedirectUrl);
        
        this.token = jsonWebToken;
    }
    
    public async loginFacebook() : Promise<void> {
        let loginUrl = await this.logbookApi.authenticationApi.getFacebookLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Facebook", loginUrl, config.socialLoginRedirectUrl);
        let code = this.urlHelper.getParameter(loggedInUrl, "code");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginFacebook(code, config.socialLoginRedirectUrl);
        
        this.token = jsonWebToken;
    }
    
    public async loginGoogle() : Promise<void> {
        let loginUrl = await this.logbookApi.authenticationApi.getGoogleLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Google", loginUrl, config.socialLoginRedirectUrl);
        let code = this.urlHelper.getParameter(loggedInUrl, "code");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginGoogle(code, config.socialLoginRedirectUrl);
        
        this.token = jsonWebToken;
    }
    
    public async loginTwitter() : Promise<void> {        
        let loginUrl = await this.logbookApi.authenticationApi.getTwitterLoginUrl(config.socialLoginRedirectUrl);
        let loggedInUrl = await this.oauthHelper.showOAuthPopup("Twitter", loginUrl.url, config.socialLoginRedirectUrl);
        let verifier = this.urlHelper.getParameter(loggedInUrl, "oauth_verifier");
        let jsonWebToken = await this.logbookApi.authenticationApi.loginTwitter(verifier, loginUrl.payload);
        
        this.token = jsonWebToken;
    }
}
