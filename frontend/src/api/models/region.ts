"use strict";

export enum Region {
    br,
    eune,
    euw,
    na,
    kr,
    lan,
    las,
    oce,
    ru,
    tr
}

export class RegionHelper {
    
    public region: Region;
    public name: string;
    
    public static getAll() : RegionHelper[] {
        return [
            {
                region: Region.br,
                name: "BR",    
            },
            {
                region: Region.eune,
                name: "EUNE",    
            },
            {
                region: Region.euw,
                name: "EUW",    
            },
            {
                region: Region.na,
                name: "NA",    
            },
            {
                region: Region.kr,
                name: "KR",    
            },
            {
                region: Region.lan,
                name: "LAN",    
            },
            {
                region: Region.las,
                name: "LAS",    
            },
            {
                region: Region.oce,
                name: "OCE",    
            },
            {
                region: Region.ru,
                name: "RU",    
            },
            {
                region: Region.tr,
                name: "TR",    
            },
        ];
    }
}
