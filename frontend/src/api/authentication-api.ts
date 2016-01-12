import {autoinject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';
import config from 'config';
import * as crypto from 'crypto-js';
import {JsonWebToken} from 'api/models/json-web-token';

@autoinject()
export class AuthenticationApi {
    
    constructor(public httpClient: HttpClient) {
        this.httpClient.configure(f =>f
            .withBaseUrl(config.webServiceUri)
            .withHeader("Content-Type", "application/json"));
    }
    
    register(emailAddress: string, password: string, language: string) : Promise<void> {
        let body = {
            emailAddress: emailAddress,
            passwordSHA256Hash: crypto.SHA256(password).toString(crypto.enc.Base64),
            preferredLanguage: language  
        };
        
        return this.httpClient
            .createRequest("Authentication/Register")
            .asPost()
            .withContent(body)
            .send()
            .then(response => {
                return null;
            })
            .catch(response => {
                return Promise.reject(response.content.message);
            });
    }
    
    loginLogbook(emailAddress: string, password: string) : Promise<JsonWebToken> {
        let body = {
            emailAddress: emailAddress,
            passwordSHA256Hash: crypto.SHA256(password).toString(crypto.enc.Base64)
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Logbook")
            .asPost()
            .withContent(body)
            .send()
            .then(response => {
                return response.content;
            })
            .catch(response => {
                return Promise.reject(response.content.message);
            });
    }
    
    getMicrosoftLoginUrl(redirectUrl: string) : Promise<string> {
        return this.httpClient
            .createRequest(`Authentication/Login/Microsoft/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content.url)
            .catch(response => Promise.reject(response.content.message));
    }
    
    loginMicrosoft(code: string, redirectUrl: string) : Promise<JsonWebToken> {
        let content = {
            code: code,
            redirectUrl: redirectUrl  
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Microsoft")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    getFacebookLoginUrl(redirectUrl: string) : Promise<string> {
        return this.httpClient
            .createRequest(`Authentication/Login/Facebook/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content.url)
            .catch(response => Promise.reject(response.content.message));
    }
    
    loginFacebook(code: string, redirectUrl: string) : Promise<JsonWebToken> {
        let content = {
            code: code,
            redirectUrl: redirectUrl
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Facebook")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    getGoogleLoginUrl(redirectUrl: string) : Promise<string> {
        return this.httpClient
            .createRequest(`Authentication/Login/Google/Url?redirectUrl=${encodeURIComponent(redirectUrl)}`)
            .asGet()
            .send()
            .then(response => response.content.url)
            .catch(response => Promise.reject(response.content.message));
    }
    
    loginGoogle(code: string, redirectUrl: string) : Promise<JsonWebToken> {
        let content = {
            code: code,
            redirectUrl: redirectUrl
        };
        
        return this.httpClient
            .createRequest("Authentication/Login/Google")
            .asPost()
            .withContent(content)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
}