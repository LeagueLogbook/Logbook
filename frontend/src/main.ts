import 'bootstrap';
import {Aurelia} from 'aurelia-framework';
import {AuthService} from 'services/auth-service';

export function configure(aurelia: Aurelia) {
    aurelia.use
        .standardConfiguration()
        .developmentLogging();

  //Uncomment the line below to enable animation.
  //aurelia.use.plugin('aurelia-animator-css');

  //Anyone wanting to use HTMLImports to load views, will need to install the following plugin.
  //aurelia.use.plugin('aurelia-html-import-template-loader')

    aurelia.start().then(a =>  {
        let authService :AuthService = a.container.get(AuthService);
        
        var root = authService.isLoggedIn
            ? "app"
            : "loggedOut";
            
        a.setRoot(root); 
    });
}
