import * as UTIF from 'utif';

export const CompressionQualityLevel = {
	Lossless: 1,
	High: 0.8,
	Good: 0.6,
	Poor: 0.4
};

const CompressionLimitInKb = 512;

function dataURItoBlob(dataUri: string, imageType: string): Blob {
	const binary = atob(dataUri.split(",")[1]);
	const array = [];
	for (let i = 0; i < binary.length; i++) {
		array.push(binary.charCodeAt(i));
	}
	return new Blob([new Uint8Array(array)], {type: imageType});
}

function resizeImage(image: HTMLImageElement, maxImageWidth: number, maxImageHeight: number, outputQuality: number, outputFormat?: string): string {
	const canvas = document.createElement("canvas");
	let width = image.width;
	let height = image.height;
	const ratio = width / height;
	let quality = outputQuality || CompressionQualityLevel.Lossless;
	let format = outputFormat;

	if (maxImageHeight && height > maxImageHeight) {
		height = maxImageHeight;
		width = height * ratio;
	}
	if (maxImageWidth && width > maxImageWidth) {
		width = maxImageWidth;
		height = width / ratio;
	}

	if (!outputFormat || quality !== CompressionQualityLevel.Lossless) {
		format = "image/jpeg";
	}

	canvas.width = width;
	canvas.height = height;
	const ctx = canvas.getContext("2d");
	ctx.drawImage(image, 0, 0, width, height);

	return canvas.toDataURL(format, quality);
}

function loadImage(url: string): Promise<HTMLImageElement> {
	return new Promise(function (resolve, reject) {
		const img = new Image();
		img.onload = function () {
			resolve(img);
		};
		img.onerror = function () {
			resolve(null);
		}
		img.src = url;
	});
}

function getBase64Only(base64,  fileType: string) {
	if (fileType === "image/tiff") {
		return Helper.Image.convertTiffToCompressableType(base64);
	}
	return base64.toString().split("base64,")[1];
}

function base64ToArrayBuffer(base64) {
	const binaryString = atob(base64.split(',')[1]);
	const len = binaryString.length;
	const bytes = new Uint8Array(len);
	for (let i = 0; i < len; i++) {
		bytes[i] = binaryString.charCodeAt(i);
	}
	return bytes.buffer;
}

function convertTiffToCompressableType(base64Tiff: any) {
	const arrayBuffer = base64ToArrayBuffer(base64Tiff);
	const ifds = UTIF.decode(arrayBuffer);
	UTIF.decodeImage(arrayBuffer, ifds[0]);

	const rgba = UTIF.toRGBA8(ifds[0]);
	const width = ifds[0].width;
	const height = ifds[0].height;
	
	const canvas = document.createElement("canvas");
	canvas.width = width;
	canvas.height = height;
	const ctx = canvas.getContext("2d");
	const imageData = ctx.createImageData(width, height);
	imageData.data.set(rgba);
	ctx.putImageData(imageData, 0, 0);

	const base64PNG = canvas.toDataURL("image/png");

	return base64PNG.replace(/^data:image\/png;base64,/, '');
}

const HelperImage = {
	dataURItoBlob,
	resizeImage,
	loadImage,
	getBase64Only,
	base64ToArrayBuffer,
	convertTiffToCompressableType,
	CompressionQualityLevel,
	CompressionLimitInKb
};
export {HelperImage};
