import {BarcodeFormat, BrowserMultiFormatReader} from "@zxing/browser";
import {DecodeHintType} from "@zxing/library";
import {HelperDatabase} from "@Main/helper/Helper.Database";
import "../js/mfb/modernizr.touchevents";
import QRCode from "qrcode";

async function checkPasswordStrength(password: any) {
	if (!password) {
		return {isValid: true};
	}
	try {
		let response = await fetch(window.Helper.Url.resolveUrl("~/Account/EvaluatePassword"), {
			method: "POST",
			headers: {
				"Content-Type": "application/x-www-form-urlencoded"
			},
			body: new URLSearchParams({
				"password": password
			})
		});

		let passwordStrength = await response.json();
		if (passwordStrength.Score < passwordStrength.MinPasswordStrength) {
			let message = "";
			for (let i = 0; i < passwordStrength.Feedback.Suggestions.length; i++) {
				message += " " + passwordStrength.Feedback.Suggestions[i];
			}
			message = message.trim();
			return {
				isValid: false,
				message: message
			};
		} else {
			return {
				isValid: true
			};
		}
	} catch (error) {
		return {
			isValid: true
		};
	}
}
function togglePasswordVisibility(inputId: string) {
	const passwordInput = document.getElementById(inputId) as HTMLInputElement;
	const toggleIcon = document.getElementById(inputId + '-toggle-icon');
	if (passwordInput && toggleIcon) {
		if (passwordInput.type === 'password') {
			passwordInput.type = 'text';
			toggleIcon.classList.remove('zmdi-eye');
			toggleIcon.classList.add('zmdi-eye-off');
		} else {
			passwordInput.type = 'password';
			toggleIcon.classList.remove('zmdi-eye-off');
			toggleIcon.classList.add('zmdi-eye');
		}
	}
}

(window as any).togglePasswordVisibility = togglePasswordVisibility;

function initializePasswordToggles() {
	$(document).on('click', '.btn-password-toggle', function(e) {
		e.preventDefault();
		e.stopPropagation();
		const inputId = $(this).siblings('input[type="password"], input[type="text"]').attr('id');
		if (inputId) {
			togglePasswordVisibility(inputId);
		}
	});
	$(document).on('keydown', '.btn-password-toggle', function(e) {
		if (e.key === 'Enter' || e.key === ' ') {
			e.preventDefault();
			$(this).trigger('click');
		}
	});

	$(document).on('focus', 'input[type="password"], input[type="text"]', function() {
		const inputId = $(this).attr('id');
		const toggleIcon = $('#' + inputId + '-toggle-icon');
		if (toggleIcon.length > 0) {
			if ($(this).attr('type') === 'text' && $(this).attr('placeholder')?.toLowerCase().includes('password')) {
					toggleIcon.removeClass('zmdi-eye').addClass('zmdi-eye-off');
			}
		}
	});
}
function setupLogin() {
	// @ts-ignore
	const hasTouchEvents = window.Modernizr.touchevents;
	if (!!window.location.hash && !window.location.search) {
		let returnUrl = encodeURIComponent(window.location.pathname.replace("Account/Login", "") + window.location.hash);
		history.replaceState(null, null, ' ');
		window.location.search = "?ReturnUrl=" + returnUrl;
	}
	if (navigator.userAgent.indexOf("Cordova") !== -1) {
		// @ts-ignore
		$.cookie("Cordova", true, {path: "/", expires: 36500});
	} else if (hasTouchEvents) {
		let platform = null;
		if (navigator.userAgent.indexOf("Android") !== -1) {
			platform = "Android";
		} else if (navigator.userAgent.indexOf("iPhone") !== -1 || navigator.userAgent.indexOf("iPad") !== -1 || navigator.userAgent.indexOf("iPod") !== -1 || navigator.userAgent.indexOf("Mac OS X") !== -1) {
			platform = "AppleIos";
		} else if (navigator.userAgent.indexOf("Window") !== -1) {
			platform = "Windows10";
		}
		let appLinks = $(".app-block[data-platform='" + platform + "']");
		if (appLinks.length > 0 && platform !== null) {
			$(appLinks).removeClass("hide");
			$("#l-login").hide();
			$("#l-login").addClass("hide");
			$("#openidlogin").addClass("hide");
			$(".app-suggestion").show();
		}
	}

	function submitForm($form) {
		const inputs = $form.find("input:visible");
		let valid = true;
		inputs.each(function (i, input) {
			const $input = $(input);
			if ($input.val() == null || $input.val() === "") {
				valid = false;
				const $inputContainer = $input.parents(".fg-line");
				$inputContainer.addClass("has-error");
				const $inputGroup = $input.parents(".input-group");
				if ($inputGroup.length === 0) {
					$inputContainer.addClass("animated shake");
				} else {
					$inputGroup.addClass("animated shake");
				}
			}
		});
		if (valid) {
			$form.find('input[type="text"]').each(function() {
				const placeholder = $(this).attr('placeholder');
				if (placeholder?.toLowerCase().includes('password')) {
					$(this).attr('type', 'password');
					}
			});

			if (window.location.hash)
				$form.attr("action", $form.attr("action") + encodeURIComponent(window.location.hash));
			$("#needRedirect").val(HelperDatabase.getFromLocalStorage("last-user-login") != null && $("#email").length !== 0 ? Boolean(HelperDatabase.getFromLocalStorage("last-user-login") !== window.Helper.String.hash($("#email").val()?.toString() || "").toString()).toString() : false.toString());

			$form.trigger("submit");
		}
		return false;
	}

	initializePasswordToggles();

	if (window.location.search)
		$("#openidloginbutton").attr("href", $("#openidloginbutton").attr("href") + window.location.search);
	$(document)
		.on("click", "#btn-login", function (e) {
			e.preventDefault();
			const $form = $(this).parents("form");
			submitForm($form);
		})
		.on("change, keydown, keyup", ".has-error input", function (e) {
			const errorParent = $(this).parents(".has-error");
			errorParent.removeClass("has-error");
			errorParent.children(".help-block.pre-line").hide();
		}).on("click", ".remove-login", function (e) {
		e.preventDefault();
		// @ts-ignore
		$.cookie("Login", null, {path: "/", expires: -1});
		window.location.href = window.Helper.Url.resolveUrl("~/Account/Login");
	}).on("click", ".logout-client-selection", function (e) {
		// @ts-ignore
		$.cookie("Login", null, {path: "/", expires: -1});
	}).on("click", "#goto-website", function (e) {
		e.preventDefault();
		$(".app-suggestion").hide();
		$("#openidlogin").removeClass("hide");
		$("#l-login").removeClass("hide");
		$("#l-login").show();
		return false;
	}).on("show.bs.modal", "#modal", async function (e) {
		const codeReader = new BrowserMultiFormatReader(new Map([[DecodeHintType.POSSIBLE_FORMATS, [BarcodeFormat.QR_CODE]]]));
		const devices = (await navigator.mediaDevices.enumerateDevices()).filter(val => val.kind === "videoinput" && val.label.search(/ir\s/i) === -1);
		if (devices.length === 0) {
			$("#modal").modal("hide");
			return;
		}
		const device = devices.find(value => value.label.includes("facing back"));
		try {
			await codeReader.decodeFromVideoDevice((device ?? devices[0]).deviceId, "barcodeScannerVideo", (result, err) => {
				if (result) {
					$("#modal").modal("hide");
					window.location.href = window.Helper.Url.resolveUrl(`~/Main/Home/Index?token=${result.getText()}`);
				}
			});
		} catch (e) {
			console.error(e);
		}
	}).on("hidden.bs.modal", "#modal", function (e) {
		BrowserMultiFormatReader.releaseAllStreams();
		const stream = ($("video")[0] as HTMLVideoElement).srcObject as MediaStream;
		if (!!stream)
			stream.getTracks().forEach(x => x.stop());
	}).on("change, keydown, keyup", "#newPassword", function (e) {
		checkPasswordStrength($(this).val()).then(result => {
			console.log(result);
			if (!result.isValid) {
				$(".is-weak .help-block").html(result.message);
				$(".is-weak").show();
			} else {
				$(".is-weak .help-block").html("");
				$(".is-weak").hide();
			}
		});
	});

	manageTotpElements();

	function manageTotpElements() {
		if ($("#totp-token-container").length === 0) {
			return;
		}

		const $barcodeCanvas = $("#totp-barcode-canvas")
		if ($barcodeCanvas.length > 0) {
			$barcodeCanvas.css("height", "auto");
			$barcodeCanvas.css("width", "auto");

			const authorizationUri = $("#totp-authorization-uri").text();

			QRCode.toCanvas($barcodeCanvas[0], authorizationUri, {width: 250});

			const $authorizationKeyCopyElement = $("#totp-authorization-uri-copy");
			if (!window.isSecureContext) {
				$authorizationKeyCopyElement.hide();
			}
			const authorizationKey = $("#totpAuthorizationKeyInput").val().toString();
			$authorizationKeyCopyElement.on("click", () => copyAuthorizationKeyToClipboard(authorizationKey));
		}

		const $tokenInputs = $(".totp-token-input");
		const $hiddenTokenInput = $("#totpToken");

		$("#totpToken1").trigger("focus");

		async function totpSubmit() {
			if ($(this).prop("disabled") || !calculateIfFormIsValid()) {
				return;
			}

			let token = $hiddenTokenInput.val().toString();
			const newAuthorizationKey = $("#newAuthorizationKey").val()?.toString() || "";

			try {
				const response = await fetch(window.Helper.Url.resolveUrl("~/Account/ValidateTotp"), {
					method: "POST",
					headers: {
						"Content-Type": "application/x-www-form-urlencoded"
					},
					body: new URLSearchParams({
						"token": token,
						"newAuthorizationKey": newAuthorizationKey
					})
				});

				if (response.ok) {
					const $form = $('form#login-mfa-totp-form');
					submitForm($form);
				} else {
					let jsonResult;
					try {
						jsonResult = await response.json();
					} catch (error) {
						renderInternalServerError();
					}
					if (jsonResult && jsonResult.message) {
						renderError(jsonResult.message);
					}
				}
			} catch (error) {
				renderInternalServerError();
			}
		}

		function renderInternalServerError(): void {
			const errorMessage = $("#internalServerErrorMessage").val().toString();
			renderError(errorMessage);
		}

		function renderError(errorMessage: string): void {
			const $errorBlock = $("div.has-error > small.help-block");
			$(".has-error").css("visibility", "visible");

			if ($errorBlock.find("span.totp-error").length === 0) {
				const errorSpan = $("<span>", {
					class: "field-validation-error totp-error",
					generated: "false",
					text: errorMessage
				});

				$errorBlock.append(errorSpan);
			}
		}

		$tokenInputs.each(function () {
			this.addEventListener("textInput", function (e) {
				// @ts-ignore
				const key = e.data;

				$(".has-error").css("visibility", "hidden");

				if (/^\d$/g.test(key)) {
					// @ts-ignore
					e.target.value = key;
					combineToken();
					for (let i = 0; i < $tokenInputs.length; i++) {
						const $input = $tokenInputs.eq(i);
						if (i < $tokenInputs.length - 1 && $input.is(":focus") && $input.val()) {
							const $nextInput = $tokenInputs.eq(i + 1);
							$nextInput.trigger("focus");
							break;
						}
					}

					if (calculateIfFormIsValid()) {
						totpSubmit();
					}
				} else if (key !== "Backspace" && key !== "Enter") {
					e.preventDefault();
					return false;
				}
				return true;
			});
		});

		$tokenInputs.on("keyup", function (e) {
			const key = e.key;

			if (key === "Backspace") {
				$(".has-error").css("visibility", "hidden");

				combineToken();
				for (let i = $tokenInputs.length - 1; i >= 0; i--) {
					const $input = $tokenInputs.eq(i);
					if ($input.is(":focus")) {
						$input.val("");
						if (i > 0) {
							const $previousInput = $tokenInputs.eq(i - 1);
							$previousInput.trigger("focus");
							break;
						}
					}
				}
			}
			return true;
		}).on("cut copy", function (e) {
			e.preventDefault();
		}).on("paste", function (e) {
			e.preventDefault();

			$(".has-error").css("visibility", "hidden");

			// @ts-ignore
			const clipboardData = e.originalEvent.clipboardData || window.clipboardData;
			const pastedData = clipboardData.getData("Text");
			const token = pastedData.replace(/\D/g, "");

			const focusedIndex = $tokenInputs.index($tokenInputs.filter(":focus"));
			if (focusedIndex !== -1) {
				let lastFilledIndex = focusedIndex;

				for (let i = 0; i < token.length && (focusedIndex + i) < $tokenInputs.length; i++) {
					$tokenInputs.eq(focusedIndex + i).val(token[i]);
					lastFilledIndex = focusedIndex + i;
				}

				$tokenInputs.eq(lastFilledIndex).trigger("focus");
			}

			combineToken();

			if (calculateIfFormIsValid()) {
				totpSubmit();
			}
		});

		async function copyAuthorizationKeyToClipboard(authorizationKey: string): Promise<void> {
			await navigator.clipboard.writeText(authorizationKey);

			const copySuccessMessage = $("#copySuccessMessage").val().toString();
			$.growl(
				{
					message: copySuccessMessage
				},
				{
					type: "inverse",
					allow_dismiss: true,
					placement: {
						from: "top",
						align: "center"
					},
					delay: 1500,
					animate: {
						enter: "animated fadeInDown",
						exit: "animated fadeOut"
					}
				});
		}

		function combineToken() {
			let token = "";

			$tokenInputs.each(function () {
				token += $(this).val();
			});

			$hiddenTokenInput.val(token);
		}

		function calculateIfFormIsValid() {
			return $hiddenTokenInput.val().toString().length === 6;
		}
	}
}

window.addEventListener("load", () => {
	setupLogin();
	if (HelperDatabase.getFromLocalStorage("email") != null) {
		const adName = HelperDatabase.getFromLocalStorage("adName")
		let loginName = null;
		if (adName) {
			loginName = adName;
		} else {
			loginName = HelperDatabase.getFromLocalStorage("email");
		}

		$("#pre-populated-email").html(loginName);
		const loginNameInput = $("input[name='email']");
		if (loginNameInput.length > 0) {
			loginNameInput.val(loginName);
		}
	}
	if (HelperDatabase.getFromLocalStorage("displayName") != null && HelperDatabase.getFromLocalStorage("displayName") != 'null') {
		$("#pre-populated-display-name").html(HelperDatabase.getFromLocalStorage("displayName"));
	} else {
		$("#pre-populated-display-name").hide();
	}
	if (HelperDatabase.getFromLocalStorage("avatar") != null && HelperDatabase.getFromLocalStorage("avatar") != 'null') {
		$("#pre-populated-avatar").show()
		$("#pre-populated-avatar").attr("src", "data:image;base64," + HelperDatabase.getFromLocalStorage("avatar"));
	}
	if (navigator.mediaDevices) {
		if (!document.getElementById("barcodeScanner")) return;
		const updateCameraStatus = () => window.navigator.mediaDevices.enumerateDevices()
			.then(value => value.filter(val => val.kind === "videoinput")
				.filter(devices => devices.label.search(/ir\s/i) === -1).length > 0 ?
				document.getElementById("barcodeScanner").classList.remove("hidden") :
				document.getElementById("barcodeScanner").classList.add("hidden"));
		updateCameraStatus();
		navigator.mediaDevices.addEventListener("devicechange", updateCameraStatus);
	}
});