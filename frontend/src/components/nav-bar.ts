"use strict";

import {AuthService} from "services/auth-service";
import {Router} from "aurelia-router";
import {bindable, autoinject} from "aurelia-framework";

@autoinject()
export class NavBar {
    
    @bindable()
    public router: Router;    
    
    public constructor(private authService: AuthService) {   
    }
    
    public get isLoggedIn() {
        return this.authService.isLoggedIn;
    }   
}
