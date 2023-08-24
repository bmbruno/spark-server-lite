(function () {

    window.SparkServerAdmin = window.SparkServerAdmin || {

        settings: {

            siteURL: ""

        },

        endpoints: {

            blogMedia: "/api/blogmedia"

        },

        loadBlogMediaList: async function () {

            // Get BlogID from page
            let blogID = document.getElementById("ID");

            // Call API endpoint to get media items
            let response = await fetch(`${SparkServerAdmin.endpoints.blogMedia}?blogID=${blogID}`);
            let data = await response.json();

            // Build unordered list from data

        }


    };




})();