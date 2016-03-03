"use strict";

import * as _ from "lodash";

export class GameType {
    public id: string;
    public name: string;
    
    public static getById(id: string): GameType {
        return _.head(this.getAll().filter(f => f.id.toLocaleLowerCase() === id.toLocaleLowerCase()));
    }
    
    public static get custom(): GameType {
        return {
            id: "custom",
            name: "Custom",
        };
    }
    
    public static get tutorial(): GameType {
        return {
            id: "tutorial",
            name: "Tutorial",  
        };
    }
    
    public static get matched(): GameType {
        return {
            id: "matched",
            name: "Matched",  
        };
    }
    
    public static getAll(): GameType[] {
        return [
            this.custom,
            this.tutorial,
            this.matched,
        ];
    }
}
