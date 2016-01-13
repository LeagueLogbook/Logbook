"use strict";

export class UrlHelper {
    getParameter(uri: string, name: string) : string {
        name = name
            .replace(/[\[]/, "\\[")
            .replace(/[\]]/, "\\]");

        let regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
        let results = regex.exec(uri);

        return results === null 
        ? null
        : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}