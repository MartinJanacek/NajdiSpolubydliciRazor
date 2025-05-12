// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let numberOfImages = [...document.querySelectorAll(`[data-otherimageslabel]`)].length;

function auto_grow(element) {
    element.style.height = "5px";
    element.style.height = (element.scrollHeight) + "px";
}

function removeSpaces(string) {
    return string.split(' ').join('');
}

function read_URL(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();

        if (input.getAttribute("data-alreadyinputed") == "") {
            numberOfImages += 1;

            document.getElementById('forminputlayout').insertAdjacentHTML('beforeend',
                `<div>
                    <label data-otherimageslabel>${numberOfImages}.</label>
                    <input type="file" name="Photos" id="Photos" class="image-input" data-alreadyInputed="" onchange="read_URL(this)" accept="image/jpg, image/jpeg, image/png" />
                    <img alt="your image" src=""/>
                    <button type="button" onclick="remove_image_container(this)" class="remove">Odstranit</button>
                    <span asp-validation-for="Photos"></span>
                </div>`
            )
        }

        let randomId = crypto.randomUUID();
        input.setAttribute("data-alreadyinputed", randomId);
        
        reader.onload = function (e) {
            document.querySelector(`[data-alreadyinputed="${randomId}"]`)
                .nextElementSibling.setAttribute('src', e.target.result);

            document.querySelector(`[data-alreadyinputed="${randomId}"]`)
                .nextElementSibling.style.display = "block";

            document.querySelector(`[data-alreadyinputed="${randomId}"]`)
                .nextElementSibling.nextElementSibling.style.display = "block";
        };

        reader.readAsDataURL(input.files[0]);
    }
    else {
        if (input.id === 'ProfilePhoto') {
            document.getElementById('ProfilePhoto').nextElementSibling.style.display = "none";
        }
        else {
            remove_image_container(input)
        } 
    }
}

function remove_image_container(button)
{
    numberOfImages--;
    button.parentNode.remove();

    otherImagesLabels = [...document.querySelectorAll(`[data-otherimageslabel]`)];

    for (var i = 0; i < otherImagesLabels.length; i++) {
        otherImagesLabels[i].innerHTML = (i + 1) + ".";
    }
}

function update_image(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();

        if (input.getAttribute("data-alreadyinputed") == "") {
            numberOfImages += 1;

            document.getElementById('forminputlayout').insertAdjacentHTML('beforeend',
                `<div>
                    <label data-otherimageslabel>${numberOfImages}. - Změněno</label>
                    <input type="file" name="Photos" id="Photos" class="image-input" data-alreadyInputed="" onchange="read_URL(this)" accept="image/jpeg, image/jpg, image/png" />
                    <img alt="your image" src=""/>
                    <button type="button" onclick="remove_image_container(this)" class="remove">Odstranit</button>
                    <span asp-validation-for="Photos"></span>
                </div>`
            )
        }

        document.getElementById('imageinfo')
            .insertAdjacentHTML('afterbegin', `<input name="ChangedImages" id="ChangedImages" value=${input.getAttribute("data-imagenumber")} />`);

        let randomId = crypto.randomUUID();
        input.setAttribute("data-alreadyinputed", randomId);

        reader.onload = function (e) {
            document.querySelector(`[data-alreadyinputed="${randomId}"]`)
                .nextElementSibling.setAttribute('src', e.target.result);

            document.querySelector(`[data-alreadyinputed="${randomId}"]`)
                .nextElementSibling.style.display = "block";

            document.querySelector(`[data-alreadyinputed="${randomId}"]`)
                .nextElementSibling.nextElementSibling.style.display = "block";
        };

        reader.readAsDataURL(input.files[0]);
    }
    else {
        if (input.id === 'ProfilePhoto') {
            document.getElementById('ProfilePhoto')
                .nextElementSibling.style.display = "none";
        }
        else {
            remove_existingimage_container(input)
        }
    }
}

function remove_existingimage_container(button) {
    document.getElementById('imageinfo')
        .insertAdjacentHTML('afterbegin', `<input name="DeletedImages" id="DeletedImages" value=${button.getAttribute("data-imagenumber")} />`);

    numberOfImages--;
    button.parentNode.remove()

    otherImagesLabels = [...document.querySelectorAll(`[data-otherimageslabel]`)];

    if (otherImagesLabels.length < 1)
    {
        numberOfImages += 1;

        document.getElementById('forminputlayout').insertAdjacentHTML('beforeend',
            `<div>
                    <label data-otherimageslabel>${numberOfImages}. - Změněno</label>
                    <input type="file" name="Photos" id="Photos" class="image-input" data-alreadyInputed="" onchange="read_URL(this)" accept="image/jpeg, image/jpg, image/png" />
                    <img alt="your image" src=""/>
                    <button type="button" onclick="remove_image_container(this)" class="remove">Odstranit</button>
                    <span asp-validation-for="Photos"></span>
                </div>`
        );
    }

    for (var i = 0; i < otherImagesLabels.length; i++) {
        otherImagesLabels[i].innerHTML = (i + 1) + ".";
    }
}