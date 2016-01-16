"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";

@autoinject()
export class Logout {
    
    public constructor(private authService: AuthService, private aurelia: Aurelia) {
        
    }
    
    public activate() : Promise<void> {
        return this.authService
            .logout()
            .then(_ => this.setAureliaRootToAppLoggedOut());
    }
    
    private setAureliaRootToAppLoggedOut() {
        this.aurelia.setRoot("app-logged-out");
    }
}