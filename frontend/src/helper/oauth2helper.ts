import {UrlHelper} from 'helper/url-helper';
import {autoinject} from 'aurelia-framework';

@autoinject()
export class OAuth2Helper {
    
    constructor(public urlHelper: UrlHelper) {
    }
    
    showOAuth2Popup(name: string, uri: string, redirectUri: string) : Promise<string> {
        let popupWindow : Window = window.open(uri, name, "width:500,height=500");
        
        return new Promise((resolve, reject) => {
            var polling = setInterval(() => {
               try {                   
                   if (popupWindow.location.href.indexOf(redirectUri) === 0) {
                       var code = this.urlHelper.getParameter(popupWindow.location.href, 'code');
                       
                       clearInterval(polling);
                       popupWindow.close();
                       
                       resolve(code);
                   }
               } 
               catch (e) { }
            }, 35);
        });
    }    
}