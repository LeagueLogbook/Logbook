"use strict";

import {Summoner} from "api/models/summoner";
import {Region} from "api/models/region";
import {autoinject} from "aurelia-framework";
import {HttpClient} from "aurelia-http-client";
import config from "config";
import {JsonWebToken} from "api/models/json-web-token";

@autoinject()
export class SummonersApi {
    public constructor(private httpClient: HttpClient) {
        this.httpClient.configure(f => f
            .withBaseUrl(config.webServiceUri)
            .withHeader("Content-Type", "application/json"));
    }
    
    public getSummoners(token: JsonWebToken) : Promise<Summoner[]> {
        return this.httpClient
            .createRequest("Summoners")
            .asGet()
            .withHeader("Authorization", `Bearer ${token.token}`)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public addSummoner(region: Region, name: string, token: JsonWebToken) : Promise<Summoner[]> {
        return this.httpClient
            .createRequest("Summoners")
            .asPatch()
            .withHeader("Authorization", `Bearer ${token.token}`)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public deleteSummoner(region: Region, summonerId: number, token: JsonWebToken): Promise<Summoner[]> {
        return this.httpClient
            .createRequest("Summoners")
            .asDelete()
            .withHeader("Authorization", `Bearer ${token.token}`)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
}
