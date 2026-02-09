ko.bindingHandlers.fileInput = {
	init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
		const options = ko.unwrap(valueAccessor());

		async function compressImage(args) {
			const deferred = $.Deferred();
			const compressibleImageTypes = ["image/bmp", "image/jpeg", "image/png", "image/webp", "image/tiff"];
			const fileToCompress = args[0];
			const base64 = args[1];
			if (fileToCompress.type.length === 0 || compressibleImageTypes.indexOf(fileToCompress.type) === -1 || fileToCompress.size < window.Helper.Image.CompressionLimitInKb * 1024) {
				return deferred.resolve({ file: fileToCompress, content: base64 });
			}
			const compressionQuality = options.compressionQualityLevel || window.Helper.Image.CompressionQualityLevel.Good;
			const fileType = compressionQuality === window.Helper.Image.CompressionQualityLevel.Lossless ? fileToCompress.type : "image/jpeg";
			let blob = window.Helper.String.base64toBlob(base64, fileType);
			window.URL = window.URL || window.webkitURL;
			const blobUrl = window.URL.createObjectURL(blob);
			const image = await window.Helper.Image.loadImage(blobUrl);
				if (image === null)
					return deferred.resolve({ file: fileToCompress, content: base64 });

			const resizedImage = window.Helper.Image.resizeImage(image, image.width, image.height, compressionQuality, fileType);
			blob = window.Helper.Image.dataURItoBlob(resizedImage, fileType);
			const res = await window.Helper.String.blobToBase64(blob);
			const fileName = compressionQuality === window.Helper.Image.CompressionQualityLevel.Lossless ? fileToCompress.name : fileToCompress.name.split(".")[0].concat(".jpg");
			const file = new window.NativeFile([blob], fileName, {type: blob.type, lastModified: fileToCompress.lastModified});

			return deferred.resolve({ file: file, content: res, blobUrl: blobUrl }).promise();
		}

		function processFile(file) {
			if (file == null) {
			  if (ko.isWriteableObservable(options.name)) {
				options.name(null);
			  }
			  if (ko.isWriteableObservable(options.contentType)) {
				options.contentType(null);
			  }
			  if (ko.isWriteableObservable(options.size)) {
				options.size(0);
			  }
			  if (ko.isWriteableObservable(options.content)) {
				options.content(null);
			  }
			  if (ko.isWriteableObservable(options.blob)) {
				options.blob(null);
			  }
			  if (ko.isWriteableObservable(options.blobUrl)) {
				options.blobUrl(null);
			  }
			} else {
				const reader = new FileReader();
				reader.onload = (function (currentFile) {
					return function (e) {
						const base64String = Helper.Image.getBase64Only(e.target.result, currentFile.type);
						return $.Deferred().resolve([currentFile, base64String]).promise().then(function (args) {
							return compressImage(args).then(function(result) {
								if (ko.isWriteableObservable(options.name)) {
									options.name(result.file.name);
								}
								if (ko.isWriteableObservable(options.contentType)) {
									options.contentType(result.file.type);
								}
								if (ko.isWriteableObservable(options.size)) {
									options.size(result.file.size);
								}
								if (ko.isWriteableObservable(options.content)) {
									options.content(result.content);
								}
								if (ko.isWriteableObservable(options.blobUrl)) {
									options.blobUrl(result.blobUrl);
									window.URL.revokeObjectURL(result.blobUrl);
								}
								if (ko.isWriteableObservable(options.blob)) {
									options.blob(result.file);
								}
							});
						});
					};
				})(file);
				reader.readAsDataURL(file);
			}
		}

		$(element).on("change", function (e) {
			if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
				const fileApiAlertString = window.Helper.getTranslatedString("M_FileApiNotSupported");
				alert(fileApiAlertString);
				return false;
			}
			processFile(e.target.files.length > 0 ? e.target.files[0] : null);
			return false;
		});
		const $container = $(element).parents(".fileinput");
		// @ts-ignore
		$container.fileinput({ name: window.$data.createGuid().toString() });
		const name = ko.unwrap(options.name);
		if (name) {
			const event = $.Event("change", { target: { files: [ { name: name } ] } });
			const fileinput = $container.data("bs.fileinput");
			fileinput.change.call(fileinput, event);
		}
	}
};