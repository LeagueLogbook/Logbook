"use strict";

import {autoinject} from "aurelia-framework";
import {HttpClient} from "aurelia-http-client";
import {SummonerCurrentGame} from "api/models/games/summoner-current-game";
import config from "config";
import {JsonWebTokenModel} from "api/models/authentication/json-web-token-model";

@autoinject()
export class CurrentGamesApi {
    public constructor(public httpClient: HttpClient) {
        this.httpClient.configure(f => f
            .withBaseUrl(config.webServiceUri)
            .withHeader("Content-Type", "application/json"));
    }
    
    public getCurrentGames(token: JsonWebTokenModel): Promise<SummonerCurrentGame[]> {
        return this.httpClient
            .createRequest("CurrentGames")
            .withHeader("Authorization", `Bearer ${token.token}`)
            .asGet()
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));            
    }
}
