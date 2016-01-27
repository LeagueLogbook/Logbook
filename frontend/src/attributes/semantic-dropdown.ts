"use strict";

import * as $ from "jquery";
import "semantic-ui";
import {autoinject} from "aurelia-framework";

@autoinject()
export class SemanticDropdownCustomDropdown {
    public constructor(private element: Element) {
        $(element).dropdown();
    }
}
