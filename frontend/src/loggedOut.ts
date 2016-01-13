"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";

@autoinject()
export class LoggedOut {
    public constructor(private authService: AuthService, private aurelia: Aurelia) {
        
    }
    
    public loginMicrosoft() : Promise<void> {
        return this.authService
            .loginMicrosoft()
            .then(_ => this.setAureliaRootToApp());
    }
    public loginTwitter() : Promise<void> {
        return this.authService
            .loginTwitter()
            .then(_ => this.setAureliaRootToApp());
    }
    public loginFacebook() : Promise<void> {
        return this.authService
            .loginFacebook()
            .then(_ => this.setAureliaRootToApp());
    }
    public loginGoogle() : Promise<void> {
        return this.authService
            .loginGoogle()
            .then(_ => this.setAureliaRootToApp());
    }
    
    private setAureliaRootToApp() {
        this.aurelia.setRoot("app");
    }
}
