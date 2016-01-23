"use strict";

import * as _ from "lodash";

export class Region {
    
    public shortName: string;
    public name: string;
    
    public static get(shortName: string) : Region {
        return _.head(this.getAll().filter(f => f.shortName.toLowerCase() === shortName.toLowerCase()));
    }
    
    public static getAll() : Region[] {
        return [
            {
                shortName: "BR",
                name: "Brazil",
            },
            {
                shortName: "EUNE",
                name: "Europe Nordic & East",    
            },
            {
                shortName: "EUW",    
                name: "Europe West",
            },
            {
                shortName: "NA",  
                name: "North America",  
            },
            {
                shortName: "KR",
                name: "Korea",    
            },
            {
                shortName: "LAN",
                name: "Latin America North",    
            },
            {
                shortName: "LAS",
                name: "Latin America South",    
            },
            {
                shortName: "OCE",
                name: "Oceania",    
            },
            {
                shortName: "RU",
                name: "Russia",    
            },
            {
                shortName: "TR",
                name: "Turkey",    
            },
        ];
    }
}
