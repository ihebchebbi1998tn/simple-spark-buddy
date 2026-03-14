const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
    // Original location : .\
    let cssFiles = [
        "Content/style/base/jquery.signaturepad.custom.css",
        "../../node_modules/fullcalendar/dist/fullcalendar.css",
        "../../node_modules/animate.css/animate.css",
        "../../node_modules/material-design-iconic-font/dist/css/material-design-iconic-font.css",
        "../../node_modules/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.css",
        "../../node_modules/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
        "Content/style/base/bootstrap-datetimepicker.custom.css",
        "../../node_modules/bootstrap-select/dist/css/bootstrap-select.css",
        "../../node_modules/leaflet/dist/leaflet.css",
        "../../node_modules/leaflet.markercluster/dist/MarkerCluster.css",
        "../../node_modules/leaflet.markercluster/dist/MarkerCluster.Default.css",
        "../../node_modules/yarn-mprogress/build/css/mprogress.css",
        "../../node_modules/bootstrap-sweetalert/lib/sweet-alert.css",
		"../../node_modules/nouislider/distribute/jquery.nouislider.min.css",
		"../../node_modules/nouislider/distribute/jquery.nouislider.pips.min.css",
        "Content/style/base/jquery.mCustomScrollbar.css",
        "Content/style/material/app.min.1.css",
        "Content/style/material/app.min.2.css",
        "Content/style/material/app.custom.css",
        "Content/style/material/app.print.css",
        "Content/style/mfb/mfb.min.css",
        "Content/style/mfb/mfb.patch.css",
        "../../node_modules/select2/dist/css/select2.css",
        "Content/style/select2/select2material.css",
        "../../node_modules/dropzone/dist/dropzone.css",

    ];

    generateCssTask(__filename, cssFiles);

};
