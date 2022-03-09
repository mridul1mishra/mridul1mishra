$(window).on("load", function () {
	if (jQuery(".promotion-form form").attr("action") != undefined && jQuery(".promotion-form form").attr("action").startsWith("/form")) {

		var formactionnew = "";
		jQuery(".promotion-form form").attr("action", formactionnew);

	}
	if (jQuery(".sitecore-form form").attr("action") != undefined && jQuery(".sitecore-form form").attr("action").startsWith("/form")) {

		var formactionnew = "";
		jQuery(".sitecore-form form").attr("action", formactionnew);

	}

	if ((localStorage.getItem("MultiChannel") != null) && (jQuery("input[value='{campaign_id}']").val() == "{campaign_id}")) {
		jQuery("input[value='{leadsource}']").val(JSON.parse(localStorage.getItem("MultiChannel")).campaign_source);
		jQuery("input[value='{campaign_id}']").val(JSON.parse(localStorage.getItem("MultiChannel")).campaign_id);
		jQuery("input[value='{productname}']").val(new URLSearchParams(jQuery(".sitecore-form form").attr("action")).get("product Name"));
		if (jQuery("input[value='{productname}']").val() == "") {

			var productname = new URLSearchParams(jQuery(".sitecore-form form").attr("action")).get("product Name");
			jQuery("input[value='{productname}']").val(productname);
			if (productname == null) {
				productname1 = window.location.search;
				productname2 = new URLSearchParams(productname1).get("product Name");
				jQuery("input[value='{productname}']").val(productname2);
			}
		}
	}
	else if (localStorage.getItem("MultiChannel") == null) {
		jQuery("input[value='{campaign_id}']").val("7012j0000000lts");
		jQuery("input[value='{leadsource}']").val("");
		var productname = new URLSearchParams(jQuery(".sitecore-form form").attr("action")).get("product Name");
		jQuery("input[value='{productname}']").val(productname);
		if (productname == null) {

			jQuery("input[value='{productname}']").val(new URLSearchParams(window.location.search).get("product Name"));
		}

	}
	if (jQuery(".sitecore-form form").attr("action").startsWith("/form")) {
		var formaction = jQuery(".sitecore-form form").attr("action");
		var formactionnew = "/fbsg" + formaction;
		jQuery(".sitecore-form form").attr("action", formactionnew);
		console.log(jQuery(".sitecore-form form").attr("action"));
	}


});

function captchaInvalidOperation() {
	var opco = "/" + window.location.pathname.split("/")[1];
	var url = opco + "/api/RPFormData";
	var language = window.location.pathname.split("/")[2];
	var obj = { "invalidRequest": "form" };
	$.ajax({
		type: "POST",
		dataType: 'json',
		url: url,
		contentType: 'application/json', // Not to set any content header
		data: JSON.stringify(obj),

		success: function (data) {
			console.log(data);
			jQuery("<span>" + "Invalid recaptcha" + '</span>').insertAfter(".sitecore-form form");
		},
		error: function (data) {
			console.log(data);
		},
	});
}

jQuery(".sitecore-form form").submit(function (e) {
	var out = true;
	var isCaptchaValid = true;
	// avoid to execute the actual submit of the form.
	if (jQuery(".sitecore-form form .has-error").length > 1) {
		return false;
	}
	//Console.log(grecaptcha.getResponse().length);
	$.each($(this).serializeArray(), function () { if (this.name == "g-recaptcha-response" && this.value == "") { isCaptchaValid = false; } });

	if (isCaptchaValid == false) {
		return false;
	}

	jQuery(".sitecore-form form .text-box").not(':button,:hidden').each(function () {
		var requiredFieldMessage = $(this).attr("data-val-required");
		if (requiredFieldMessage != undefined && requiredFieldMessage.length > 0 && this.value == '') {
			out = false;
			return false;
		}
	});

	if (out == false) {
		return false;
	}

	var form = $(this).serializeArray();
	var opco = "/" + window.location.pathname.split("/")[1];

	var url = opco + "/api/RPFormData";
	var urlFormDataSave = opco + "/api/RPFormDataSave";
	var language = window.location.pathname.split("/")[2];
	var obj = { "strings": form, "url": url, "token": "abdc1234", "language": language };
	$.ajax({
		type: "POST",
		dataType: 'json',
		url: url,
		contentType: 'application/json', // Not to set any content header
		data: JSON.stringify(obj),

		success: function (data) {
			console.log(data);
			console.log("This is success");

			if (!(data.ReturnToUrl === "")) {
				window.location.href = data.ReturnToUrl;
			}
			else if (!(data.SummaryText === "")) {
				jQuery(".sitecore-form").html('')
				jQuery(".sitecore-form").html(data.SummaryText)
			}
		},
		error: function (data) {
			console.log('An error occurred.');
		},
	});

	e.preventDefault();

});

jQuery(".promotion-form form").submit(function (e) {
	var out = true;
	var isCaptchaValid = true;
	// avoid to execute the actual submit of the form.
	if (jQuery(".promotion-form form .has-error").length > 1) {
		return false;
	}
	//Console.log(grecaptcha.getResponse().length);
	$.each($(this).serializeArray(), function () { if (this.name == "g-recaptcha-response" && this.value == "") { isCaptchaValid = false; } });

	if (isCaptchaValid == false) {
		return false;
	}

	jQuery(".promotion-form form .text-box").not(':button,:hidden').each(function () {
		var requiredFieldMessage = $(this).attr("data-val-required");
		if (requiredFieldMessage != undefined && requiredFieldMessage.length > 0 && this.value == '') {
			out = false;
			return false;
		}
	});

	if (out == false) {
		return false;
	}

	var form = $(this).serializeArray();
	var opco = "/" + window.location.pathname.split("/")[1];

	var url = opco + "/api/RPFormData";
	var language = window.location.pathname.split("/")[2];
	var obj = { "strings": form, "url": url, "token": "abdc1234", "language": language };
	$.ajax({
		type: "POST",
		dataType: 'json',
		url: url,
		contentType: 'application/json', // Not to set any content header
		data: JSON.stringify(obj),

		success: function (data) {
			console.log(data);
			console.log("This is success");

			if (!(data.ReturnToUrl === "")) {
				window.location.href = data.ReturnToUrl;
			}
			else if (!(data.SummaryText === "")) {
				jQuery(".promotion-form").html('')
				jQuery(".promotion-form").html(data.SummaryText)
			}
		},
		error: function (data) {
			console.log('An error occurred.');
		},
	});
	e.preventDefault();

});