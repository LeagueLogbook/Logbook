import "semantic-ui";
import {Aurelia} from "aurelia-framework";
import {AuthService} from "services/auth-service";

export function configure(aurelia: Aurelia) {
    "use strict";

    aurelia.use
        .standardConfiguration()
        .developmentLogging();
        
    aurelia.start().then(a =>  {        
        let authService: AuthService = a.container.get(AuthService);
        
        let root = authService.isLoggedIn
            ? "app-logged-in"
            : "app-logged-out";
            
        a.setRoot(root); 
    });
}
