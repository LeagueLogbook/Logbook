"use strict";

import {BannedChampion} from "api/models/banned-champion";
import {Participant} from "api/models/participant";

export class Team {
    public bannedChampions: BannedChampion[];
    public participants: Participant[];
}
