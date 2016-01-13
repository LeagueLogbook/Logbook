"use strict";

import {Router, RouterConfiguration} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';

@autoinject()
export class App {
  router: Router;
  
  constructor() {
  }
  
  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'Aurelia';
    config.map([
        { 
            route: ['', 'welcome'], 
            name: 'welcome',      
            moduleId: 'welcome',      
            nav: true, 
            title: 'Welcome' 
        }
    ]);

    this.router = router;
  }
}
