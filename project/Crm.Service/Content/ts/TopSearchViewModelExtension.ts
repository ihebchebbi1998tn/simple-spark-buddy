export class TopSearchViewModelExtension extends window.Main.ViewModels.TopSearchViewModel {
    constructor() {
        super();

        if (window.AuthorizationManager.isAuthorizedForAction("TopSearch", "ServiceOrderDispatch")) {
            this.entities.push({
                caption: "Dispatches",
				searchProperties: ["DispatchNo", "ServiceOrder.OrderNo", "ServiceOrder.ErrorMessage", "ServiceOrder.PurchaseOrderNo", "ServiceOrder.Company.Name"],
                template: "item-ServiceOrderDispatchList",
                viewModel: new window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel(null)
            });
        }
        if (window.AuthorizationManager.isAuthorizedForAction("TopSearch", "ServiceOrderHead")) {
            this.entities.push({
                caption: "ServiceOrders",
				searchProperties: ["OrderNo", "ErrorMessage", "PurchaseOrderNo", "Company.Name"],
                template: "item-ServiceOrderHeadList",
                viewModel: new window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel()
            });
        }
        if (window.AuthorizationManager.isAuthorizedForAction("TopSearch", "ServiceCase")) {
            this.entities.push({
                caption: "ServiceCases",
                searchProperties: ["ServiceCaseNo", "ErrorMessage"],
                template: "item-ServiceCaseList",
                viewModel: new window.Crm.Service.ViewModels.ServiceCaseListIndexViewModel()
            });
        }
        if (window.AuthorizationManager.isAuthorizedForAction("TopSearch", "Installation")) {
            this.entities.push({
                caption: "Installations",
				searchProperties: ["InstallationNo", "LegacyInstallationId", "Description"],
                template: "item-InstallationList",
                viewModel: new window.Crm.Service.ViewModels.InstallationListIndexViewModel()
            });
        }
        if (window.AuthorizationManager.isAuthorizedForAction("TopSearch", "ServiceContract")) {
            this.entities.push({
                caption: "ServiceContracts",
                searchProperties: ["ContractNo"],
                template: "item-ServiceContractList",
                viewModel: new window.Crm.Service.ViewModels.ServiceContractListIndexViewModel()
            });
        }
    }
}

window.Main.ViewModels.TopSearchViewModel = TopSearchViewModelExtension;