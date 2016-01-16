"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";
import {BrowserService} from "services/browser-service";

@autoinject()
export class Login {
    public emailAddress: string;
    public password: string;
    
    public registerEmailAddress: string;
    public registerPassword: string;
    public registerPasswordSecond: string;
    
    public constructor(private authService: AuthService, private aurelia: Aurelia, private browserService: BrowserService) {
        
    }
    
    public login() : Promise<void> {
        return this.authService
            .loginLogbook(this.emailAddress, this.password)
            .then(_ => this.browserService.reload());
    }
    
    public loginMicrosoft() : Promise<void> {
        return this.authService
            .loginMicrosoft()
            .then(_ => this.browserService.reload());
    }
    public loginTwitter() : Promise<void> {
        return this.authService
            .loginTwitter()
            .then(_ => this.browserService.reload());
    }
    public loginFacebook() : Promise<void> {
        return this.authService
            .loginFacebook()
            .then(_ => this.browserService.reload());
    }
    public loginGoogle() : Promise<void> {
        return this.authService
            .loginGoogle()
            .then(_ => this.browserService.reload());
    }
    
    public register() : Promise<void> {
        return this.authService
            .register(this.registerEmailAddress, this.registerPassword)
            .catch(error => alert(error));
    }
}
