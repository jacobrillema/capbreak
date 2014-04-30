var capbreak = capbreak || {};
capbreak.gmaps = {};

capbreak.gmaps.markerSize = { x: 22, y: 40 };
capbreak.gmaps.map = null;
capbreak.gmaps.markers = [];
capbreak.gmaps.lastZoom = 6;
capbreak.gmaps.displayRadar = false;
capbreak.gmaps.radarZoomLimit = 9;

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
        map: capbreak.gmaps.map,
        label: label,
        icon: '/Content/Wx/Nexrad/dot.png',
        optimized: false,
        zIndex: zindex
    });

    if (label != 'html-anchor') {
        google.maps.event.addListener(marker, 'click', function () {
            capbreak.gmaps.addMarker(latlon, 'html-anchor', 1);
            capbreak.gmaps.displayRadar = true;
            capbreak.dusty.log('Fetching product N0R for ' + label + '...');
            displayRadar(label, 'n0r', latlon);
            //drawCircle(latlon.k, latlon.A);
        });
    }

    capbreak.gmaps.markers.push(marker);
};

capbreak.gmaps.setAllMap = function () {
    for (var i = 0; i < capbreak.gmaps.markers.length; i++) {
        capbreak.gmaps.markers[i].setMap(capbreak.gmaps.map);
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

    capbreak.dusty.log('Loading WSR-88D sites');
    $.each(sitelist, function () {
        var latlon = new google.maps.LatLng(this.Latitude, this.Longitude);
        capbreak.gmaps.addMarker(latlon, this.Name, 2);
    });
};

(function () {
    google.maps.Marker.prototype.setLabel = function (label) {
        this.label = new MarkerLabel({
            map: this.map,
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
                this.span.style.left = (position.x - (capbreak.gmaps.markerSize.x / 2)) - (text.length * 3) + 10 + 'px';
                this.span.style.top = (position.y - capbreak.gmaps.markerSize.y + 40) + 'px';
            }
        }
    });
})();