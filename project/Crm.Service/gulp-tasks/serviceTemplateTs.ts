import "../Content/ts/helper/Helper.Installation";
import "../Content/ts/helper/Helper.ServiceObject";
import "../../Main/Content/ts/OfflineBootstrapper";
import "../Content/ts/ServiceOrderReportViewModel";
import "../Content/ts/DispatchReportViewModel";
import "../Content/ts/DispatchReportPreviewModalViewModel";
import {helperBase as Helper, mergeDeep} from "../../Main/Content/ts/helper/helper";
import {HelperInstallation} from "@Crm.Service/helper/Helper.Installation";
import {HelperServiceObject} from "@Crm.Service/helper/Helper.ServiceObject";
import {HelperServiceOrderMaterial} from "@Crm.Service/helper/Helper.ServiceOrderMaterial";
import {HelperServiceOrderTimePosting} from "@Crm.Service/helper/Helper.ServiceOrderTimePosting";
import "../../Crm/gulp-tasks/crmHelperTs";

window.Helper = mergeDeep(window.Helper, Helper);
window.Helper.Installation ||= HelperInstallation;
window.Helper.ServiceObject ||= HelperServiceObject;
window.Helper.ServiceOrderMaterial ||= HelperServiceOrderMaterial;
window.Helper.ServiceOrderTimePosting ||= HelperServiceOrderTimePosting;