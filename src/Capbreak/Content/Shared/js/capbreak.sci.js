var capbreak = capbreak || {};
capbreak.sci = {};

capbreak.sci.degToRad = function (deg) {
    return deg * (Math.PI / 180)
};

capbreak.sci.cToF = function (c, round) {
    var f = c * (9 / 5) + 32;

    return (round) ? Math.round(f) : f;
}

capbreak.sci.fToC = function (f, round) {
    var c = (f - 32) * (5 / 9);

    return (round) ? Math.round(c) : c;
}