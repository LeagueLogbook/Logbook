"use strict";

import {AuthService} from "services/auth-service";
import {autoinject, Aurelia} from "aurelia-framework";
import {BrowserService} from "services/browser-service";

@autoinject()
export class Logout {
    
    public constructor(private authService: AuthService, private aurelia: Aurelia, private browserService: BrowserService) {
        
    }
    
    public activate() : Promise<void> {
        return this.authService
            .logout()
            .then(_ => this.browserService.reload());
    }
}
