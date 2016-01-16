"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";

@autoinject()
export class Login {
    public constructor(private authService: AuthService, private aurelia: Aurelia) {
        
    }
    
    public activate() : Promise<void> {
        return new Promise<void>((resolve, reject) => {
            resolve();
        });
    }
    
    public loginMicrosoft() : Promise<void> {
        return this.authService
            .loginMicrosoft()
            .then(_ => this.setAureliaRootToAppLoggedIn());
    }
    public loginTwitter() : Promise<void> {
        return this.authService
            .loginTwitter()
            .then(_ => this.setAureliaRootToAppLoggedIn());
    }
    public loginFacebook() : Promise<void> {
        return this.authService
            .loginFacebook()
            .then(_ => this.setAureliaRootToAppLoggedIn());
    }
    public loginGoogle() : Promise<void> {
        return this.authService
            .loginGoogle()
            .then(_ => this.setAureliaRootToAppLoggedIn());
    }
    
    private setAureliaRootToAppLoggedIn() {
        this.aurelia.setRoot("app-logged-in");
    }
}