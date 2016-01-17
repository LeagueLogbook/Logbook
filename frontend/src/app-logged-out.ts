"use strict";

import {Router, RouterConfiguration} from "aurelia-router";

export class AppLoggedOut {
    
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
                route: "login",
                name: "login",
                moduleId: "login",
                nav: false,
                title: "Login",
            },
        ]);
        config.mapUnknownRoutes(instruction => "welcome");
        
        this.router = router;
    }    
}
