# Custom knockout extenders

  If you wish to add additional functionality to an observable (ex. different read/write behaviours) use extenders. There are already some defined in the application.

## filterOperator

  The `filterOperator` extender used at the rendering of filters in material client. This extender is responsible for the javascript generic list filter's structure.

### Example

  Example for single parameter usage

```html
  <% if (Model.Item.FilterType == typeof(bool)) { %>
    <%var modelPath = Model.Item.GetModelPath();
    var read = $"function() {{ var value = ko.unwrap(this.getFilter('{modelPath}')); return value !== null && value !== undefined && value.Operator ? value.Value : value; }}";
    var write = $"function(newValue) {{ this.getFilter('{modelPath}').extend({{ filterOperator: {'==='} }})(newValue); }}";
    var data = $"ko.pureComputed({{ read: {read}, write: {write} }}, $data)"; %>
    <div class="fg-line select">
      <p class="m-b-5"><%= Html.Localize(caption) %></p>
      <select name="<%= Model.Item.GetModelPath() %>" data-bind="value: <%= data %>, options: booleanOptions, optionsValue: 'Value', optionsText: function(x) { return Helper.String.getTranslatedString(x.Text); }" class="form-control">
      </select>
    </div>
  <% }
```

   Example for single parameter usage - with operator object

```html
  <% if (Model.Item.FilterType == typeof(bool)) { %>
    <%var modelPath = Model.Item.GetModelPath();
    var read = $"function() {{ var value = ko.unwrap(this.getFilter('{modelPath}')); return value !== null && value !== undefined && value.Operator ? value.Value : value; }}";
    var write = $"function(newValue) {{ this.getFilter('{modelPath}').extend({{ filterOperator: {{ type: 'some', collectionName: '{modelPath}', collectionProperty: '{filterDefinition.CollectionIdProperty.Name}' }} }})(newValue); }}";
    var data = $"ko.pureComputed({{ read: {read}, write: {write} }}, $data)"; %>
    <div class="fg-line select">
      <p class="m-b-5"><%= Html.Localize(caption) %></p>
      <select name="<%= Model.Item.GetModelPath() %>" data-bind="value: <%= data %>, options: booleanOptions, optionsValue: 'Value', optionsText: function(x) { return Helper.String.getTranslatedString(x.Text); }" class="form-control">
      </select>
    </div>
  <% }
```

  Example for additional parameter usage

```html
  <% if (Model.Item.FilterType == typeof(bool)) { %>
    <%var modelPath = Model.Item.GetModelPath();
    var read = $"function() {{ var value = ko.unwrap(this.getFilter('{modelPath}')); return value !== null && value !== undefined && value.Operator ? value.Value : value; }}";
    var write = $"function(newValue) {{ this.getFilter('{modelPath}').extend({{ filterOperator: {{ operator: '===', additionalProperties: {{caption: 'testCaption'}} }} }})(newValue); }}";
    var data = $"ko.pureComputed({{ read: {read}, write: {write} }}, $data)"; %>
    <div class="fg-line select">
      <p class="m-b-5"><%= Html.Localize(caption) %></p>
      <select name="<%= Model.Item.GetModelPath() %>" data-bind="value: <%= data %>, options: booleanOptions, optionsValue: 'Value', optionsText: function(x) { return Helper.String.getTranslatedString(x.Text); }" class="form-control">
      </select>
    </div>
  <% }
```

### Parameters

- operator

  This parameter will define the filter's Operator attribute. The first two example above shows how to add a simple operator value or add an operator object.

- additionalProperties

  The parameters defined in additionalProperties object will be added as properties to the  javascript filter object. 
  In this case you need to call the extender with an object what has an additionalProperties named object as property. 
  In this inner object you can define the properties what you wanna use in javascript filters.\
  See third example for usage.
