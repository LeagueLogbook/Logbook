"use strict";

export class Timer {
    private _timeout: number;    
    
    public constructor(private interval: number, private tick: () => Promise<void>) {        
    }    
    
    public start(): void {
        this._timeout = setTimeout(async () => {
            try {
                this.stop();
                await this.tick();            
            }
            finally {
                this.start();
            }
        }, this.interval);
    }
    
    public stop(): void {
        clearTimeout(this._timeout);
    }
}
