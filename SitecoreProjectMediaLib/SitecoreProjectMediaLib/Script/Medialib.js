﻿$(document).ready(function () {
    $('#btnUpload').click(function () {

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object
           

            $.ajax({
                url: 'api/Sitecore/MediaLibController/DetachMedia',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (result) {
                    alert("Excelupload complete");
                },
                error: function (err) {
                    alert(err.statusText);
                }
});
} else {
    alert("FormData is not supported.");
}
});
});