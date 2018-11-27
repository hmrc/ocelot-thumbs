/**
 * addThumbs adds a thumbs block to the DOM. If target is a string
 * it's used as a selector to find a DOM Element to attach to. Otherwise,
 * target should be a DOM Element.
 * 
 * @author Osric Wilkinson <osric.wilkinson@hmrc.gsi.gov.uk>
 * @param {string|HTMLElement} target The DOM element (or it's selector) to append to
 * @param {string} product The id of the product being rated
 * @param {?string} endPoint The specific part of the product being rated. Optional (defaults to "")
 */

function addThumbs(target, product, endPoint) {

    var thumbBlock; 
    var thumbsDelay = 15 * 1000; // 15 seconds
    var groupAPI = "http://localhost:5000/API/Groups/"
    var thumbsAPI = "http://localhost:5000/API/Thumbs/"

    /**
     * Build a DOM object
     * @param {string} tag The name of the tag to build
     * @param {object|string} attributes Either an object with key/value attribute pairs, or a string to use for the 'class' attribute
     * @param {?HTMLElement|string|number...} contents The objects to include in the element
     */
    function _buildElement(tag, attributes) {
        var i, k, v, arg;
        if (window.buildElement) {
            return buildElement.apply(null, arguments)
        }

        var el = document.createElement(tag);
        switch (typeof attributes) {
            case 'string':
                el.setAttribute("class", attributes);
                break;
            case 'object':
                for (k in attributes) {
                    if (attributes.hasOwnProperty(k)) {
                        v = attributes[k]
                        if (v !== undefined) {
                            el.setAttribute(k, v)
                        }
                    }
                }
        }

        for (i = 2; i < arguments.length; i += 1) {
            arg = arguments[i];
            switch (typeof arg) {
                case 'string':
                case 'number':
                case 'boolean':
                    el.appendChild(document.createTextNode(arg))
                    break;
                case 'object':
                    el.appendChild(arg)
                    break;
            }
        }

        return el;
    }

    /**
     * Build a font awesome DOM object
     * @param {string} name
     * @return {HTMLElement}
     */
    function _buildFA(name) {
        return _buildElement("i", {
            class: "fa fa-fw fa-" + name,
            "aria-hidden": true
        })
    }

    /**
     * Post thumb to the backend API
     * @param {string} data A JSON.stringify encoded object
     * @returns {jquery.Deferred}
     */
    function _postThumb(data) {
        return $.ajax({
            contentType: "application/json",
            data: data,
            method: "POST",
            url: thumbsAPI,
            xhrFields: {
                withCredentials: true
            }
        })
    }

    /**
     * Get the content owner from the backend API
     * @param {string} product The product id that will be saved to the DB
     * @returns {jquery.Deferred} 
     */
    function _getContentOwner(product) {
        return $.ajax({
            //TODO: Check docs for the API
        })
    }

    /**
     * Build a DOM object that will update itself to show the name of the
     * product owner.
     * @param {string} product 
     * @return {HTMLElement}
     */
    function _buildProductOwner(product) {
        var div = _buildElement("div", {
            id: "thumbs-product-owner",
            class: "text-primary text-center"
        })
        _getContentOwner(product)
            .done(function (json) {
                //TODO: Check docs for the API
                var owner = json.something

                if (owner !== undefined) {
                    div.appendChild(_buildElement("strong", undefined, owner + "'s team created this guidance. Was it helpful?"))
                }
            })
        return div;
    }

    /**
     * Build a DOM object button with an up thumb or down thumb
     * @param {boolean} dir true = up thumb, false = down thumb
     * @return {HTMLElement}
     */
    function _buildButton(dir) {
        var dirName = (dir ? "up" : "down")

        return _buildElement("button", {
                class: "btn btn-primary thumbs-button",
                "data-dir": dirName
            },
            _buildFA("thumbs-o-" + dirName)
        )
    }

    /**
     * Build a DOM object that shows the thumbs up/down buttons
     * @param {string} product the product to submit the thumb for
     * @return {HTMLElement}
     */
    function _buildThumbs(product) {
        return buildElement("div", {
                class: "card-footer",
                id: "thumbsArea"
            },
            _buildProductOwner(product),
            _buildElement("div", "d-flex justify-content-center",
                _buildButton(true),
                _buildButton(false)
            )
        )
    }

    /**
     * Get the target we're appending to.
     * @param {HTMLElement|string} target 
     * @returns {HTMLElement}
     */
    function _getTarget(target) {
        if (target instanceof HTMLElement) {
            return target
        } else {
            return document.querySelector(target)
        }
    }

    /**
     * Tell the user that their thumb has been recorded
     */
    function _notifySuccess() {
        $("#thumbsArea")
            .empty()
            .text("Thank you for your feedback.")
    }

    /**
     * Tell the user there was a problem
     * @param jqXHR 
     */
    function _notifyFailure(jqXHR) {
        $("#thumbsArea")
            .empty()
            .text("Sorry, there was a problem recording your feedback.")
    }

    /**
     * Disable thumbs buttons
     */
    function _disableButtons() {
        $(thumbBlock).find(".thumbs-button").prop("disabled", true)
    }

    /**
     * Wait for thumbsDelay and then hide the thumbs area
     */
    function _hideThumbs() {
        setTimeout(function () {
            $(thumbBlock).slideUp()
        }, thumbsDelay)
    }

    /**
     * Handle thumb clicks. Called by jquery, so 'this' is
     * the button that was clicked.
     */
    function _handleClick() {
        var dirName = this.dataset.dir

        var thumbData = {
            product: product,
            group: "//todo: get the group",
            endPoint: endPoint !== undefined ? endPoint : "",
            rating: dirName === "up"
        }

        _disableButtons()

        _postThumb(JSON.stringify(thumbData))
            .done(_notifySuccess)
            .fail(_notifyFailure)
            .always(_hideThumbs)
    }

    // The actual code for the outer function
    thumbBlock = _buildThumbs(product);
    $(thumbBlock).on("click", ".thumbs-button", _handleClick)
    _getTarget(target).appendChild(thumbBlock);
}