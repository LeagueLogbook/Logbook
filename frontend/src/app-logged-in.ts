"use strict";

import {Router, RouterConfiguration} from "aurelia-router";
import {autoinject} from "aurelia-framework";

@autoinject()
export class AppLoggedIn {
    
    public router: Router;
            
    public configureRouter(config: RouterConfiguration, router: Router) : void {
        config.title = "Aurelia";
        config.map([
            {
                route: ["", "current-games"],
                name: "current-games",
                moduleId: "current-games",
                nav: true,
                title: "Current games",
            },
            { 
                route: "summoners", 
                name: "summoners",      
                moduleId: "summoner-overview",      
                nav: true, 
                title: "Summoners",
            },
            { 
                route: "logout", 
                name: "logout",      
                moduleId: "logout",      
                nav: false, 
                title: "Logout",
            },
        ]);
        config.mapUnknownRoutes(instruction => "summoner-overview");

        this.router = router;
    }
}
