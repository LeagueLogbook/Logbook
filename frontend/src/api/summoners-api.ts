"use strict";

import {Summoner} from "api/models/summoners/summoner";
import {autoinject} from "aurelia-framework";
import {HttpClient} from "aurelia-http-client";
import config from "config";
import {JsonWebTokenModel} from "api/models/authentication/json-web-token-model";

@autoinject()
export class SummonersApi {
    public constructor(private httpClient: HttpClient) {
        this.httpClient.configure(f => f
            .withBaseUrl(config.webServiceUri)
            .withHeader("Content-Type", "application/json"));
    }
    
    public getSummoners(token: JsonWebTokenModel): Promise<Summoner[]> {
        return this.httpClient
            .createRequest("Summoners")
            .asGet()
            .withHeader("Authorization", `Bearer ${token.token}`)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public addSummoner(region: string, name: string, token: JsonWebTokenModel): Promise<Summoner[]> {
        let body = {
            region: region,
            summonerName: name,
        };
        
        return this.httpClient
            .createRequest("Summoners")
            .asPatch()
            .withHeader("Authorization", `Bearer ${token.token}`)
            .withContent(body)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
    
    public deleteSummoner(region: string, summonerId: number, token: JsonWebTokenModel): Promise<Summoner[]> {
        let body = {
            region: region,
            summonerId: summonerId,
        };
        
        return this.httpClient
            .createRequest("Summoners")
            .asDelete()
            .withHeader("Authorization", `Bearer ${token.token}`)
            .withContent(body)
            .send()
            .then(response => response.content)
            .catch(response => Promise.reject(response.content.message));
    }
}
