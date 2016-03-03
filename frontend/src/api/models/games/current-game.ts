"use strict";

import {Team} from "api/models/games/team";

export class CurrentGame {
    public gameId: number;
    public region: string;
    public gameMode: string;
    public gameQueueType: string;
    public gameType: string;
    public mapType: string;
    public gameStartTime: Date;
    public blueTeam: Team;
    public purpleTeam: Team;
}
