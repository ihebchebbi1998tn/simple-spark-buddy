import "../Content/js/knockout.custom.select2autocompleter2.js";

import "../Content/ts/knockout.custom.planningBoardMap.ts";
import "../Content/ts/Helper/DragHelper";
import "../Content/ts/Consumer.ts"
import "../Content/ts/Lazy.ts";
import "../Content/ts/Legend.ts";
import "../Content/ts/MapPanel.ts";
import "../Content/ts/Pipeline";
import "../Content/ts/ResourceUtilization.ts";
import "../Content/ts/Modals/SchedulerAdHocModalViewModel.ts";
import "../Content/ts/Modals/SchedulerAddProfileModalViewModel.ts";
import "../Content/ts/Modals/SchedulerAddResourceModalViewModel.ts";
import "../Content/ts/SchedulerDetailsViewModel.ts";
import "../Content/ts/Modals/SchedulerEditModalViewModel.ts";
import "../Content/ts/Modals/SchedulerEditProfileModalViewModel.ts";
import "../Content/ts/Modals/SchedulerLoadProfileModalViewModel.ts";
import "../Content/ts/SchedulerMapViewModel.ts";
import "../Content/ts/Timeline";
import "../Content/ts/knockout-async-computed.ts";
import "../Content/ts/knockout.component.Map.ts";
import "../Content/ts/knockout.component.Scheduler.ts";
import "../Content/ts/Modals/SchedulerGetRouteModalViewModel.ts";
import "../Content/ts/Modals/SchedulerResourceReorderModalViewModel.ts";
import "../Content/ts/Extensions/DashboardCalendarWidgetViewModelExtension";
import "../Content/ts/Extensions/StartupViewModelExtension";

import "../Content/ts/Model/Absence.ts";
import "../Content/ts/Model/AbsenceOrder.ts";
import "../Content/ts/Model/Article.ts";
import "../Content/ts/Model/Assignment.ts";
import "../Content/ts/Model/AssignmentStore.ts";
import "../Content/ts/Model/CrudManager.ts";
import "../Content/ts/Model/Dispatch.ts";
import "../Content/ts/Model/DispatchStore.ts";
import "../Content/ts/Model/HourSpan.ts";
import "../Content/ts/Model/MapMessage.ts";
import "../Content/ts/Model/RouteData.ts";
import "../Content/ts/Model/ServiceOrder.ts";
import "../Content/ts/Model/ServiceOrderTimePosting.ts";
import "../Content/ts/Model/Technicians.ts";

import {HelperScheduler} from "../Content/ts/Helper/Helper.Scheduler";

window.Helper.Scheduler ||= HelperScheduler;