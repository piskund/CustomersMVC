// Change status of all checkboxes marked with class "checkboxes".
function toggleChecked(status) {
    $("#checkboxes input")
        .each(function() {
            // Set the checked status of each to match the 
            // checked status of the check all checkbox:
            $(this).prop("checked", status);
        });
}

// Set/unset checkboxes value base on master checkbox value.
$(document)
    .ready(function() {
        // Get a value of the selectAll box:
        var checkAllBox = $("#selectAll");

        //Set the default value of the global checkbox to false:
        checkAllBox.prop("checked", false);

        // Attach the call to toggleChecked to the
        // click event of the global checkbox:
        checkAllBox.click(function() {
            var status = checkAllBox.prop("checked");
            toggleChecked(status);
        });
    });

// Set/unset checkboxes value if "Select All" link clicked.
$(document)
    .ready(function() {
        $(".selectAllLink")
            .click(function() {
                var checkAllBox = $("#selectAll");
                var status = checkAllBox.prop("checked");
                checkAllBox.prop("checked", !status);
                toggleChecked(!status);
                return false;
            });
    });

$(document)
    .ready(function () {
        // Get a value of the selectAll box:
        var checkAllBox = $("#selectAll");

        //Set the default value of the global checkbox to false:
        checkAllBox.prop("checked", false);

        // Attach the call to toggleChecked to the
        // click event of the global checkbox:
        checkAllBox.click(function () {
            var status = checkAllBox.prop("checked");
            toggleChecked(status);
        });
    });