"use strict";

import * as _ from "lodash";

export class Region {
    
    public shortName: string;
    public name: string;
    
    public static get(shortName: string) : Region {
        return _.head(this.getAll().filter(f => f.shortName.toLowerCase() === shortName.toLowerCase()));
    }
    
    public static get BR(): Region {
        return {
            shortName: "BR",
            name: "Brazil",
        };
    } 
    public static get EUNE(): Region {
        return {
            shortName: "EUNE",
            name: "Europe Nordic & East",    
        };
    }
    public static get EUW(): Region {
        return {
            shortName: "EUW",    
            name: "Europe West",
        };
    }
    public static get NA(): Region {
        return {
            shortName: "NA",  
            name: "North America",  
        };
    }
    public static get KR(): Region {
        return {
            shortName: "KR",
            name: "Korea",    
        };
    }
    public static get LAN(): Region {
        return {
            shortName: "LAN",
            name: "Latin America North",    
        };
    }
    public static get LAS(): Region {
        return {
            shortName: "LAS",
            name: "Latin America South",    
        };
    }
    public static get OCE(): Region {
        return {
            shortName: "OCE",
            name: "Oceania",    
        };
    }
    public static get RU(): Region {
        return {
            shortName: "RU",
            name: "Russia",    
        };
    }
    public static get TR(): Region {
        return {
            shortName: "TR",
            name: "Turkey",    
        };
    }
    
    public static getAll() : Region[] {
        return [
            this.BR, 
            this.EUNE,
            this.EUW,
            this.NA,
            this.KR,
            this.LAN,
            this.LAS,
            this.OCE,
            this.RU,
            this.TR,
        ];
    }
}
