var capbreak = capbreak || {};
capbreak.geo = {};

capbreak.geo.kmToMi = function (km) {
    return km * 0.62137;
};

capbreak.geo.kmToNmi = function (km) {
    return (km / 1.852);
};

capbreak.geo.distanceBetweenLatLon = function (lat1, lon1, lat2, lon2) {
    var R = 6371; // Radius of the earth in km
    var dLat = capbreak.sci.degToRad(lat2 - lat1);
    var dLon = capbreak.sci.degToRad(lon2 - lon1);
    var a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(capbreak.sci.degToRad(lat1)) * Math.cos(capbreak.sci.degToRad(lat2)) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c; // Distance in km

    return d;
}