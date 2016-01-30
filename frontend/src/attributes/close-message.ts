"use strict";

import * as $ from "jquery";
import {autoinject} from "aurelia-framework";

@autoinject()
export class CloseMessageCustomAttribute {
    
    public constructor(private element: Element) {
    }
    
    
    public attached(): void {
        this.element.addEventListener("click", this.onButtonClick.bind(this));
    }
    
    public detached(): void {
        this.element.removeEventListener("click", this.onButtonClick.bind(this));
    }
    
    private onButtonClick(): void {
        $(this.element)
            .closest(".message")
            .transition("fade");
    }
}
