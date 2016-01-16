"use strict";

import {Router, RouterConfiguration} from "aurelia-router";

export class AppLoggedOut {
    
    public router: Router;
    
    public attached() {        
        this.router.navigateToRoute("welcome");
    }
    
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
        
        this.router = router;
        this.router.navigate("welcome");
    }    
}
