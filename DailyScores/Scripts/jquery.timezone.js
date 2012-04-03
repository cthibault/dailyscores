$(document).ready(function () {
    var storedTimeZoneOffset = $.cookie('TimeZoneOffset'),
        currentTimeZoneOffset = (new Date().getTimezoneOffset() * -1).toString();
    if (storedTimeZoneOffset === null || storedTimeZoneOffset !== currentTimeZoneOffset) {
        $.cookie('TimeZoneOffset', currentTimeZoneOffset);
        document.location.reload(true);
    }
});