"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";
import {BrowserService} from "services/browser-service";
import {Command} from "helper/command";

@autoinject()
export class Login {
    public emailAddress: string;
    public password: string;
    
    public registerEmailAddress: string;
    public registerPassword: string;
    public registerPasswordSecond: string;
    
    public loginCommand: Command;
    public loginMicrosoftCommand: Command;
    public loginFacebookCommand: Command;
    public loginGoogleCommand: Command;
    public loginTwitterCommand: Command;
    public registerCommand: Command;
        
    public constructor(private authService: AuthService, private aurelia: Aurelia, private browserService: BrowserService) {
        this.emailAddress = "";
        this.password = "";
        this.registerEmailAddress = "";
        this.registerPassword = "";
        this.registerPasswordSecond = "";
        
        this.loginCommand = new Command(() => this.login(), () => this.canLogin());
        this.loginMicrosoftCommand = new Command(() => this.loginMicrosoft());
        this.loginFacebookCommand = new Command(() => this.loginFacebook());
        this.loginGoogleCommand = new Command(() => this.loginGoogle());
        this.loginTwitterCommand = new Command(() => this.loginTwitter());
        this.registerCommand = new Command(() => this.register(), () => this.canRegister());
    }
    
    private canLogin() : boolean {
        return this.emailAddress !== null
            && this.emailAddress !== "" 
            && this.password !== null
            && this.password !== "";
    }
        
    private async login() : Promise<void> {
        await this.authService.loginLogbook(this.emailAddress, this.password);        
        this.browserService.reload();
    }
    
    private canRegister() : boolean {
        return this.registerEmailAddress !== null 
            && this.registerEmailAddress !== "" 
            && this.registerPassword !== null 
            && this.registerPassword !== "" 
            && this.registerPasswordSecond !== null 
            && this.registerPasswordSecond !== null 
            && this.registerPassword === this.registerPasswordSecond;
    }
    
    private async register() : Promise<void> {
        await this.authService.register(this.registerEmailAddress, this.registerPassword);
    }
    
    private async loginMicrosoft() : Promise<void> {
        await this.authService.loginMicrosoft();
        this.browserService.reload();
    }
    private async loginTwitter() : Promise<void> {
        await this.authService.loginTwitter();
        this.browserService.reload();            
    }
    private async loginFacebook() : Promise<void> {
        await this.authService.loginFacebook();
        this.browserService.reload();            
    }
    private async loginGoogle() : Promise<void> {
        await this.authService.loginGoogle();
        this.browserService.reload();
    }
}
