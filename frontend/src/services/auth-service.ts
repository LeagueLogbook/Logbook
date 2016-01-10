import {autoinject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';
import {StorageService} from './storage-service';
import {OAuth2Helper} from '../helper/oauth2Helper';
import config from '../config';
import * as crypto from 'crypto-js';

@autoinject()
export class AuthService {
    
    storageServiceKey: string = "auth_token";
    
    constructor(public httpClient : HttpClient, public storageService : StorageService, public oauth2Helper: OAuth2Helper) {                
        this.httpClient.configure(f =>f
            .withBaseUrl(config.webServiceUri)
            .withHeader("Content-Type", "application/json"));
    }
    
    loginLogbook(emailAddress: string, password: string) : Promise<string> {
                
        let body = {
            emailAddress: emailAddress,
            passwordSHA256Hash: crypto.SHA256(password).toString(crypto.enc.Base64)
        };
        
        return this.httpClient
            .createRequest("Authenticatino/Login/Logbook")
            .withContent(body)
            .asPost()
            .send()
            .then(response => {
               this.storageService.setItem(this.storageServiceKey, response.content);
            })
            .catch(response => {
                var message = response.content.message;
                return Promise.reject(message);
            });
    }
}