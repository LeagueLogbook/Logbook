"use strict";

import * as _ from "lodash";

export class MapType {
    public id: string;
    public name: string;
    
    public static getById(id: string): MapType {
        return _.head(this.getAll().filter(f => f.id.toLocaleLowerCase() === id.toLocaleLowerCase()));
    }
        
    public static get summonersRiftSummerVariant(): MapType {
        return {
            id: "summonersRiftSummerVariant",
            name: "Summoner's Rift Original Summer",
        };
    }
    
    public static get summonersRiftAutumnVariant(): MapType {
        return {
            id: "summonersRiftAutumnVariant",
            name: "Summoner's Rift Original Autumn",
        };
    }
    
    public static get theProvingGrounds(): MapType {
        return {
            id: "theProvingGrounds",
            name: "Tutorial Map",
        };
    }
    
    public static get twistedTreelineOriginal(): MapType {
        return {
            id: "twistedTreelineOriginal",
            name: "Twisted Treeline Original",
        };
    }
    
    public static get theCrystalScar(): MapType {
        return {
            id: "theCrystalScar",
            name: "The Crystal Scar",
        };
    }
    
    public static get twistedTreelineCurrent(): MapType {
        return {
            id: "twistedTreelineCurrent",
            name: "Twisted Treeline",
        };
    }  
    
    public static get summonersRift(): MapType {
        return {
            id: "summonersRift",
            name: "Summoner's Rift",
        };
    } 
    
    public static get howlingAbyss(): MapType {
        return {
            id: "howlingAbyss",
            name: "Howling Abyss",
        };
    }
    
    public static getAll(): MapType[]{
        return [
            this.summonersRiftSummerVariant,
            this.summonersRiftAutumnVariant,
            this.theProvingGrounds,
            this.twistedTreelineOriginal,
            this.theCrystalScar,
            this.twistedTreelineCurrent,
            this.summonersRift,
            this.howlingAbyss,  
        ];
    } 
}
