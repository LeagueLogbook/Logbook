import {AuthService} from 'services/auth-service';
import {autoinject, Aurelia} from 'aurelia-framework';

 @autoinject()
export class Welcome {
    
    constructor(private authService: AuthService, private aurelia: Aurelia) {
        
    }
    
    logout(): Promise<void> {
        return this.authService
            .logout()
            .then(_ => this.setAureliaRootToLoggedOut());
    }
    
    private setAureliaRootToLoggedOut() {
        this.aurelia.setRoot("loggedOut");
    }
}