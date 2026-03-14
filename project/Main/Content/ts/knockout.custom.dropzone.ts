ko.bindingHandlers.dropzone = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		const value = ko.unwrap(valueAccessor());
		const acceptedFiles: string = value.acceptFile || "";
		const maxFiles = value.maxFiles || null;
		const maxFileSize = value.maxFileSize / 1024 || +window.Main.Settings.MaxFileLengthInKb || 256;
			const name = value.name;
		let loading = value.loading || window.ko.observable(false);
		let filesWithInvalidFileType = [];
		let fileWithSizeExceeded = [];
		let fileWithUploadCountExceeded = [];
		const disable = value.disable || false;

		const acceptedFilesMessage = acceptedFiles === "" ? "" : `<div><small>(${acceptedFiles})</small></div>`;
			const dropFileMessage = maxFiles === 1
				? window.Helper.getTranslatedString("DropFile")
				: window.Helper.getTranslatedString("DropFiles");

		const options = {
			maxFiles: maxFiles,
			maxFileSize: maxFileSize,
			addRemoveLinks: true,
			previewTemplate: '<div style="display: none"></div>',
			acceptedFiles: acceptedFiles,
			dictDefaultMessage:`${dropFileMessage}<br>
				<small>${window.Helper.getTranslatedString("MaximumAllowedFilesize")}: ${maxFileSize} kB 
				(${maxFileSize / 1024} MB)</small>${acceptedFilesMessage}`,
			dictRemoveFile: window.Helper.getTranslatedString("Remove"),
			dictCancelUpload: window.Helper.getTranslatedString("CancelTitle"),
			createImageThumbnails: false,
			autoQueue: false,
			init: function () {

				this.on("addedfiles", files => {
					try {
						loading(true);
						const dropZone = this;
						if (value.files !== undefined) {
							const validFiles = validateFiles(files);
							for (const file of validFiles) {
								const reader = new FileReader();
								reader.onload = (function (file) {
									return async function (e) {
										const compressibleImageTypes = ["image/bmp", "image/jpeg", "image/png", "image/webp", "image/tiff"];
										const fileResource = window.database.Crm_FileResource.defaultType.create();
										const base64String = Helper.Image.getBase64Only(e.target.result, file.type);
										fileResource.OfflineRelevant = true;
										fileResource.Filename = file.name;
										fileResource.ContentType = file.type;
										if(file.type.length > 0 && compressibleImageTypes.indexOf(file.type) !== -1 && file.size >= window.Helper.Image.CompressionLimitInKb * 1024) {
											// @ts-ignore
											const compressionQuality = options.compressionQualityLevel || window.Helper.Image.CompressionQualityLevel.Good;
											const fileType = compressionQuality === window.Helper.Image.CompressionQualityLevel.Lossless ? file.type : "image/jpeg";
											let blob = window.Helper.String.base64toBlob(base64String, fileType);
											window.URL = window.URL || window.webkitURL;
											const blobUrl = window.URL.createObjectURL(blob);
											const image = await window.Helper.Image.loadImage(blobUrl);
											if (image) {
												const resizedImage = window.Helper.Image.resizeImage(image, image.width, image.height, compressionQuality, fileType);
												blob = window.Helper.Image.dataURItoBlob(resizedImage, fileType);
												const res = await window.Helper.String.blobToBase64(blob);
												const fileName = compressionQuality === window.Helper.Image.CompressionQualityLevel.Lossless ? file.name : file.name.split(".")[0].concat(".jpg");
												fileResource.Length = blob.size;
												fileResource.Content = res;
												fileResource.Filename = fileName;
												fileResource.ContentType = fileType;
												file = new window.NativeFile([blob], fileName, {type: blob.type, lastModified: file.lastModified});
											} else {
												fileResource.Content = base64String;
												fileResource.Length = file.size;
											}
										} else {
											fileResource.Content = base64String;
											fileResource.Length = file.size;
										}
										const newFileResource = fileResource.asKoObservable();
										// @ts-ignore
										newFileResource.$file = file;
										value.files.unshift(newFileResource);
									};
								})(file);
								reader.readAsDataURL(file);
							}
							dropZone.processQueue();
						}
					} catch (e: any) {
						console.error(e.message);
					} finally {
						displayUploadWarnings();
						loading(false);
					}
				});

				this.on("addedfile", file => {
					try {
						loading(true);
						let dropZone = this;
						if (value.files === undefined) {
							let validatedFile = validateFiles(file);
							if (validatedFile.length) {
								const reader = new FileReader();
								reader.onload = (function (file) {
									return async function (e) {
										const compressibleImageTypes = ["image/bmp", "image/jpeg", "image/png", "image/webp", "image/tiff"];
										const base64String = Helper.Image.getBase64Only(e.target.result, file.type);
										if (ko.isWriteableObservable(value.fileResource)) {
											value.fileResource().Filename(file.name);
											value.fileResource().ContentType(file.type);
											if (file.type.length > 0 && compressibleImageTypes.indexOf(file.type) !== -1) {
												// @ts-ignore
												const compressionQuality = options.compressionQualityLevel || window.Helper.Image.CompressionQualityLevel.Good;
												const fileType = compressionQuality === window.Helper.Image.CompressionQualityLevel.Lossless ? file.type : "image/jpeg";
												let blob = window.Helper.String.base64toBlob(base64String, fileType);
												window.URL = window.URL || window.webkitURL;
												const blobUrl = window.URL.createObjectURL(blob);
												const image = await window.Helper.Image.loadImage(blobUrl);
												if (image) {
													const resizedImage = window.Helper.Image.resizeImage(image, image.width, image.height, compressionQuality, fileType);
													blob = window.Helper.Image.dataURItoBlob(resizedImage, fileType);
													const res = await window.Helper.String.blobToBase64(blob);
													const fileName = compressionQuality === window.Helper.Image.CompressionQualityLevel.Lossless ? file.name : file.name.split(".")[0].concat(".jpg");

													value.fileResource().Filename(fileName);
													value.fileResource().ContentType(fileType);
													value.fileResource().Length(blob.size);
													value.fileResource().Content(res);
												} else {
													value.fileResource().Length(file.size);
													value.fileResource().Content(base64String);
												}
											} else {
												value.fileResource().Length(file.size);
												value.fileResource().Content(base64String);
											}
										}
										loading(false);
									};
								})(file);
								reader.readAsDataURL(file);
							}
							dropZone.processQueue();
						}
					} catch (e: any) {
						console.error(e.message);
					} finally {
						loading(false);
					}
				});
			}
		};

		const validateFiles = function(files) {
			if (files instanceof FileList)
				files = Array.from(files);

			let filesToValidate = [];
			if (files instanceof Array)
				filesToValidate = filesToValidate.concat(files);
			else
				filesToValidate.push(files);

			filesWithInvalidFileType = [];
			fileWithSizeExceeded = [];
			fileWithUploadCountExceeded = [];

			filesToValidate.forEach(function (file) {
				if (!(acceptedFiles.includes(file.type) || acceptedFiles === "")) {
					filesWithInvalidFileType.push(file.name);
				}
				if (file.size > maxFileSize * 1024) {
					fileWithSizeExceeded.push(file.name);
				}
			});
			filesWithInvalidFileType.concat(fileWithSizeExceeded).forEach(function (fileName) {
				let i = filesToValidate.map(function (file) { return file.name; }).indexOf(fileName);
				if (i > -1) {
					filesToValidate.splice(i, 1);
				}
			});
			if (maxFiles !== null && (value.files && value.files().length + filesToValidate.length > maxFiles)) {
				fileWithUploadCountExceeded = value.files().concat(filesToValidate).slice(maxFiles).map(function (file) { return file.name; });
				filesToValidate.splice(maxFiles - value.files().length);
			}
			return filesToValidate;
		}

		const displayUploadWarnings = function() {
			let warningMessage = "";
			if (filesWithInvalidFileType.length > 0)
				warningMessage = window.Helper.String.getTranslatedString("RuleViolation.Format").replace("{0}", window.Helper.String.getTranslatedString("File")) + "\r\n" + filesWithInvalidFileType.join("; ");
			if (fileWithSizeExceeded.length > 0)
				warningMessage += (warningMessage.length > 0 ?  "\r\n\r\n" : "") + window.Helper.String.getTranslatedString("MaxFileSizeExceeded") + "\r\n" + fileWithSizeExceeded.join("\r\n");
			if (fileWithUploadCountExceeded.length > 0)
				warningMessage += (warningMessage.length > 0 ?  "\r\n\r\n" : "") + window.Helper.String.getTranslatedString("MaxUploadCountExceeded") + "\r\n" + fileWithUploadCountExceeded.join("\r\n");

			if (warningMessage) {
				window.Helper.Confirm.genericConfirm({
					text: warningMessage,
					type: "warning",
					showCancelButton: false
				});	
			}
		}

		const formElement = $(element).parents("form-element")[0];
		if (formElement !== undefined) {
			formElement.addEventListener('click', function (event) {
				if ((event.target as HTMLElement).id.length === 0)
					return false;

				const removeButtonId = value.name.length > 0 ? `${value.name}-removeFileResource` : 'dropzone-removeFileResource';
				if ((event.target as HTMLElement).id === removeButtonId) {
					if (ko.isWriteableObservable(value.fileResource)) {
						value.fileResource().Filename(null);
						value.fileResource().ContentType(null);
						value.fileResource().Length(null);
						value.fileResource().Content(null);
					}
				}
				return true;
			}, false);
		}

		$.extend(options, value);
		
		$(element).addClass('dropzone');
		const dropzone = new Dropzone(element, options);
		ko.utils.domNodeDisposal.addDisposeCallback(element, () => {
			dropzone.destroy();
		});
		if (disable) {
			dropzone.disable();
		}
	}
}