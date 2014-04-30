var capbreak = capbreak || {};
capbreak.dusty = {};
capbreak.dusty.location = {};

// Exsposed functions
capbreak.dusty.timestampUtc = function () {
    return moment.utc().format('MM-DD HH:mm') + 'Z';
};

capbreak.dusty.log = function (msg) {
    $('#dusty-console ol').append('<li>[' + capbreak.dusty.timestampUtc() + '] ' + msg + '</li>');
};

// TODO handle no support
capbreak.dusty.updateLocation = function (position) {
    capbreak.dusty.location = { latitude: position.coords.latitude, longitude: position.coords.longitude };
    $('#dusty-latlon').text(capbreak.dusty.location.latitude + ', ' + capbreak.dusty.location.longitude);
};

(function () {
    // Private functions
    function updateClock() {
        var time = moment.utc().format('HH:mm') + 'Z';
        $('#dusty-clock').text(time);
    }

    // Document manipulation
    var screenWidth = $(window).width();
    var screenHeight = $(window).height();
    $('body')
        .css('width', screenWidth)
        .css('height', screenHeight);

    // Object creation
    // Clock timer
    setInterval(updateClock(), 500);

    // Location timer
    setInterval(function () {
        navigator.geolocation.getCurrentPosition(
            capbreak.dusty.updateLocation, null, { enableHighAccuracy: true }
        );
    }, 60000);
    navigator.geolocation.getCurrentPosition(capbreak.dusty.updateLocation, null, { enableHighAccuracy: true } );

    // Binding
    $(document).bind('keypress', function (e) {
        if (e.which == 96) {
            $('#dusty-console').slideToggle();
        }
    });

    google.maps.event.addDomListener(window, 'load', capbreak.gmaps.init);
})();