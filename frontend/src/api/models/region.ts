"use strict";

import * as _ from "lodash";

export class Region {
    
    public id: string;
    public shortName: string;
    public name: string;
    
    public static getById(id: string) : Region {
        return _.head(this.getAll().filter(f => f.id === id.toLocaleLowerCase()));
    }
    
    public static getByShortName(shortName: string) {
        return _.head(this.getAll().filter(f => f.shortName.toLocaleLowerCase() === shortName.toLocaleLowerCase()));
    }
    
    public static get BR(): Region {
        return {
            id: "br",
            shortName: "BR",
            name: "Brazil",
        };
    } 
    public static get EUNE(): Region {
        return {
            id: "eune",
            shortName: "EUNE",
            name: "Europe Nordic & East",    
        };
    }
    public static get EUW(): Region {
        return {
            id: "euw",
            shortName: "EUW",    
            name: "Europe West",
        };
    }
    public static get NA(): Region {
        return {
            id: "na",
            shortName: "NA",  
            name: "North America",  
        };
    }
    public static get KR(): Region {
        return {
            id: "kr",
            shortName: "KR",
            name: "Korea",    
        };
    }
    public static get LAN(): Region {
        return {
            id: "lan",
            shortName: "LAN",
            name: "Latin America North",    
        };
    }
    public static get LAS(): Region {
        return {
            id: "las",
            shortName: "LAS",
            name: "Latin America South",    
        };
    }
    public static get OCE(): Region {
        return {
            id: "oce",
            shortName: "OCE",
            name: "Oceania",    
        };
    }
    public static get RU(): Region {
        return {
            id: "ru",
            shortName: "RU",
            name: "Russia",    
        };
    }
    public static get TR(): Region {
        return {
            id: "tr",
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
