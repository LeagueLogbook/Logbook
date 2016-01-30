"use strict";

import {Command} from "helper/command";
import {autoinject, bindable, ObserverLocator} from "aurelia-framework";

@autoinject()
export class CommandError {
        
    public isVisible: boolean;
    public error: string;
        
    @bindable 
    public command: Command;
    @bindable
    public hideAfter: number;
    @bindable
    public marginTop: boolean;
    
    public constructor(private observerLocator: ObserverLocator) {
        this.hideAfter = 3000;
        this.marginTop = false;
    }
    
    public commandChanged(newValue: Command, oldValue: Command) {
        
        if (oldValue) {
            this.observerLocator
                .getObserver(oldValue, "error")
                .unsubscribe(this.onCommandErrorChanged.bind(this));    
        }                
        
        if (newValue) {
            this.observerLocator
                .getObserver(newValue, "error")
                .subscribe(this.onCommandErrorChanged.bind(this));
        }
    }
        
    public onCommandErrorChanged(newValue: string, oldValue: string) {
        this.error = newValue;
        
        if (this.error !== null && this.error !== "") {
            this.isVisible = true;
            
            let timeout = setTimeout(() => {                
                this.error = "";
                this.isVisible = false;
                
                clearTimeout(timeout);
            }, this.hideAfter);
        }
    }
    
    public marginTopChanged(newValue: Object, oldValue: Object) {
        if (newValue === "false") {
            this.marginTop = false;
        }
        if (newValue === "true") {
            this.marginTop = true;
        }
    }
}
