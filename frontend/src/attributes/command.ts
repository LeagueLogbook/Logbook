"use strict";

import {Command} from "helper/command";
import {autoinject, ObserverLocator} from "aurelia-framework";

@autoinject()
export class ButtonCommandCustomAttribute {
        
    private command: Command;
                
    public constructor(private element: Element, private observerLocator: ObserverLocator) {
    }
    
    public attached(): void {
        this.element.addEventListener("click", this.onButtonClick.bind(this));
    }
    
    public detached(): void {
        this.element.removeEventListener("click", this.onButtonClick.bind(this));
    }
    
    public valueChanged(newValue: Command, oldValue: Command): void {
        this.command = newValue;
        
        if (oldValue) {
            this.observerLocator
                .getObserver(oldValue, "isExecuting")
                .unsubscribe(this.onCommandIsExecutingChanged.bind(this));         
                
            this.observerLocator
                .getObserver(oldValue, "canExecute")
                .unsubscribe(this.onCommandCanExecuteChanged.bind(this));
        }
        
        if (newValue) {
            this.observerLocator
                .getObserver(newValue, "isExecuting")
                .subscribe(this.onCommandIsExecutingChanged.bind(this));
                
            this.observerLocator
                .getObserver(newValue, "canExecute")
                .subscribe(this.onCommandCanExecuteChanged.bind(this));
                
            this.onCommandIsExecutingChanged(this.command.isExecuting, undefined);
            this.onCommandCanExecuteChanged(this.command.canExecute, undefined);                
        }        
    }
    
    private onCommandIsExecutingChanged(newValue: boolean, oldValue: boolean): void {
        if (newValue === true) {
            this.element.classList.add("loading");
            this.element.classList.add("disabled");
        }
        if (newValue === false) {
            this.element.classList.remove("loading");
            this.element.classList.remove("disabled");
        }
    }
    private onCommandCanExecuteChanged(newValue: boolean, oldValue: boolean): void {
        if (newValue === true) {
            this.element.classList.remove("disabled");
        }
        if (newValue === false) {
            this.element.classList.add("disabled");
        }
    }
    private async onButtonClick(): Promise<void> {
        if (this.command !== null) {
            await this.command.execute();
        }
    }
}
