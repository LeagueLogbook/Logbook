"use strict";

import * as $ from "jquery";
import "semantic-ui";
import {autoinject, customAttribute} from "aurelia-framework";

@customAttribute("semantic-dropdown")
@autoinject()
export class SemanticDropdown {
    public constructor(private element: Element) {
        $(element).dropdown();
    }
}
