"use strict";

import {Router, RouterConfiguration} from "aurelia-router";

export class AppLoggedIn {
    
    public router: Router;
    
    public configureRouter(config: RouterConfiguration, router: Router) {
        config.title = "Aurelia";
        config.map([
            { 
                route: ["", "welcome"], 
                name: "welcome",      
                moduleId: "welcome",      
                nav: true, 
                title: "Welcome",
            },
            { 
                route: "logout", 
                name: "logout",      
                moduleId: "logout",      
                nav: false, 
                title: "Logout",
            },
        ]);

        this.router = router;
    }
}
