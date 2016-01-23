"use strict";

import {Region, RegionHelper} from "api/models/region";
import {bindable} from "aurelia-framework";

export class AddSummoner {   
    
    public regions: RegionHelper[];
    public selectedRegion: RegionHelper;
    public summonerName: string;    
    @bindable
    public onAddSummoner: (data: IAddSummonerData) => void;
    
    public constructor() {
        this.regions = RegionHelper.getAll();
        this.selectedRegion = this.regions[0];
        this.summonerName = "";
    }
    
    public updateSelectedRegion(region: RegionHelper): void {
        this.selectedRegion = region;
    }
    
    public addSummoner(): void {
        let data : IAddSummonerData = {
            summonerName: this.summonerName,
            region: this.selectedRegion.region,  
        };
        
        if (this.onAddSummoner) {
            this.onAddSummoner(data);            
        }
    }
}

export interface IAddSummonerData {
    summonerName: string;
    region: Region;
}
