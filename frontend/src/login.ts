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
        this.emailAddress = "";
        this.password = "";
        this.registerEmailAddress = "";
        this.registerPassword = "";
        this.registerPasswordSecond = "";
    }
    
    public get canLogin() : boolean {
        return this.emailAddress !== null 
            && this.emailAddress !== "" 
            && this.password !== null 
            && this.password !== "";
    }
    
    public get canRegister() : boolean {
        return this.registerEmailAddress !== null 
            && this.registerEmailAddress !== "" 
            && this.registerPassword !== null 
            && this.registerPassword !== "" 
            && this.registerPasswordSecond !== null 
            && this.registerPasswordSecond !== null 
            && this.registerPassword === this.registerPasswordSecond;
    }
    
    public async login() : Promise<void> {    
        try {
            if (this.canLogin === false) {
                return;
            }
            
            await this.authService.loginLogbook(this.emailAddress, this.password);        
            this.browserService.reload();
        }
        catch (error) {
            alert(error);            
        }
    }
    
    public async loginMicrosoft() : Promise<void> {
        try {
            await this.authService.loginMicrosoft();
            this.browserService.reload();
        }
        catch (error) {
            alert(error);
        }
    }
    public async loginTwitter() : Promise<void> {
        try {
            await this.authService.loginTwitter();
            this.browserService.reload();            
        }
        catch (error) {
            alert(error);
        }
    }
    public async loginFacebook() : Promise<void> {
        try {
            await this.authService.loginFacebook();
            this.browserService.reload();            
        }
        catch (error) {
            alert(error);
        }
    }
    public async loginGoogle() : Promise<void> {
        try {
            await this.authService.loginGoogle();
            this.browserService.reload();   
        }
        catch (error) {
            alert(error);
        }
    }
    
    public async register() : Promise<void> {
        try {
            if (this.canRegister === false) {
                return;
            }
            
            await this.authService.register(this.registerEmailAddress, this.registerPassword);   
        }
        catch (error) {
            alert(error);
        }
    }
}
