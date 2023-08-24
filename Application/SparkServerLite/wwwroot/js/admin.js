(function () {

    window.SparkServerAdmin = window.SparkServerAdmin || {

        settings: {

            siteURL: ""

        },

        endpoints: {

            blogMedia: "/api/blogmedia"

        },

        loadBlogMediaList: async function () {

            // TODO: show spinner

            // Get BlogID from page
            let blogID = document.getElementById("ID").value;

            // Get media items
            let response = await fetch(`${SparkServerAdmin.endpoints.blogMedia}?blogID=${blogID}`);
            let result = await response.json();

            if (result.status == "OK") {

                // Build unordered list from data
                let output = "<ul>";

                result.data.map((element) => {

                    output += `<li><img src='${element.thumbnailPath}' />${element.filename}</li>`;

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

            // TODO: hide spinner
        }


    };




})();