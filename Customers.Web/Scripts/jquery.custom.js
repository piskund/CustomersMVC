function toggleChecked(status) {
    $("#checkboxes input").each(function () {
        // Set the checked status of each to match the 
        // checked status of the check all checkbox:
        $(this).prop("checked", status);
    });
}

$(document).ready(function () {
    // Get a value of the selectAll box:
    var checkAllBox = $("#selectAll");

    //Set the default value of the global checkbox to false:
    checkAllBox.prop('checked', false);

    // Attach the call to toggleChecked to the
    // click event of the global checkbox:
    checkAllBox.click(function () {
        var status = checkAllBox.prop('checked');
        toggleChecked(status);
    });
});

$(document).ready(function () {
    var link = document.getElementById("selectAllLink");

    // Attach selection of all checkboxes to hyperlink click
    link.onclick = function () {
        var checkAllBox = $("#selectAll");
        var status = checkAllBox.prop('checked');
        checkAllBox.prop("checked", !status);
        toggleChecked(!status);
        return false;
    }
});