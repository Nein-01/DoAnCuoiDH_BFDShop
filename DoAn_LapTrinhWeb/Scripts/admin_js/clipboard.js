// Select elements
const target = document.getElementById('kt_clipboard_1');
const button = target.nextElementSibling;

// Init clipboard -- for more info, please read the offical documentation: https://clipboardjs.com/
var clipboard = new ClipboardJS(button, {
    target: target,
    text: function () {
        return target.value;
    }
});

// Success action handler
clipboard.on('success', function (e) {
    const currentLabel = button.innerHTML;

    // Exit label update when already in progress
    if (button.innerHTML === 'Copied!') {
        return;
    }

    // Update button label
    button.innerHTML = 'Copied!';

    // Revert button label after 3 seconds
    setTimeout(function () {
        button.innerHTML = currentLabel;
    }, 3000)
});