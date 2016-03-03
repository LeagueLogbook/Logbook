"use strict";

import {autoinject} from "aurelia-framework";
import {HttpClient} from "aurelia-http-client";
import config from "config";
import * as crypto from "crypto-js";
import {JsonWebTokenModel} from "api/models/authentication/json-web-token-model";
import {TwitterLoginUrl} from "api/models/authentication/twitter-login-url";

@autoinject()
export class AuthenticationApi {
    
    public constructor(public httpClient: HttpClient) {
        this.httpClient.configure(f => f
            .withBaseUrl(config.webServiceUri)
            .withHeader("Content-Type", "application/json"));
    }
    
    public register(emailAddress: string, password: string, language: string) : Promise<void> {
        let body = {
            emailAddress: emailAddress,
            passwordSHA256Hash: crypto.SHA256(password).toString(crypto.enc.Base64),
            preferredLanguage: language,
        };

        return this.httpClient
            .createRequest("Authentication/Register")
            .asPost()
            .withContent(body)
            .send()
            .then(response => null)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public loginLogbook(emailAddress: string, password: string) : Promise<JsonWebTokenModel> {
        let body = {
            emailAddress: emailAddress,
            passwordSHA256Hash: crypto.SHA256(password).toString(crypto.enc.Base64),
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Logbook")
            .asPost()
            .withContent(body)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public getMicrosoftLoginUrl(redirectUrl: string) : Promise<string> {
        return this.httpClient
            .createRequest(`Authentication/Login/Microsoft/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content.url)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public loginMicrosoft(code: string, redirectUrl: string) : Promise<JsonWebTokenModel> {
        let content = {
            code: code,
            redirectUrl: redirectUrl,
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Microsoft")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public getFacebookLoginUrl(redirectUrl: string) : Promise<string> {
        return this.httpClient
            .createRequest(`Authentication/Login/Facebook/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content.url)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public loginFacebook(code: string, redirectUrl: string) : Promise<JsonWebTokenModel> {
        let content = {
            code: code,
            redirectUrl: redirectUrl,
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Facebook")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public getGoogleLoginUrl(redirectUrl: string) : Promise<string> {
        return this.httpClient
            .createRequest(`Authentication/Login/Google/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content.url)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public loginGoogle(code: string, redirectUrl: string) : Promise<JsonWebTokenModel> {
        let content = {
            code: code,
            redirectUrl: redirectUrl,
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Google")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public getTwitterLoginUrl(redirectUrl: string) : Promise<TwitterLoginUrl> {
        return this.httpClient
            .createRequest(`Authentication/Login/Twitter/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public loginTwitter(oauthVerifier: string, payload: string) : Promise<JsonWebTokenModel> {
        let content = {
            oauthVerifier: oauthVerifier,
            payload: payload,
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Twitter")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
}
