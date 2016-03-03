"use strict";

import {autoinject} from "aurelia-framework";
import {SummonersService} from "services/summoners-service";
import {Summoner} from "api/models/summoners/summoner";
import {IAddSummonerData} from "components/add-summoner";
import {Region} from "api/entities/summoners/region";

@autoinject()
export class SummonerOverview {
    
    public summoners: Summoner[];
    
    public constructor(private summonersService: SummonersService) {
    }
    
    public async activate(): Promise<void> {
        this.summoners = await this.summonersService.getSummoners();
    }
    
    public async addSummoner(data: IAddSummonerData) : Promise<void> {
        this.summoners = await this.summonersService.addSummoner(data.region, data.summonerName);
    }
    
    public async deleteSummoner(summoner: Summoner): Promise<void> {
        let region = Region.getById(summoner.region);        
        this.summoners = await this.summonersService.deleteSummoner(region, summoner.id);
    }
}
