"use strict";

import {autoinject} from "aurelia-framework";
import {AuthenticationApi} from "api/authentication-api";
import {SummonersApi} from "api/summoners-api";
import {CurrentGamesApi} from "api/current-games-api";

@autoinject()
export class LogbookApi {
    
    public constructor(public authenticationApi: AuthenticationApi, public summonersApi: SummonersApi, public currentGamesApi: CurrentGamesApi) {
                
    }
}
