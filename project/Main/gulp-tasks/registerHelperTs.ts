import {HelperDOM} from "../Content/ts/helper/Helper.DOM";
import {HelperUrl} from "../Content/ts/helper/Helper.Url";
import {HelperCulture} from "../Content/ts/helper/Helper.Culture";
import {helperBase as Helper, mergeDeep} from "../Content/ts/helper/helper";

window.Helper = mergeDeep(window.Helper, Helper);
window.Helper.Culture ||= HelperCulture;
window.Helper.DOM ||= HelperDOM;
window.Helper.Url ||= HelperUrl;

