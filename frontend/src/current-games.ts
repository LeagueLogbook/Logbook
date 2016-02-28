"use strict";

export class CurrentGames {
    private _timeout: number;
    
    public activate(): void {
        this._timeout = setTimeout(() => this.onTick(), 10 * 1000);
    }
    public deactivate(): void {
        clearTimeout(this._timeout);
    }
    
    private onTick(): void {
        alert("Hoi");
    }
}
