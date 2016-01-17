"use strict";

import {autoinject} from "aurelia-framework";
import {SummonersService} from "services/summoners-service";
import {Summoner} from "api/models/summoner";
import {Region} from "api/models/region";

@autoinject()
export class SummonerOverview {
    
    public summoners: Summoner[];
    public regions: RegionValue[];
    public selectedRegion: Region;
    
    public constructor(private summonersService: SummonersService) {
        this.regions = [
            {
                region: Region.euw,
                name: "EUW",
            },
            {
                region: Region.na,
                name: "NA",
            },
        ];
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
        alert(this.selectedRegion);
    }
}

class RegionValue {
    public region: Region;
    public name: string;
}
