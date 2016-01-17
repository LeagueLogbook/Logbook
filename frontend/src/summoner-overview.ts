"use strict";

import {autoinject} from "aurelia-framework";
import {SummonersService} from "services/summoners-service";
import {Summoner} from "api/models/summoner";
import {Region, RegionHelper} from "api/models/region";

@autoinject()
export class SummonerOverview {
    
    public summoners: Summoner[];
    public regions: RegionHelper[];
    public selectedRegion: RegionHelper;
    
    public constructor(private summonersService: SummonersService) {
        this.regions = RegionHelper.getAll();
        this.selectedRegion = this.regions[0];
    }
    
    public async activate() : Promise<void> {
        let summoner = new Summoner();
        summoner.id = 123;
        summoner.name = "haefele";
        summoner.region = Region.euw;
        
        let summoners = await this.summonersService.getSummoners();
        this.summoners = summoners.concat(summoner);
    }
    
    public test(): void {
        alert(this.selectedRegion.region);
    }
}
