import {AuthService} from 'services/auth-service';
import {autoinject} from 'aurelia-framework';

@autoinject()
export class LoggedOut {
    constructor(private authService: AuthService) {
        
    }
    
    loginMicrosoft() : Promise<void> {
        return this.authService.loginMicrosoft();
    }
    loginTwitter() {
        
    }
    loginFacebook() {
        return this.authService.loginFacebook();
    }
    loginGoogle() {
        
    }
}