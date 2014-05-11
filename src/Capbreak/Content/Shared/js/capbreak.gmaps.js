var capbreak = capbreak || {};
capbreak.gmaps = {};
capbreak.gmaps.markerSize = { x: 22, y: 40 };
capbreak.gmaps.map = null;
capbreak.gmaps.markers = [];
capbreak.gmaps.lastZoom = 6;
capbreak.gmaps.displayRadar = false;
capbreak.gmaps.radarZoomLimit = 9;
capbreak.gmaps.activeNexradSite = null;

capbreak.gmaps.displayPolygon = function (type) {
    var endpoint = '/wx/nexrad/warningsfeed?type=' + type;

    $.getJSON(endpoint, function (data) {
        if (data == null || data[0] == null || data[0].Polygons == null || data[0].Polygons.length < 1) { return; }

        $.each(data[0].Polygons, function () {
            var polycoords = [];
            $.each(this, function () {
                polycoords.push(new google.maps.LatLng(this.X, this.Y));
            });

            var polygon = new google.maps.Polygon({
                paths: polycoords,
                strokeColor: '#FF0000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FF0000',
                fillOpacity: 0.0,
                zIndex: 10,
                optimized: false
            });

            polygon.setMap(capbreak.gmaps.map);
        });
    });
};

capbreak.gmaps.addMarker = function (latlon, label, zindex) {
    var marker = new google.maps.Marker({
        position: latlon,
        label: label,
        map: capbreak.gmaps.map,
        icon: '/Content/Wx/Nexrad/dot.png',
        optimized: false,
        zIndex: zindex
    });

    if (label != 'html-anchor') {
        google.maps.event.addListener(marker, 'click', function () {
            capbreak.gmaps.addMarker(latlon, 'html-anchor', 1);
            capbreak.gmaps.displayRadar = true;
            // TODO need to redo all of this and get it in one place
            capbreak.dusty.log('Fetching product N0R for ' + label + '...');
            displayRadar(label, 'n0r', latlon);
            capbreak.gmaps.activeNexradSite = capbreak.nexrad.sites[label.toLowerCase()];
            capbreak.gmaps.activeNexradSite.lastUpdate = new Date().getTime();
        });
    }

    capbreak.gmaps.nexradSiteMarkers.push(marker);
};

// TODO load sites async - only do once a month, save to localstorage
capbreak.gmaps.nexradSiteMarkers = [];
capbreak.gmaps.toggleNexradSites = function () {
    if (capbreak.gmaps.nexradSiteMarkers == null || !capbreak.gmaps.nexradSiteMarkers.length) {
        capbreak.dusty.log('Loading WSR-88D sites');
        $.each(sitelist, function () {
            var latlon = new google.maps.LatLng(this.Latitude, this.Longitude);
            capbreak.gmaps.addMarker(latlon, this.Name, 2);
        });
    }
    else {
        var markers = capbreak.gmaps.nexradSiteMarkers;
        if (markers[0].map == null) {
            $.each(markers, function () {
                this.setMap(capbreak.gmaps.map);
            });
        }
        else {
            $.each(markers, function () {
                this.setMap(null);
            });
        }
    }
};

capbreak.gmaps.locationMarker = null;
capbreak.gmaps.toggleLocation = function () {
    if (capbreak.gmaps.locationMarker == null) {
        var latlon = new google.maps.LatLng(capbreak.dusty.location.latitude, capbreak.dusty.location.longitude);
        capbreak.gmaps.locationMarker = new google.maps.Marker({
            position: latlon,
            icon: '/Content/Shared/images/location-icon.png'
        });
    }

    if (capbreak.gmaps.locationMarker.map == null) {
        capbreak.dusty.log('Displaying location');
        capbreak.gmaps.locationMarker.setMap(capbreak.gmaps.map);
    }
    else {
        capbreak.dusty.log('Removing location');
        capbreak.gmaps.locationMarker.setMap(null);
    }
};
capbreak.gmaps.updateLocation = function () {
    if (capbreak.gmaps.locationMarker != null) {
        var latlon = new google.maps.LatLng(capbreak.dusty.location.latitude, capbreak.dusty.location.longitude);
        capbreak.gmaps.locationMarker.position = latlon;

        // TODO check to see if this actually updates or is even needed
        if (capbreak.gmaps.locationMarker.map != null) {
            capbreak.gmaps.locationMarker.setMap(map);
        }
    }
};

capbreak.gmaps.nationalRadarOverlay = null;
capbreak.gmaps.toggleNationalRadar = function () {
    if (capbreak.gmaps.nationalRadarOverlay == null) {
        var imgurl = 'http://radar.weather.gov/ridge/Conus/RadarImg/latest_radaronly.gif';
        var imgbounds = new google.maps.LatLngBounds(
            new google.maps.LatLng(21.652538062803, -127.620375523875420),
            new google.maps.LatLng(50.406626367301044, -66.517937876818)
        );
        capbreak.gmaps.nationalRadarOverlay = new google.maps.GroundOverlay(imgurl, imgbounds);
    }
    
    if (capbreak.gmaps.nationalRadarOverlay.map == null) {
        capbreak.dusty.log('Displaying national radar');
        capbreak.gmaps.nationalRadarOverlay.setMap(capbreak.gmaps.map);
    }
    else {
        capbreak.dusty.log('Removing national radar');
        capbreak.gmaps.nationalRadarOverlay.setMap(null);
    }
};

capbreak.gmaps.metarMarkers = [];
capbreak.gmaps.toggleMetars = function () {
    if (capbreak.gmaps.metarMarkers == null || !capbreak.gmaps.metarMarkers.length) {
        var endpoint = '/nexrad/metarfeed?state=mn';
        capbreak.dusty.log('Loading METARs');
        $.getJSON(endpoint, function (data) {
            if (data == null || data.data == null || !data.data.METAR.length) { return; }
            $.each(data.data.METAR, function () {
                var latlon = new google.maps.LatLng(this.lat, this.lon);
                var marker = new google.maps.Marker({
                    position: latlon,
                    map: capbreak.gmaps.map,
                    icon: '/content/shared/images/1x1.png',
                    label: String.format('<span class="temp">{0}</span><span class="dewp">{1}</span>', Math.round(capbreak.sci.cToF(this.temp)), Math.round(capbreak.sci.cToF(this.dewp)))
                });
                capbreak.gmaps.metarMarkers.push(marker);
            });
        });
    }
    else {
        var markers = capbreak.gmaps.metarMarkers;
        if (markers[0].map == null) {
            $.each(markers, function () {
                this.setMap(capbreak.gmaps.map);
            });
        }
        else {
            $.each(markers, function () {
                this.setMap(null);
            });
        }
    }
};

capbreak.gmaps.init = function () {
    capbreak.dusty.log('Initializing Google maps');
    var myLatlng = new google.maps.LatLng(40, -97);
    var mapOptions = { zoom: capbreak.gmaps.lastZoom, center: myLatlng };
    capbreak.gmaps.map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    google.maps.event.addListener(capbreak.gmaps.map, 'zoom_changed', function (test) {
        var zoomLevel = capbreak.gmaps.map.getZoom();

        if (capbreak.gmaps.displayRadar) {
            // Browsers like to crash with massive canvases, so zoom with CSS at close ranges
            // TODO look into splitting canvases

            if (zoomLevel == capbreak.gmaps.radarZoomLimit && (zoomLevel < capbreak.gmaps.lastZoom)) { capbreak.dusty.log('Reverting back to pure rendering mode for radar'); }
            if (zoomLevel == (capbreak.gmaps.radarZoomLimit + 1) && (zoomLevel > capbreak.gmaps.lastZoom)) { capbreak.dusty.log('Switching to CSS zoom mode for radar'); }

            if (zoomLevel <= capbreak.gmaps.radarZoomLimit) {
                renderData();
                $('canvas#radar').css('width', 'auto');
                $('canvas#radar').css('height', 'auto');
            }
            else {
                // Use CSS to zoom
                var radar = $('canvas#radar');
                var width = radar.width();
                var height = radar.height();
                if (zoomLevel > capbreak.gmaps.lastZoom) {
                    // Zoom in
                    radar.width(width * 2);
                    radar.height(height * 2);
                }
                else {
                    // Zoom out
                    radar.width(width / 2);
                    radar.height(height / 2);
                }

                radar.css('left', radar.width() / -2);
                radar.css('top', radar.height() / -2);
            }

            capbreak.gmaps.lastZoom = zoomLevel;
        }
    });
};

// Extending Google Maps
(function () {
    google.maps.Marker.prototype.setLabel = function (label) {
        this.label = new MarkerLabel({
            map: capbreak.gmaps.map,
            marker: this,
            text: label
        });
        this.label.bindTo('position', this, 'position');
    };
    var MarkerLabel = function (options) {
        this.setValues(options);

        if (options.text == 'html-anchor') {
            $('#html-anchor').remove();
            this.div = document.createElement('div');
            this.div.id = 'html-anchor';
            this.div.className = 'map-marker-html';
        }
        else {
            this.span = document.createElement('span');
            this.span.className = 'map-marker-label';
        }
    };
    MarkerLabel.prototype = $.extend(new google.maps.OverlayView(), {
        onAdd: function () {
            if (this.get('text') == 'html-anchor') {
                this.getPanes().overlayImage.appendChild(this.div);
            }
            else {
                this.getPanes().overlayImage.appendChild(this.span);
            }
            var self = this;
            this.listeners = [google.maps.event.addListener(this, 'position_changed', function () { self.draw(); })];
        },
        draw: function () {
            var text = String(this.get('text'));
            var position = this.getProjection().fromLatLngToDivPixel(this.get('position'));
            if (this.get('text') == 'html-anchor') {
                this.div.style.left = position.x + 'px';
                this.div.style.top = position.y + 'px';
            }
            else {
                this.span.innerHTML = text;
                //this.span.style.left = (position.x - (capbreak.gmaps.markerSize.x / 2)) - (text.length * 3) + 10 + 'px';
                //this.span.style.top = (position.y - capbreak.gmaps.markerSize.y + 40) + 'px';
                this.span.style.left = position.x + 'px';
                this.span.style.top = position.y + 'px';
            }
        }
    });
})();