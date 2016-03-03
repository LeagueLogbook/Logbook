"use strict";

import * as _ from "lodash";

export class GameQueueType {
    public id: string;
    public name: string;
    
    public static getById(id: string): GameQueueType {
        return _.head(this.getAll().filter(f => f.id.toLocaleLowerCase() === id.toLocaleLowerCase()));
    }
    
    public static get custom(): GameQueueType {
        return {
            id: "custom",
            name: "Custom",
        };
    }
    
    public static get normal5x5Blind(): GameQueueType {
        return {
            id: "normal5x5Blind",
            name: "Normal 5v5 Blind Pick",
        };
    }
    
    public static get rankedSolo5x5(): GameQueueType {
        return {
            id: "rankedSolo5x5",
            name: "Ranked Solo 5v5",
        };
    }
    
    public static get rankedPremade5x5(): GameQueueType {
        return {
            id: "rankedPremade5x5",
            name: "Ranked Premade 5v5",
        };
    }
    
    public static get bot5x5(): GameQueueType {
        return {
            id: "bot5x5",
            name: "Historical Summoner's Rift Coop vs AI",
        };
    }
    
    public static get normal3x3(): GameQueueType {
        return {
            id: "normal3x3",
            name: "Normal 3v3",
        };
    }
    
    public static get rankedPremade3x3(): GameQueueType {
        return {
            id: "rankedPremade3x3",
            name: "Ranked Premade 3v3",
        };
    }
    
    public static get normal5x5Draft(): GameQueueType {
        return {
            id: "normal5x5Draft",
            name: "Normal 5v5 Draft Pick",
        };
    }
    
    public static get odin5x5Blind(): GameQueueType {
        return {
            id: "odin5x5Blind",
            name: "Dominion 5v5 Blind Pick",
        };
    }
    
    public static get odin5x5Draft(): GameQueueType {
        return {
            id: "odin5x5Draft",
            name: "Dominion 5v5 Draft Pick",
        };
    }
    
    public static get botOdin5x5(): GameQueueType {
        return {
            id: "botOdin5x5",
            name: "Dominion Coop vs AI",
        };
    }
    
    public static get bot5x5Intro(): GameQueueType {
        return {
            id: "bot5x5Intro",
            name: "Summoner's Rift Coop vs AI Intro Bot",
        };
    }
    
    public static get bot5x5Beginner(): GameQueueType {
        return {
            id: "bot5x5Beginner",
            name: "Summoner's Rift Coop vs AI Beginner Bot",
        };
    }
    
    public static get bot5x5Intermediate(): GameQueueType {
        return {
            id: "bot5x5Intermediate",
            name: "Historical Summoner's Rift Coop vs AI Intermediate Bot",
        };
    }
    
    public static get rankedTeam3x3(): GameQueueType {
        return {
            id: "rankedTeam3x3",
            name: "Ranked Team 3v3",
        };
    }
    
    public static get rankedTeam5x5(): GameQueueType {
        return {
            id: "rankedTeam5x5",
            name: "Ranked Team 5v5",
        };
    }
    
    public static get botTt3x3(): GameQueueType {
        return {
            id: "botTt3x3",
            name: "Twisted Treeline Coop vs AI",
        };
    }
    
    public static get groupFinder5x5(): GameQueueType {
        return {
            id: "groupFinder5x5",
            name: "Team Builder",
        };
    }
    
    public static get aram5x5(): GameQueueType {
        return {
            id: "aram5x5",
            name: "ARAM",
        };
    }
    
    public static get oneforall5x5(): GameQueueType {
        return {
            id: "oneforall5x5",
            name: "One for All",
        };
    }
    
    public static get firstblood1x1(): GameQueueType {
        return {
            id: "firstblood1x1",
            name: "Snowdown Showdown 1v1",
        };
    }
    
    public static get firstblood2x2(): GameQueueType {
        return {
            id: "firstblood2x2",
            name: "Snowdown Showdown 2v2",
        };
    }
    
    public static get sr6x6(): GameQueueType {
        return {
            id: "sr6x6",
            name: "Summoner's Rift 6x6 Hexakill",
        };
    }
    
    public static get urf5x5(): GameQueueType {
        return {
            id: "urf5x5",
            name: "Ultra Rapid Fire",
        };
    }
    
    public static get botUrf5x5(): GameQueueType {
        return {
            id: "botUrf5x5",
            name: "Ultra Rapid Fire games played against AI",
        };
    }
    
    public static get nightmareBot5x5Rank1(): GameQueueType {
        return {
            id: "nightmareBot5x5Rank1",
            name: "Doom Bots Rank 1",
        };
    }
    
    public static get nightmareBot5x5Rank2(): GameQueueType {
        return {
            id: "nightmareBot5x5Rank2",
            name: "Doom Bots Rank 2",
        };
    }
    
    public static get nightmareBot5x5Rank5(): GameQueueType {
        return {
            id: "nightmareBot5x5Rank5",
            name: "Doom Bots Rank 5",
        };
    }
    
    public static get ascension5x5(): GameQueueType {
        return {
            id: "ascension5x5",
            name: "Ascension",
        };
    }
    
    public static get hexakill(): GameQueueType {
        return {
            id: "hexakill",
            name: "Twisted Treeline 6x6 Hexakill",
        };
    }
    
    public static get bilgewaterAram5x5(): GameQueueType {
        return {
            id: "bilgewaterAram5x5",
            name: "Butcher's Bridge",
        };
    }
    
    public static get kingPoro5x5(): GameQueueType {
        return {
            id: "kingPoro5x5",
            name: "King Poro",
        };
    }
    
    public static get bilgewater5x5(): GameQueueType {
        return {
            id: "bilgewater5x5",
            name: "Black Market Brawlers",
        };
    }
    
    public static get teamBuilderDraftUnranked(): GameQueueType {
        return {
            id: "teamBuilderDraftUnranked",
            name: "Normal 5v5 Draft Pick",
        };
    }
    
    public static get teamBuilderDraftRanked(): GameQueueType {
        return {
            id: "teamBuilderDraftRanked",
            name: "Ranked 5v5 Draft Pick",
        };
    }
    
    public static getAll(): GameQueueType[]{
        return [
            this.custom,
            this.normal5x5Blind,
            this.rankedSolo5x5,
            this.rankedPremade5x5,
            this.bot5x5,
            this.normal3x3,
            this.rankedPremade3x3,
            this.normal5x5Draft,
            this.odin5x5Blind,
            this.odin5x5Draft,
            this.botOdin5x5,
            this.bot5x5Intro,
            this.bot5x5Beginner,
            this.bot5x5Intermediate,
            this.rankedTeam3x3,
            this.rankedTeam5x5,
            this.botTt3x3,
            this.groupFinder5x5,
            this.aram5x5,
            this.oneforall5x5,
            this.firstblood1x1,
            this.firstblood2x2,
            this.sr6x6,
            this.urf5x5,
            this.botUrf5x5,
            this.nightmareBot5x5Rank1,
            this.nightmareBot5x5Rank2,
            this.nightmareBot5x5Rank5,
            this.ascension5x5,
            this.hexakill,
            this.bilgewaterAram5x5,
            this.kingPoro5x5,
            this.bilgewater5x5,
            this.teamBuilderDraftUnranked,
            this.teamBuilderDraftUnranked,
        ];
    }
}
