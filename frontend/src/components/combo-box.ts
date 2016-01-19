"use strict";

import {bindable} from "aurelia-framework";
import * as _ from "lodash";

export class ComboBox {
    @bindable()
    public items: Object[];
    
    @bindable()
    public selectedItem: Object;
        
    public updateSelectedItem(item: Object): void {
        this.selectedItem = item;
    }
    /* tslint:disable:no-unused-variable */
    private itemsChanged(newValue: Object[], oldValue: Object[]): void {
    /* tsline:enable */
        this.selectedItem = _.head(newValue);
    }
}
