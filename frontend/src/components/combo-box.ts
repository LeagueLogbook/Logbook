"use strict";

import {bindable} from "aurelia-framework";

export class ComboBox {
    @bindable()
    public items: Object[];
    
    @bindable()
    public selectedItem: Object;
        
    public itemToString(item: any) : string {
        if (item === null || typeof item === "undefined") {
            return "";            
        }
        
        return item.name;
    }
    
    public updateSelectedItem(item: Object): void {
        this.selectedItem = item;
    }
}
