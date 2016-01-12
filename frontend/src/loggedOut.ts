import {AuthService} from 'services/auth-service';
import {autoinject, Aurelia} from 'aurelia-framework';

@autoinject()
export class LoggedOut {
    constructor(private authService: AuthService, private aurelia: Aurelia) {
        
    }
    
    loginMicrosoft() : Promise<void> {
        return this.authService
            .loginMicrosoft()
            .then(_ => this.setAureliaRootToApp());
    }
    loginTwitter() : Promise<void> {
        return this.authService
            .loginTwitter()
            .then(_ => this.setAureliaRootToApp());
    }
    loginFacebook() : Promise<void> {
        return this.authService
            .loginFacebook()
            .then(_ => this.setAureliaRootToApp());
    }
    loginGoogle() : Promise<void> {
        return this.authService
            .loginGoogle()
            .then(_ => this.setAureliaRootToApp())
    }
    
    private setAureliaRootToApp() {
        this.aurelia.setRoot("app");
    }
}