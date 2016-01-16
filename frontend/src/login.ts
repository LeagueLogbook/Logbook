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
    
    public get canLogin() : boolean {
        return 
            this.emailAddress != null && 
            this.emailAddress != "" && 
            this.password != null && 
            this.password != "";
    }
    
    public get canRegister() : boolean {
        return  
            this.registerEmailAddress != null &&
            this.registerEmailAddress != "" &&
            this.registerPassword != null &&
            this.registerPassword != "" &&
            this.registerPasswordSecond != null &&
            this.registerPasswordSecond != null &&
            this.registerPassword == this.registerPasswordSecond;
    }
    
    public async login() : Promise<void> {    
        try {
            if (this.canLogin == false) {
                return;
            }
            
            await this.authService.loginLogbook(this.emailAddress, this.password)        
            this.browserService.reload();
        }
        catch(error) {
            alert(error);            
        }
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
    
    public async register() : Promise<void> {
        try {
            if (this.canRegister == false) {
                return;
            }
            
            await this.authService.register(this.registerEmailAddress, this.registerPassword);   
        }
        catch(error) {
            alert(error);
        }
    }
}
