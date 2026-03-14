/**
 * @param namespaceString dot delimited string
 * @example 
 * namespace("Crm.Example.Plugin").SomethingViewModel = SomethingViewModel;
 * let a = new window.Crm.Example.Plugin.SomethingViewModel();
 */
export function namespace(namespaceString: string): any {
    let parent = window;
    for (const part of namespaceString.split(".")) {
        parent[part] ||= {};
        parent = parent[part];
    }
    return parent;
}

window.namespace = namespace;
