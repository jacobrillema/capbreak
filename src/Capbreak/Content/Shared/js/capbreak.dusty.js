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

// TODO handle no support for geolocation
capbreak.dusty.updateLocation = function (position) {
    capbreak.dusty.location = { latitude: position.coords.latitude, longitude: position.coords.longitude };
    $('#dusty-latlon').text(capbreak.dusty.location.latitude.toFixed(2) + ', ' + capbreak.dusty.location.longitude.toFixed(2));
    capbreak.dusty.log('Updated location');
};

// Events object and loop timer
capbreak.dusty.events = [];
capbreak.dusty.events.timer = setInterval(function () { capbreak.dusty.processEvents(); }, 1000);
capbreak.dusty.processEvents = function () {
    var now = new Date().getTime();

    // Check to see if radar needs updating
    if (capbreak.gmaps != null && capbreak.gmaps.activeNexradSite != null) {
        var site = capbreak.gmaps.activeNexradSite;
        var lastUpdate = site.lastUpdate;
        if (lastUpdate == null || ((now - lastUpdate) > 60000)) {
            displayRadar(site.name, 'n0r', new google.maps.LatLng(site.latitude, site.longitude));
        }
    }
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

    // ==TIMERS==
    // Clock timer
    setInterval(function () { updateClock(); }, 500);

    // Location timer
    setInterval(function () {
        navigator.geolocation.getCurrentPosition(
            capbreak.dusty.updateLocation, null, { enableHighAccuracy: true }
        );
    }, 60000);
    navigator.geolocation.getCurrentPosition(capbreak.dusty.updateLocation, null, { enableHighAccuracy: true } );

    // Bindings
    $(document).bind('keypress', function (e) {
        if (e.which == 96) {
            $('#dusty-console').slideToggle();
        }
    });

    $('#dusty-hamburger').click(function () {
        $('#dusty-console').slideToggle();
    });

    $('#dusty-console').click(function () {
        $('#dusty-console').slideToggle();
    });

    $('#dusty-natradar').click(function () {
        capbreak.gmaps.toggleNationalRadar();
    });

    $('#dusty-location').click(function () {
        capbreak.gmaps.toggleLocation();
    });

    $('#dusty-sites').click(function () {
        capbreak.gmaps.toggleNexradSites();
    });

    $('#dusty-metars').click(function () {
        capbreak.gmaps.toggleMetars();
    });

    google.maps.event.addDomListener(window, 'load', capbreak.gmaps.init);
})();