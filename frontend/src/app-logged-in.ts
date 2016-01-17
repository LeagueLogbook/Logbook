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
                route: ["", "summonerOverview"], 
                name: "summonerOverview",      
                moduleId: "summoner-overview",      
                nav: true, 
                title: "Overview",
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
