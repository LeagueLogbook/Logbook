"use strict";

export class Command {
    
    private _isExecuting: boolean;
    private _error: string;
    
    public get error(): string {
        return this._error;
    }    
    
    public get isExecuting(): boolean {
        return this._isExecuting;
    }
    
    public get canExecute(): boolean {
        if (this.onCanExecute) {
            return this.onCanExecute();
        }
        
        return true;
    }
    
    public constructor(private onExecute: () => Promise<void>, private onCanExecute?: () => boolean) {        
    }
    
    public async execute(): Promise<void> {        
        if (this.isExecuting === true) {
            return;
        }
        
        if (this.canExecute === false) {
            return;            
        }
        
        try {
            this._isExecuting = true;
            this._error = null;
            
            await this.onExecute();
        }
        catch (error) {
            this._error = error;
        }
        finally {
            this._isExecuting = false;
        }
    }
}
