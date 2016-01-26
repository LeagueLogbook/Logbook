"use strict";

import {Region} from "api/models/region";
import {bindable} from "aurelia-framework";

export class AddSummoner {   
    
    public regions: Region[];
    public selectedRegionShortName: string;
    
    public summonerName: string;    
    @bindable
    public onAddSummoner: (data: IAddSummonerData) => Promise<void>;
    
    public constructor() {
        this.regions = Region.getAll();
        this.selectedRegionShortName = this.regions[0].shortName;
        this.summonerName = "";
    }
    
    public test(): void {
        alert(this.selectedRegionShortName);
    }
        
    public async addSummoner(): Promise<void> {
        let data : IAddSummonerData = {
            summonerName: this.summonerName,
            region: Region.get(this.selectedRegionShortName),  
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
