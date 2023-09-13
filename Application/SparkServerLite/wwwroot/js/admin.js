(function () {

    window.SparkServerAdmin = window.SparkServerAdmin || {

        endpoints: {

            blogMedia: "/api/blogmedia",
            uploadMedia: "/api/uploadmedia",
            deleteMedia: "/api/deletemedia",
            nextBlogBanner: "/api/getnextblogbanner",
            convertToHTML: "/api/markdowntohtml"

        },

        init: function () {

            SparkServerAdmin.wireButtons();

        },

        wireButtons: function () {

            // Convert button
            let covnertButton = document.getElementById("ConvertToHTML");
            if (covnertButton) {
                covnertButton.addEventListener("click", SparkServerAdmin.handleConvertButton);
            }

            // Upload Media button
            let uploadButton = document.getElementById("UploadMediaFiles");
            if (uploadButton) {
                uploadButton.addEventListener("click", SparkServerAdmin.handleMediaUpload);
            }

            // Today (current datetime)
            let todayButton = document.getElementById("TodayButton");
            if (todayButton) {
                todayButton.addEventListener("click", SparkServerAdmin.handleTodayButton);
            }

            // Create from title
            let createTitleButton = document.getElementById("CreateUniqueURL");
            if (createTitleButton) {
                createTitleButton.addEventListener("click", SparkServerAdmin.handleCreateURL);
            }

            // Get next default hero
            let getNextBannerButton = document.getElementById("GetNextBannerImage");
            if (getNextBannerButton) {
                getNextBannerButton.addEventListener("click", SparkServerAdmin.handleNextBanner);
            }

            // Clear tags
            let clearBlogTagsButton = document.getElementById("ClearBlogTags");
            if (clearBlogTagsButton) {
                clearBlogTagsButton.addEventListener("click", SparkServerAdmin.handleClearBlogTags);
            }

            // Modal close
            let closeModalButton = document.getElementById("CloseModal");
            if (closeModalButton) {
                closeModalButton.addEventListener("click", SparkServerAdmin.closeModal);
            }

            // Delete buttons
            SparkServerAdmin.wireDeleteConfirm();

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
                                <button type="button" class="media-copyurl-button" data-url="${element.thumbnailPath}">Copy Thumbnail</button>
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

                        if (!confirm("Are you should you want to delete this?")) {
                            e.preventDefault();
                            return false;
                        }

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

        wireDeleteConfirm: function () {

            let deleteButtons = document.querySelectorAll(".delete-confirm");
            if (deleteButtons) {

                deleteButtons.forEach((button) => {

                    button.addEventListener("click", function (e) {

                        if (!confirm("Are you should you want to delete this?")) {
                            e.preventDefault();
                            return false;
                        }
                    });
                });
            }

        },

        deleteMedia: async function (blogID, filename) {

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

            // Refresh media list
            SparkServerAdmin.loadBlogMediaList();

        },

        handleConvertButton: async function () {

            let field = document.getElementById("Markdown");

            if (field && field.value != '') {

                let formData = new FormData();

                formData.append("markdown", field.value);
                let response = await fetch(`${SparkServerAdmin.endpoints.convertToHTML}`, { method: "POST", body: formData });
                let result = await response.json();

                if (result.status == "OK") {

                    document.getElementById("Content").value = result.data;

                } else if (result.status == "ERROR") {

                    alert(result.message);

                } else if (result.status == "EXCEPTION") {

                    alert("EXCEPTION! See browser console for details.");

                }
            }

        },

        handleTodayButton: function () {

            let publishDateField = document.getElementById("PublishDate");
            if (publishDateField) {
                publishDateField.value = new Date().toLocaleDateString();
            }
        },

        handleCreateURL: function () {

            let titleField = document.getElementById("Title");

            if (titleField) {

                let title = titleField.value;
                title = title.split("-").join("");
                title = title.split("$").join("");
                title = title.split("%").join("");
                title = title.split(":").join("");
                title = title.split("(").join("");
                title = title.split(")").join("");
                title = title.split("'").join("");
                title = title.split("\"").join("");
                title = title.split(".").join("");
                title = title.split(",").join("");
                title = title.split("<").join("");
                title = title.split(">").join("");
                title = title.split("\\").join("");
                title = title.split("/").join("");
                title = title.split("?").join("");
                title = title.split("!").join("");
                title = title.split(" ").join("-");
                title = title.toLowerCase();

                document.getElementById("Slug").value = title;
            }

        },

        handleNextBanner: async function () {

            // TODO: show local spinner; disable button

            let response = await fetch(`${SparkServerAdmin.endpoints.nextBlogBanner}`);
            let result = await response.json();

            if (result.status == "OK") {

                document.getElementById("ImagePath").value = result.data[0];
                document.getElementById("ImageThumbnailPath").value = result.data[1];

            } else if (result.status == "ERROR") {

                alert(result.message);

            } else if (result.status == "EXCEPTION") {

                alert("EXCEPTION! See browser console for details.");

            }

            // TODO: hide local spinner; enable button

        },

        handleClearBlogTags: function () {

            let tags = document.querySelectorAll(".blogtag-checkbox");

            tags.forEach((tag) => {

                tag.checked = false;

            });

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

        openModal: function (title, body, full = false) {

            let modal = document.getElementById("Modal");

            if (modal) {

                modal.innerHTML = "";

                if (title)
                    modal.innerHTML += `<h2>${title}</h2>`;

                if (body)
                    modal.innerHTML += body;

                if (full)
                    modal.classList.add("full");

                modal.style.display = "block";

            } else {

                console.log("Modal not found on this page.");

            }

        },

        closeModal: function () {

            let modal = document.getElementById("Modal");

            if (modal) {
                modal.style.display = "none";
                modal.classList.remove("full");
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