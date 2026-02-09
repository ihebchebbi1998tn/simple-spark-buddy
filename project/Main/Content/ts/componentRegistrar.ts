type ComponentType = { 
	/** Custom component name, which contains 1 dash */
	componentName: string; 
	/** This should be a Function, not a lambda, because of Knockout's internal
	 * code does not play nice with it. */
	viewModel?: { 
		createViewModel: Function 
	} | ((...args) => void); 
	/** URL of the template */
	template: string;
	/** Add permission for the component
	 * @example
	 * WebAPI::RecentPage */
	permission?: string 
}


/**
 * Registers a component with the given component name, viewModel and template url.
 *
 * @param params - Parameters of the Knockout component
 *
 * @example
 * 
 * // MyComponent.ts
 * 
 * registerComponent({
 * 	componentName: "chat",
 * 	template: "Main.VideoCall/Template/Chat",
 * 	viewModel: {
 * 		createViewModel: function (params, componentInfo): any {
 * 			return params.viewModel;
 * 		},
 * 	},
 * })
 * 
 * // GulpTaskFile.ts 
 * 
 * import "path/to/MyComponent.ts"
 * 
 */
export function registerComponent(params: ComponentType): void {
	const config: any = {
		viewModel: params.viewModel,
		template: {url: params.template},
	};
	if(params.permission) {
		config.permission = params.permission;	
	}
	window.ko.components.register(params.componentName, config);
}
