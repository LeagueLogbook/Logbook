"use strict";

import {autoinject} from "aurelia-framework";
import {LogbookApi} from "api/logbook-api";
import {AuthService} from "services/auth-service";
import {SummonerCurrentGame} from "api/models/games/summoner-current-game";
import {Timer} from "helper/timer";
import {Command} from "helper/command";

@autoinject()
export class CurrentGames {
    
    public currentGames: SummonerCurrentGame[];
    
    public refreshCommand: Command;
    
    private _timer: Timer;
    
    public constructor(private logbookApi: LogbookApi, private authService: AuthService) {
        this._timer = new Timer(10 * 1000, () => this.refreshCommand.execute());        
        this.refreshCommand = new Command(() => this.refresh());
    }
    
    public activate(): void {
        this._timer.start();
    }
    public deactivate(): void {
        this._timer.stop();
    }
        
    private async refresh(): Promise<void> {
        let games = await this.logbookApi.currentGamesApi.getCurrentGames(this.authService.token);
        this.currentGames = games;
    }
}
