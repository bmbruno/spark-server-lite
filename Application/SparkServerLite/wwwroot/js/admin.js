(function () {

    window.SparkServerAdmin = window.SparkServerAdmin || {

        endpoints: {

            blogMedia: "/api/blogmedia",
            uploadMedia: "/api/uploadmedia",
            deleteMedia: "/api/deletemedia",
            nextBlogBanner: "/api/getnextblogbanner",
            convertToHTML: "/api/markdowntohtml",
            libraryMedia: "/api/librarylist",
            uploadLibraryMedia: "/api/uploadlibrarymedia",
            deleteLibraryMedia: "/api/deletelibrarymedia"

        },

        init: function () {

            SparkServerAdmin.wireButtons();

            // The Date input type requires a certain 
            let publishDate = document.getElementById("PublishDate");
            if (publishDate && publishDate.defaultValue !== "")
                publishDate.value = SparkServerAdmin.formatDateForDatepicker(new Date(publishDate.defaultValue), true);

        },

        wireButtons: function () {

            // Convert button
            let convertButton = document.getElementById("ConvertToHTML");
            if (convertButton) {
                convertButton.addEventListener("click", SparkServerAdmin.handleConvertButton);
            }

            // Preview button
            let previewButton = document.getElementById("PreviewHTML");
            if (previewButton) {
                previewButton.addEventListener("click", SparkServerAdmin.handlePreviewButton);
            }

            // Upload Media button
            let uploadButton = document.getElementById("UploadMediaFiles");
            if (uploadButton) {
                uploadButton.addEventListener("click", SparkServerAdmin.handleMediaUpload);
            }

            // Upload Library Media button
            let uploadLibraryButton = document.getElementById("UploadLibraryMediaFiles");
            if (uploadLibraryButton) {
                uploadLibraryButton.addEventListener("click", SparkServerAdmin.handleLibraryMediaUpload);
            }

            // Today (current datetime)
            let todayButton = document.getElementById("TodayButton");
            if (todayButton) {
                todayButton.addEventListener("click", SparkServerAdmin.handleTodayButton);
            }

            // Create from title
            let createTitleButton = document.getElementById("CreateSlug");
            if (createTitleButton) {
                createTitleButton.addEventListener("click", SparkServerAdmin.handleCreateSlug);
            }

            // Get next default hero
            let getNextBannerButton = document.getElementById("GetNextBannerImage");
            if (getNextBannerButton) {
                getNextBannerButton.addEventListener("click", SparkServerAdmin.handleNextBanner);
            }
            
            // Select blog banner from library
            let selectBlogBannerButton = document.getElementById("SelectBlogBanner");
            if (selectBlogBannerButton) {
                selectBlogBannerButton.addEventListener("click", SparkServerAdmin.handleSelectBlogBanner)
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

            // Tooltips
            let tooltipIcons = document.querySelectorAll(".tooltip-icon");
            if (tooltipIcons) {
                tooltipIcons.forEach(icon => {
                    icon.addEventListener("click", (e) => { SparkServerAdmin.handleTooltipClick(e); });
                });
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
                                <button type="button" class="media-copy-button" data-copy="[![ALT](${element.thumbnailPath})](${element.webPath})" title="Copy Markdown image tag."><i class="fa fa-files-o" aria-hidden="true"></i> MD Token</button>
                                <button type="button" class="media-copy-button" data-copy="${element.webPath}" title="${element.webPath}"><i class="fa fa-files-o" aria-hidden="true"></i> URL</button>
                                <button type="button" class="media-copy-button" data-copy="${element.thumbnailPath}" title="${element.thumbnailPath}"><i class="fa fa-files-o" aria-hidden="true"></i> Thumbnail</button>
                                <button type="button" class="media-delete-button delete-confirm" data-filename="${element.filename}"><i class="fa fa-trash" aria-hidden="true"></i> Delete</button>
                            </div>
                        </li>`;
                        
                });

                output += "</ul>";

                document.getElementById("BlogMediaList").innerHTML = output;

                SparkServerAdmin.wireMediaButtons();

            } else if (result.status == "ERROR") {

                SparkServerAdmin.openModal("ERROR!", result.message);

            } else if (result.status == "EXCEPTION") {

                SparkServerAdmin.openModal("EXCEPTION!", result.message);

            }

            SparkServerAdmin.hideLoader("BlogMediaListLoader");
        },

        loadLibraryMediaList: async function () {

            SparkServerAdmin.showLoader("LibraryMediaListLoader");

            let response = await fetch(SparkServerAdmin.endpoints.libraryMedia);
            let result = await response.json();
            let output = "<ul>";

            if (result.status == "OK") {

                result.data.map((element) => {

                    output += `
                        <li>
                            <div class="image-container">
                                <img src='${element.thumbnailPath}' />
                            </div>
                            <div class="text-container">
                                <h3>${element.filename}</h3>
                                <div class='media-url'>${element.webPath}</div>
                                <button type="button" class="media-copy-button" data-url="${element.webPath}" title="${element.webPath}"><i class="fa fa-files-o" aria-hidden="true"></i> URL</button>
                                <button type="button" class="media-copy-button" data-url="${element.thumbnailPath}" title="${element.thumbnailPath}"><i class="fa fa-files-o" aria-hidden="true"></i> Thumbnail</button>
                                <button type="button" class="media-delete-button delete-confirm" data-filename="${element.filename}"><i class="fa fa-trash" aria-hidden="true"></i> Delete</button>
                            </div>
                        </li>`;

                });

                output += "</ul>";

                document.getElementById("LibraryMediaList").innerHTML = output;

                SparkServerAdmin.wireLibraryMediaButtons();

            } else if (result.status == "ERROR") {

                SparkServerAdmin.openModal("ERROR!", result.message);

            } else if (result.status == "EXCEPTION") {

                SparkServerAdmin.openModal("EXCEPTION!", result.message);

            }

            SparkServerAdmin.hideLoader("LibraryMediaListLoader");
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
            let copyButtons = document.querySelectorAll(".media-copy-button");

            if (copyButtons) {

                copyButtons.forEach((button) => {

                    button.addEventListener("click", function (e) {

                        e.preventDefault();
                        let attrUrl = button.attributes["data-copy"];

                        if (attrUrl) {
                            navigator.clipboard.writeText(attrUrl.value);
                            SparkServerAdmin.showToast("Copied to Clipboard", "Content has been copied to the clipboard!", 2, "success");
                        }

                    });

                });
            }
        },

        wireLibraryMediaButtons: function () {

            // Delete buttons
            let deleteButtons = document.querySelectorAll(".media-delete-button");

            if (deleteButtons) {

                deleteButtons.forEach((button) => {

                    button.addEventListener("click", function (e) {

                        if (!confirm("Are you should you want to delete this?")) {
                            e.preventDefault();
                            return false;
                        }

                        let filename = button.attributes["data-filename"].value;

                        SparkServerAdmin.deleteLibraryMedia(filename);

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
                            SparkServerAdmin.showToast("Copied to Clipboard", "URL fragment has been copied into the clipboard", 2, "success");
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

            if (result.status == "OK") {

                SparkServerAdmin.showToast("Media Deleted", "Media has been successfully deleted.", 6, "success");

            } else if (result.status == "ERROR") {

                SparkServerAdmin.openModal("ERROR!", result.message);

            } else if (result.status == "EXCEPTION") {

                SparkServerAdmin.openModal("EXCEPTION!", result.message);

            }

            // Refresh media list
            SparkServerAdmin.loadBlogMediaList();

        },

        deleteLibraryMedia: async function (filename) {

            let formData = new FormData();
            formData.append("filename", filename);

            let response = await fetch(SparkServerAdmin.endpoints.deleteLibraryMedia, { method: "POST", body: formData });
            let result = await response.json();

            if (result.status == "OK") {

                SparkServerAdmin.showToast("Image Deleted", "Library iamge has been successfully deleted.", 6, "success");

            } else if (result.status == "ERROR") {

                SparkServerAdmin.openModal("ERROR!", result.message);

            } else if (result.status == "EXCEPTION") {

                SparkServerAdmin.openModal("EXCEPTION!", result.message);

            }

            // Refresh media list
            SparkServerAdmin.loadLibraryMediaList();

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

                    SparkServerAdmin.openModal("ERROR!", result.message);

                } else if (result.status == "EXCEPTION") {

                    SparkServerAdmin.openModal("EXCEPTION!", result.message);

                }
            }

        },

        handlePreviewButton: async function () {

            let field = document.getElementById("Markdown");

            if (field && field.value != '') {

                let formData = new FormData();

                formData.append("markdown", field.value);
                let response = await fetch(`${SparkServerAdmin.endpoints.convertToHTML}`, { method: "POST", body: formData });
                let result = await response.json();

                if (result.status == "OK") {

                    let content = `<article class='blog'>${result.data}</article>`;
                    SparkServerAdmin.openModal(null, content, true);

                } else if (result.status == "ERROR") {

                    SparkServerAdmin.openModal("ERROR!", result.message);

                } else if (result.status == "EXCEPTION") {

                    SparkServerAdmin.openModal("EXCEPTION!", result.message);

                }
            }

        },

        handleTodayButton: function () {

            let publishDateField = document.getElementById("PublishDate");
            if (publishDateField) {
                publishDateField.value = SparkServerAdmin.formatDateForDatepicker(new Date(), false);
            }
        },

        handleCreateSlug: function () {

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

            let response = await fetch(`${SparkServerAdmin.endpoints.nextBlogBanner}`);
            let result = await response.json();

            if (result.status == "OK") {

                document.getElementById("ImagePath").value = result.data[0];
                document.getElementById("ImageThumbnailPath").value = result.data[1];

            } else if (result.status == "ERROR") {

                SparkServerAdmin.openModal("ERROR!", result.message);

            } else if (result.status == "EXCEPTION") {

                SparkServerAdmin.openModal("EXCEPTION!", result.message);

            }

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
                SparkServerAdmin.openModal("Alert!", "Please add media to upload.");

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

        handleLibraryMediaUpload: function (e) {

            e.preventDefault();
            SparkServerAdmin.disableButton("UploadLibraryMediaFiles");
            SparkServerAdmin.showLoader("LibraryUploadMessage");

            var fileInput = document.getElementById("LibraryMediaFiles");

            if (fileInput.files.length === 0) {
                SparkServerAdmin.openModal("Alert!", "Please add media to upload.");
                SparkServerAdmin.enableButton("UploadLibraryMediaFiles");
                SparkServerAdmin.hideLoader("LibraryUploadMessage");
                return;
            }

            let formData = new FormData();

            // Media upload
            for (var i = 0; i < fileInput.files.length; i++) {
                formData.append(fileInput.files[i].name, fileInput.files[i]);
            }

            fetch(SparkServerAdmin.endpoints.uploadLibraryMedia, { method: "POST", body: formData })
                .then(response => response.json())
                .then(data => SparkServerAdmin.handleLibraryMediaUploadResponse(data));

        },

        handleMediaUploadResponse: function (data) {

            // SparkServerAdmin.hideLoader();
            SparkServerAdmin.enableButton("UploadMediaFiles");
            SparkServerAdmin.hideLoader("MediaFilesUploadMessage");

            if (data.status === "ERROR") {

                SparkServerAdmin.showToast("Error!", data.message, 8, "error");
            }

            if (data.status === "EXCEPTION") {
                SparkServerAdmin.openModal("EXCEPTION!", data.message);
            }

            if (data.status === "OK") {

                SparkServerAdmin.showToast("Success!", "Media file(s) uploaded.", 8, "success");

                // Refresh media list
                SparkServerAdmin.loadBlogMediaList();

            }

        },

        handleLibraryMediaUploadResponse: function (data) {

            // SparkServerAdmin.hideLoader();
            SparkServerAdmin.enableButton("UploadLibraryMediaFiles");
            SparkServerAdmin.hideLoader("LibraryUploadMessage");

            if (data.status === "ERROR") {

                SparkServerAdmin.showToast("Error!", data.message, 8, "error");
            }

            if (data.status === "EXCEPTION") {
                SparkServerAdmin.openModal("EXCEPTION!", data.message);
            }

            if (data.status === "OK") {

                SparkServerAdmin.showToast("Success!", "Library file(s) uploaded.", 8, "success");

                // Refresh media list
                SparkServerAdmin.loadLibraryMediaList();

            }

        },

        handleSelectBlogBanner: async function () {
          
            // Add spinner to modal and show
            let modalBody = "<img src='/images/loader.gif' alt='Modal loading.' style='text-align: center;'/>";
            SparkServerAdmin.openModal("Select Blog Banner", modalBody, false);
            
            // Load library & build UI for selecting media

            let response = await fetch(SparkServerAdmin.endpoints.libraryMedia);
            let result = await response.json();
            let output = "<ul>";

            if (result.status === "OK") {

                result.data.map((element) => {

                    output += `
                        <li class="media-select">
                            <div class="image-container">
                                <img src='${element.thumbnailPath}' />
                            </div>
                            <div class="text-container">
                                <h3>${element.filename}</h3>
                                <button type="button" class="select-blog-banner" data-imageurl="${element.webPath}" data-thumburl="${element.thumbnailPath}">Use Image</button>
                            </div>
                        </li>`;

                });

                output += "</ul>";
            }
                
            // Update modal content with library list
            SparkServerAdmin.updateModal(output);

            // Wire-up events on new HTML
            let buttons = document.querySelectorAll(".select-blog-banner");

            if (buttons) {
                
                buttons.forEach((button) => {
                    
                    button.addEventListener("click", (e) => {
                        
                        // Image/Thumbnail is stored on the button itself
                        let imageURL = button.getAttribute("data-imageurl");
                        let thumbURL = button.getAttribute("data-thumburl");
    
                        document.getElementById("ImagePath").value = imageURL;
                        document.getElementById("ImageThumbnailPath").value = thumbURL;
    
                        SparkServerAdmin.closeModal();

                    })
                });
            }
        },

        handleTooltipClick: (e) => {

            let content = e.currentTarget.getAttribute("data-tooltip");
            
            if (content && content !== "")
                SparkServerAdmin.openModal(null, content, false);

        },

        openModal: function (title, body, full = false) {

            let modal = document.getElementById("Modal");
            let content = document.getElementById("ModalContent");

            if (modal && content) {

                content.innerHTML = "";

                if (title)
                    content.innerHTML += `<h2>${title}</h2>`;

                if (body)
                    content.innerHTML += body;

                if (full)
                    modal.classList.add("full");

                modal.style.top = (window.scrollY + (window.innerHeight * 0.05)) + "px";
                modal.style.display = "block";
                
                // Wire up escape key for close functionality
                document.body.addEventListener("keydown", SparkServerAdmin.handleModalKeydown);

            } else {

                console.log("Modal not found on this page.");

            }

        },
        
        updateModal: function (newContent) {

            let content = document.getElementById("ModalContent");
            
            if (content) {
                content.innerHTML = newContent;
            }
            
        },

        closeModal: function () {

            let modal = document.getElementById("Modal");
            let content = document.getElementById("ModalContent");

            if (modal && content) {
                modal.style.top = "";
                modal.style.display = "none";
                modal.classList.remove("full");
                content.innerHTML = "";
            }

        },
        
        handleModalKeydown: function (e) {
            
            if (e.key === "Escape") {
                SparkServerAdmin.closeModal();
                document.body.removeEventListener("keydown", SparkServerAdmin.handleModalKeydown);
            }
            
        },

        showToast: function (title, body, time = 6, type = "message") {

            let toast = document.getElementById("Toast");
            let content = document.getElementById("ToastContent");

            if (toast && content) {

                content.innerHTML = "";

                if (title)
                    content.innerHTML += `<h2>${title}</h2>`;

                if (body)
                    content.innerHTML += body;

                // "error", "success"
                toast.classList.add(type);
                toast.style.display = "block";

                // Start countdown to removal
                window.setTimeout(() => {
                                        
                    toast.style.display = "none";

                    // Reset class list (blank and set to 'toast')
                    toast.className = "";
                    toast.classList.add("toast");

                }, time * 1000);

                

            } else {

                console.log("Toast not found on this page.");

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

        },

        formatDateForDatepicker: (dateValue, includeTime) => {

            let month = (dateValue.getMonth() < 9) ? `0${dateValue.getMonth() + 1}` : `${dateValue.getMonth() + 1}`;
            let day = (dateValue.getDate() < 10) ? `0${dateValue.getDate()}` : `${dateValue.getDate()}`;
            let hours = "00";
            let minutes = "00";
            
            if (includeTime) {
                hours = dateValue.getHours() < 10 ? '0' + dateValue.getHours() : `${dateValue.getHours()}`
                minutes = dateValue.getMinutes() < 10 ? '0' + dateValue.getMinutes() : `${dateValue.getMinutes()}`;
            }

            return `${dateValue.getFullYear()}-${month}-${day}T${hours}:${minutes}`;

        }

    };

    // Entry point
    SparkServerAdmin.init();

})();