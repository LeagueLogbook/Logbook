import {AuthService} from './services/auth-service';
import {autoinject} from 'aurelia-framework';

 @autoinject()
export class Welcome {
    
    constructor(public authService: AuthService) {
        
    }
    
    loginMicrosoft(): Promise<void> {
        alert("Jo");
        return this.authService.loginMicrosoft();
    }    
}