// Copy a target to the clipboard

function CopyTarget(container) {
    this.container = container
    this.target = this.container.dataset.copyTarget
    this.targetElement = document.getElementById(this.target)
    this.accessibleLabel = this.container.dataset.accessibleLabel
}

CopyTarget.prototype.init = function() {
    if (!this.targetElement) {
        return
    }
    this.createLink()
}

CopyTarget.prototype.createLink = function() {
    var link = document.createElement('a')
        link.className = "app-copy-button app-api-info__button";
        link.innerHTML = this.accessibleLabel ? 'Copy key<span class="govuk-visually-hidden"> for ' + this.accessibleLabel + '</span>' : 'Copy key'
        link.href = "#"
        link.addEventListener("click", this.copyTargetAction.bind(this))
    this.link = link
    this.container.appendChild(link)
}

CopyTarget.prototype.copyTargetAction = function(e) {
    var textToCopy = this.targetElement.innerText
    var that = this
    navigator.clipboard.writeText(textToCopy).then(function() {
        that.link.innerText = "Key copied"
    }, function() {
        that.container.innerText = "Failed"
    });
    e.preventDefault()
}

// nodeListForEach

function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}


// Application

var copyTargets = document.querySelectorAll('[data-copy-target]');

nodeListForEach(copyTargets, function (copyTargets) {
  new CopyTarget(copyTargets).init();
});