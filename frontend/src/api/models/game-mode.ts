"use strict";

import * as _ from "lodash";

export class GameMode {
    public id: string;
    public name: string;
    
    public static getById(id: string): GameMode {
        return _.head(this.getAll().filter(f => f.id.toLocaleLowerCase() === id.toLocaleLowerCase()));
    }
    
    public static get classic(): GameMode {
        return {
            id: "classic",
            name: "Classic Summoner's Rift and Twisted Treeline",  
        };
    }
    
    public static get dominion(): GameMode {
        return {
            id: "dominion",
            name: "Dominion",  
        };
    }
    
    public static get aram(): GameMode {
        return {
            id: "aram",
            name: "ARAM",  
        };
    }
    
    public static get tutorial(): GameMode {
        return {
            id: "tutorial",
            name: "Tutorial",  
        };
    }
    
    public static get oneForAll(): GameMode {
        return {
            id: "oneForAll",
            name: "One for All",  
        };
    }
    
    public static get firstBlood(): GameMode {
        return {
            id: "firstBlood",
            name: "Snowdown Showdown",  
        };
    }
    
    public static get ascension(): GameMode {
        return {
            id: "ascension",
            name: "Ascension",  
        };
    }
    
    public static get intro(): GameMode {
        return {
            id: "intro",
            name: "Intro",  
        };
    }
    
    public static get kingPoro(): GameMode {
        return {
            id: "kingPoro",
            name: "King Poro",  
        };
    }
    
    public static getAll(): GameMode[] {
        return [
            this.classic,
            this.dominion,
            this.aram,
            this.tutorial,
            this.oneForAll,
            this.firstBlood,
            this.ascension,
            this.intro,
            this.kingPoro,
        ];
    }
}
