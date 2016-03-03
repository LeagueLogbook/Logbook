"use strict";

import {BannedChampion} from "api/models/games/banned-champion";
import {Participant} from "api/models/games/participant";

export class Team {
    public bannedChampions: BannedChampion[];
    public participants: Participant[];
}
