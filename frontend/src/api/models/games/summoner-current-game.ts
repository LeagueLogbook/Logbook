"use strict";

import {Summoner} from "api/models/summoners/summoner";
import {CurrentGame} from "api/models/games/current-game";

export class SummonerCurrentGame {
    public summoner: Summoner;
    public game: CurrentGame;
}
