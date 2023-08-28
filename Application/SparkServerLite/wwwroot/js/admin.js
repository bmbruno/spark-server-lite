(function () {

    window.SparkServerAdmin = window.SparkServerAdmin || {

        endpoints: {

            blogMedia: "/api/blogmedia",
            uploadMedia: "/api/uploadmedia",
            deleteMedia: "/api/deletemedia"

        },

        init: function () {

            // TODO: move to own function
            document.getElementById("UploadMediaFiles").addEventListener("click", SparkServerAdmin.handleMediaUpload);

            SparkServerAdmin.wireDeleteConfirmButtons();

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
                            <div class="image-container">
                                <img src='${element.thumbnailPath}' />
                            </div>
                            <div class="text-container">
                                <p>${element.filename}</p>
                                <div class='media-url'>${element.webPath}</div>
                                <button type="button" class="media-copyurl-button" data-url="${element.webPath}">Copy URL</button>
                                <button type="button" class="media-delete-button delete-confirm" data-filename="${element.filename}">Delete</button>
                            </div>
                        </li>`;

                });

                output += "</ul>";

                document.getElementById("BlogMediaList").innerHTML = output;

                SparkServerAdmin.wireMediaButtons();

            } else if (result.status == "ERROR") {

                alert(result.message);

            } else if (result.status == "EXCEPTION") {

                alert("EXCEPTION! See browser console for details.");

            }

            if (result.message)
                console.log(result.message);

            SparkServerAdmin.hideLoader("BlogMediaListLoader");
        },

        wireMediaButtons: function () {

            // Delete buttons
            let deleteButtons = document.querySelectorAll(".media-delete-button");

            if (deleteButtons) {

                deleteButtons.forEach((button) => {

                    button.addEventListener("click", function (e) {

                        e.preventDefault();

                        let blogID = document.getElementById("ID").value;
                        let filename = button.attributes["data-filename"].value;

                        SparkServerAdmin.deleteMedia(blogID, filename);

                    });

                });
            }

            // CopyURL buttons
            let copyButtons = document.querySelectorAll(".media-copyurl-button");

            if (copyButtons) {

                copyButtons.forEach((button) => {

                    button.addEventListener("click", function (e) {

                        e.preventDefault();
                        let attrUrl = button.attributes["data-url"];

                        if (attrUrl) {
                            navigator.clipboard.writeText(attrUrl.value);
                        }

                    });

                });
            }
        },

        wireDeleteConfirmButtons: function () {

            let deleteButtons = document.querySelectorAll(".delete-confirm")

            if (deleteButtons) {

                deleteButtons.forEach((button) => {

                    button.addEventListener("click", function (e) {

                        if (!confirm("Are you should you want to delete this?")) {
                            return false;
                        }
                    });
                });
            }
        },

        deleteMedia: async function (blogID, filename) {

            alert(`[deleteMedia] blogID: ${blogID} filename: ${filename}`);

            // TODO: send delete GET
            let formData = new FormData();
            formData.append("blogID", blogID);
            formData.append("filename", filename);
            let response = await fetch(SparkServerAdmin.endpoints.deleteMedia, { method: "POST", body: formData });
            let result = await response.json();

            if (response.status == "OK") {

                alert(response.message);

            } else if (result.status == "ERROR") {

                alert(result.message);

            } else if (result.status == "EXCEPTION") {

                alert("EXCEPTION! See browser console for details.");

            }

            // TODO: refresh media list
            SparkServerAdmin.loadBlogMediaList();

        },

        handleMediaUpload: function (e) {

            e.preventDefault();
            SparkServerAdmin.disableButton("UploadMediaFiles");
            SparkServerAdmin.showLoader("MediaFilesUploadMessage");

            var fileInput = document.getElementById("MediaFiles");

            if (fileInput.files.length === 0) {
                alert("Please add media to upload.");
                SparkServerAdmin.enableButton("UploadMediaFiles");
                SparkServerAdmin.hideLoader("MediaFilesUploadMessage");
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

        handleMediaUploadResponse: function (data) {

            // SparkServerAdmin.hideLoader();
            SparkServerAdmin.enableButton("UploadMediaFiles");
            SparkServerAdmin.hideLoader("MediaFilesUploadMessage");

            if (data.status === "ERROR") {
                alert(`Error!\n\n${data.Message}`);
            }

            if (data.status === "EXCEPTION") {
                alert(`EXCEPTION!\n\n${data.Message}`);
            }

            if (data.status === "OK") {

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