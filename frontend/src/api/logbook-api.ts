"use strict";

import {autoinject} from "aurelia-framework";
import {AuthenticationApi} from "api/authentication-api";
import {SummonersApi} from "api/summoners-api";

@autoinject()
export class LogbookApi {
    
    public constructor(public authenticationApi: AuthenticationApi, public summonersApi: SummonersApi) {
                
    }
}
