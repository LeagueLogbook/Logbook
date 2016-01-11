import {autoinject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';
import {AuthenticationApi} from './authentication-api';

@autoinject()
export class LogbookApi {
    
    constructor(public authenticationApi: AuthenticationApi) {
                
    }
}