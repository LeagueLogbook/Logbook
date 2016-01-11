import {AuthService} from './services/auth-service';
import {autoinject} from 'aurelia-framework';

 @autoinject()
export class Welcome {
    
    constructor(public authService: AuthService) {
        
    }
    
    activate() : Promise<void> {
        return this.authService
            .loginLogbook("haefele@xemio.net", "123456")
            .then(token => alert(token.token))
            .catch(error => alert(error));
    }
}