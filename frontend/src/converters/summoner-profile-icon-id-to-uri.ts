"use strict";

export class SummonerProfileIconIdToUriValueConverter {
    public toView(profileIconId: number) : string {
        return `http://cdn.lolstats.org/img/profileicon/${profileIconId}.png`;
    }    
}
