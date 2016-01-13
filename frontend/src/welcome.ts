"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";

 @autoinject()
export class Welcome {
    
    public constructor(private authService: AuthService, private aurelia: Aurelia) {
        
    }
    
    public logout(): Promise<void> {
        return this.authService
            .logout()
            .then(_ => this.setAureliaRootToLoggedOut());
    }
    
    public currentUserId(): void {
        alert(this.authService.currentUserId);
    }
    
    private setAureliaRootToLoggedOut() {
        this.aurelia.setRoot("loggedOut");
    }
}
