$(document).ready(function() {
    var storedTimeZoneOffset = $.cookie('TimeZoneOffset'),
        currentTimeZoneOffset = (new Date()).getTimezoneOffset().toString();
    if (storedTimeZoneOffset === null || storedTimeZoneOffset !== currentTimeZoneOffset) {
        $.cookie('TimeZoneOffset', currentTimeZoneOffset);
        document.location.reload(true);
    }
});