"use strict";

import {autoinject} from "aurelia-framework";
import {AuthenticationApi} from "./authentication-api";

@autoinject()
export class LogbookApi {
    
    public constructor(public authenticationApi: AuthenticationApi) {
                
    }
}
