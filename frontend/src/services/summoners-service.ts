"use strict";

import {autoinject} from "aurelia-framework";
import {LogbookApi} from "api/logbook-api";
import {AuthService} from "services/auth-service";
import {Summoner} from "api/models/summoners/summoner";
import {Region} from "api/entities/summoners/region";

@autoinject()
export class SummonersService {
    public constructor(private logbookApi: LogbookApi, private authService: AuthService) {
    }
    
    public getSummoners() : Promise<Summoner[]> {
        return this.logbookApi.summonersApi.getSummoners(this.authService.token);
    }
    
    public addSummoner(region: Region, name: string) : Promise<Summoner[]> {
        return this.logbookApi.summonersApi.addSummoner(region.shortName, name, this.authService.token);
    }
    
    public deleteSummoner(region: Region, summonerId: number) : Promise<Summoner[]> {
        return this.logbookApi.summonersApi.deleteSummoner(region.shortName, summonerId, this.authService.token);
    }
}
