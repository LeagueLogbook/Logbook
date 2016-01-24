"use strict";

import {Region} from "api/models/region";
import {bindable} from "aurelia-framework";

export class AddSummoner {   
    
    public regions: Region[];
    public selectedRegion: Region;
    public summonerName: string;    
    @bindable
    public onAddSummoner: (data: IAddSummonerData) => Promise<void>;
    
    public constructor() {
        this.regions = Region.getAll();
        this.selectedRegion = this.regions[0];
        this.summonerName = "";
    }
    
    public updateSelectedRegion(region: Region): void {
        this.selectedRegion = region;
    }
    
    public async addSummoner(): Promise<void> {
        let data : IAddSummonerData = {
            summonerName: this.summonerName,
            region: this.selectedRegion,  
        };
        
        if (this.onAddSummoner) {
            await this.onAddSummoner(data);            
        }
        
        this.summonerName = "";
    }
}

export interface IAddSummonerData {
    summonerName: string;
    region: Region;
}
