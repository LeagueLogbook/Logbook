"use strict";

export class OAuthHelper {
        
    public showOAuthPopup(name: string, uri: string, redirectUri: string) : Promise<string> {
        let popupWindow : Window = window.open(uri, name, "width:500,height=500");
        
        return new Promise((resolve, reject) => {
            let polling = setInterval(() => {
                try {                   
                    if (popupWindow.location.href.indexOf(redirectUri) === 0) {                       
                        clearInterval(polling);
                        popupWindow.close();
                        
                        resolve(popupWindow.location.href);
                    }
                } 
                catch (e) { 
                    // ignore errors
                }
            }, 35);
        });
    }    
}
