function process() {
    const file = document.querySelector("#upload").files[0];

    if (!file) return;

    const reader = new FileReader();

    reader.readAsDataURL(file);

    reader.onload = function (event) {
        const imgElement = document.createElement("img");
        imgElement.src = event.target.result;
        //document.querySelector("#input").src = event.target.result;

        imgElement.onload = function (e) {
            const canvas = document.createElement("canvas");
            const MAX_WIDTH = 1000;

            const scaleSize = MAX_WIDTH / e.target.width;
            canvas.width = MAX_WIDTH;
            canvas.height = e.target.height * scaleSize;

            const ctx = canvas.getContext("2d");

            ctx.drawImage(e.target, 0, 0, canvas.width, canvas.height);

            const srcEncoded = ctx.canvas.toDataURL(
                'image/jpeg',
                0.8
            );

            // you can send srcEncoded to the server
            document.querySelector("#output").src = srcEncoded;
            document.querySelector("#base64Image").value = srcEncoded.split(',')[1];
        };
    };


}


function processall() {
    const files = document.querySelector("#uploades").files;

    if (!files.length === 0) return;
    if (files.length > 20) return;
    // Clear previous outputs

    const outputContainer = document.querySelector("#outputes");

    const base64Images = document.querySelector("#base64Images");

    outputContainer.innerHTML = ''; // Clear previous images

    base64Images.value = ''; // Clear previous base64 data


    Array.from(files).forEach(file => {

        const reader = new FileReader();


        reader.readAsDataURL(file);


        reader.onload = function (event) {

            const imgElement = document.createElement("img");

            imgElement.src = event.target.result;


            imgElement.onload = function (e) {

                const canvas = document.createElement("canvas");

                const MAX_WIDTH = 600;


                const scaleSize = MAX_WIDTH / e.target.width;

                canvas.width = MAX_WIDTH;

                canvas.height = e.target.height * scaleSize;


                const ctx = canvas.getContext("2d");

                ctx.drawImage(e.target, 0, 0, canvas.width, canvas.height);


                const srcEncoded = ctx.canvas.toDataURL(
                    'image/jpeg',
                    0.8
                );


                // Append each resized image to the output container

                const resizedImg = document.createElement("img");

                resizedImg.src = srcEncoded;

                outputContainer.appendChild(resizedImg);


                // Accumulate base64 strings

                base64Images.value += srcEncoded.split(',')[1] + '\n'; // Separate by newline

            };

        };

    });


}
