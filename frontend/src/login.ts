"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";
import {BrowserService} from "services/browser-service";

@autoinject()
export class Login {
    public constructor(private authService: AuthService, private aurelia: Aurelia, private browserService: BrowserService) {
        
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
}