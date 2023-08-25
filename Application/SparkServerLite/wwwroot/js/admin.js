(function () {

    window.SparkServerAdmin = window.SparkServerAdmin || {

        endpoints: {

            blogMedia: "/api/blogmedia",
            uploadMedia: "/api/uploadmedia"

        },

        init: function () {

            document.getElementById("UploadMediaFiles").addEventListener("click", SparkServerAdmin.handleMediaUpload);

        },

        loadBlogMediaList: async function () {

            SparkServerAdmin.showLoader("BlogMediaListLoader");

            // Get BlogID from page
            let blogID = document.getElementById("ID").value;

            // Get media items
            let response = await fetch(`${SparkServerAdmin.endpoints.blogMedia}?blogID=${blogID}`);
            let result = await response.json();

            if (result.status == "OK") {

                // Build unordered list from data
                let output = "<ul>";

                result.data.map((element) => {

                    output += `
                        <li>
                            <img src='${element.thumbnailPath}' />${element.filename}
                        </li>`;

                });

                output += "</ul>";

                document.getElementById("BlogMediaList").innerHTML = output;

            } else if (result.status == "ERROR") {

                alert(result.message);

            } else if (result.status == "EXCEPTION") {

                alert("EXCEPTION! See browser console for details.");

            }

            if (result.message)
                console.log(result.message);

            SparkServerAdmin.hideLoader("BlogMediaListLoader");
        },

        handleMediaUpload: function (e) {

            e.preventDefault();
            SparkServerAdmin.disableButton("UploadMediaFiles");

            var fileInput = document.getElementById("MediaFiles");

            if (fileInput.files.length === 0) {
                alert("Please add media to upload.");
                SparkServerAdmin.enableButton("UploadMediaFiles");
                return;
            }

            let formData = new FormData();
            formData.append("blogID", document.getElementById("ID").value);

            // Media upload
            for (var i = 0; i < fileInput.files.length; i++) {
                formData.append(fileInput.files[i].name, fileInput.files[i]);
            }

            // TODO: show local spinner + message

            fetch(SparkServerAdmin.endpoints.uploadMedia, { method: "POST", body: formData })
                .then(response => response.json())
                .then(data => SparkServerAdmin.handleMediaUploadResponse(data));

        },

        handleMediaUploadResponse: function () {

            // SparkServerAdmin.hideLoader();
            SparkServerAdmin.enableButton("UploadMediaFiles");

            if (data.Status === "ERROR") {
                alert(`Error!\n\n${data.Message}`);
            }

            if (data.Status === "EXCEPTION") {
                alert(`EXCEPTION!\n\n${data.Message}`);
            }

            if (data.Status === "SUCCESS") {

                // TODO: show success message

                // Refresh media list
                SparkServerAdmin.loadBlogMediaList();

            }

        },

        showLoader: (id) => {

            document.getElementById(id).style.display = "block";

        },

        hideLoader: (id) => {

            document.getElementById(id).style.display = "none";

        },

        disableButton: (id) => {

            document.getElementById(id).disabled = true;

        },

        enableButton: (id) => {

            document.getElementById(id).disabled = false;

        }


    };

    // Entry point
    SparkServerAdmin.init();

})();