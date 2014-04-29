/* General info
##  Level 3 Implementation: http://www.roc.noaa.gov/wsr88d/PublicDocs/ICDs/2620001T.pdf
##  Radial Data: Page 3-113; Figure 3-10
##  NWS Data Management: http://www.nws.noaa.gov/datamgmt/filstnd.html#ds
##  PHP NEXRAD DECODER: https://github.com/johncharrell/WSR-88D/blob/master/classes/NexradDecoder.php
*/

/*
    Actual Level-3 Base Reflectivity 124 nmi Range Product Code <br>
    AWIPS HEADER CODE: N*R <br>
    PRODUCT CODE: 19
    public final static int L3PC_BASE_REFLECTIVITY_124NM = 19;
*/

//var infoCanvas = document.getElementById('info');
//var infoContext = infoCanvas.getContext('2d');
// TODO optimization:
// 1. 192027 -> 179423 filesize when reducing property names
// 2. Average Render Time @ 960x960 for all !ND positions: 127ms
// 3. > 1: 90ms (~22k polygons), > 2: 62ms, > 3: 42ms
// Hide/show canvas performance: http://jsperf.com/jquery-show-hide-vs-css-display-none-block/3
// Bitwise operators on the lineTo methods is good for 10% performance gain: http://jsperf.com/jquery-show-hide-vs-css-display-none-block/3
// Unfortunately there are slight gaps in the data when rendered ^
var mouseMove = false;
var mouseEvent = null;
var mouseElement = null;
var badAngleDeltaCount = 0;
var width = 460;
var height = 460;
var zoom = 1;
var polygonReduce = true;	// Enabling this cuts render time by ~100%, and has no adverse display impact
var transparency = false;	// Enabling this causes a 50% performance hit
var groupRendering = true;	// Enabling this cuts render time by 25-33%, and has no adverse display impact
var colorTable = [ '#000', '#00EAEC', '#01A0F6', '#0000F6', '#00FF00', '#00C800', '#009000', '#FFFF00', '#E7C000', '#FF9000', '#FF0000', '#D60000', '#C00000', '#FF00FF', '#9955C9', '#FFF' ];
//var nexradScans = [nx0, nx1, nx2, nx3, nx4, nx5, nx6, nx7, nx8, nx9, nx10, nx11, nx12];
var totalRenderTime = 0;
var polygonGroups = [];
var radarRadius = 0;
var currentScan = null;

//for (var i = 0; i < 13; i++) {
//	$('body').append(
//		$('<canvas/>', { id: 'nx' + i, style: 'display: none;' }).attr('width', width).attr('height', height)
//	);
	
//	// Hack to help remove smoothing from lines - sub pixel rendering is expensive - does this actually impact anything?
//	/*$('canvas').each(function() {
//		this.translate(0.5, 0.5);
//	});*/
	
//	var tmpContext = $('#nx' + i).get(0).getContext('2d');
//	var renders = renderNexrad(tmpContext, nexradScans[i]);
//	console.log(renders);
//}

/* Bin spacing details
if (header.getProductCode() == NexradHeader.L3PC_TDWR_LONG_RANGE_BASE_REFLECTIVITY_8BIT) {
            binSpacing = 300;
        }
        else if (header.getProductCode() == NexradHeader.L3PC_TDWR_BASE_REFLECTIVITY_8BIT ||
                header.getProductCode() == NexradHeader.L3PC_TDWR_BASE_VELOCITY_8BIT) {
            binSpacing = 150;
        }
        else if (header.getProductCode() == NexradHeader.L3PC_LONG_RANGE_BASE_VELOCITY_8BIT ||
        		header.getProductCode() == NexradHeader.L3PC_DIGITAL_DIFFERENTIAL_REFLECTIVITY ||
        		header.getProductCode() == NexradHeader.L3PC_DIGITAL_CORRELATION_COEFFICIENT ||
        		header.getProductCode() == NexradHeader.L3PC_DIGITAL_SPECIFIC_DIFFERENTIAL_PHASE ||
        		header.getProductCode() == NexradHeader.L3PC_DIGITAL_ONE_HOUR_ACCUMULATION ||
        		header.getProductCode() == NexradHeader.L3PC_DIGITAL_HYDROMETEOR_CLASSIFICATION ||
        		header.getProductCode() == NexradHeader.L3PC_DIGITAL_HYBRID_HYDROMETEOR_CLASSIFICATION) {
            binSpacing = 250;
        }
        else if (header.getProductCode() == NexradHeader.L3PC_ENHANCED_ECHO_TOPS ||
                header.getProductCode() == NexradHeader.L3PC_DIGITAL_VERT_INT_LIQUID) {
            binSpacing = 1000;
        }*/

function displayRadar(site, product, latlon) {
    // http://capbreak.com/wx/nexrad/latest?site=kmpx&product=n0r
    var endpoint = '/wx/nexrad/latest?site=' + site + '&product=' + product;

    if (!$('canvas#radar').length) {
        $('body').append($('<canvas/>', { id: 'radar' }));
    }

    $.getJSON(endpoint, function (data) {
        currentScan = data;
        renderData(currentScan, latlon);
    });
}

function renderData(scan, latlon) {
    scan = scan || currentScan;
    calculateBounds(latlon);
    var radar = $('canvas#radar');
    var context = radar.get(0).getContext('2d');
    renderNexrad(context, currentScan);
    var anchor = $('#html-anchor');
    anchor.append(radar);
    radar.css('left', radar.width() / -2);
    radar.css('top', (radar.height() / -2) - 4);  // 5px adjustment for icon/origin
}

// Ba lat
// ra lon
// 
function getBoundsInNmi() {
    var bounds = map.getBounds();

    // draw polygon around bounds
    /*var triangleCoords = [
    new google.maps.LatLng(bounds.Ba.j, bounds.ra.j),
    new google.maps.LatLng(bounds.Ba.j, bounds.ra.k),
    new google.maps.LatLng(bounds.Ba.k, bounds.ra.k),
    new google.maps.LatLng(bounds.Ba.k, bounds.ra.j),
    new google.maps.LatLng(bounds.Ba.j, bounds.ra.j)
    ];

    // Construct the polygon for testing
    var polygon = new google.maps.Polygon({
        paths: triangleCoords,
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35
    });

    polygon.setMap(map);*/

    // Go back to normal stuff for this function
    var km = getDistanceFromLatLonInKm(bounds.Ba.j, bounds.ra.j, bounds.Ba.k, bounds.ra.k);

    return convertKmToNmi(km);
}

function calculateBounds(latlon) {
    // Center the map
    //debugger;
    map.setCenter(latlon);

    var mapcanvas = $('#map-canvas');
    var bounds = map.getBounds();
    var pxheight = mapcanvas.height();
    var pxwidth = mapcanvas.width();
    var pxdiagonal = Math.sqrt(Math.pow(pxwidth, 2) + Math.pow(pxheight, 2));
    var nmidiagonal = convertKmToNmi(getDistanceFromLatLonInKm(bounds.Ba.j, bounds.ra.j, bounds.Ba.k, bounds.ra.k));
    var nmitop = convertKmToNmi(getDistanceFromLatLonInKm(bounds.Ba.j, bounds.ra.j, bounds.Ba.j, bounds.ra.k));
    var nmibottom = convertKmToNmi(getDistanceFromLatLonInKm(bounds.Ba.k, bounds.ra.j, bounds.Ba.k, bounds.ra.k));
    var nmileft = convertKmToNmi(getDistanceFromLatLonInKm(bounds.Ba.j, bounds.ra.j, bounds.Ba.k, bounds.ra.j));
    var nmiright = convertKmToNmi(getDistanceFromLatLonInKm(bounds.Ba.j, bounds.ra.k, bounds.Ba.k, bounds.ra.k));

    var mppdiagonal = nmidiagonal / pxdiagonal;
    var mpptop = nmitop / pxwidth;
    var mppbottom = nmibottom / pxwidth;
    var mppleft = nmileft / pxheight;
    var mppright = nmiright / pxheight;

    console.log('diagonal: ' + mppdiagonal);
    console.log('top: ' + mpptop);
    console.log('bottom: ' + mppbottom);
    console.log('left: ' + mppleft);
    console.log('right: ' + mppright);

    // TODO set all this once up higher
    var milesPerPixel = mppdiagonal;

    // Change canvas width and height to appropriate mpp - hardcoded to 230 miles at the moment, but why this value?
    var rangeMi = 248;
    var canvasDiameter = rangeMi / milesPerPixel;
    $('canvas#radar').attr('width', canvasDiameter);
    $('canvas#radar').attr('height', canvasDiameter);
    $('#radarbox').css('width', canvasDiameter);
    $('#radarbox').css('height', canvasDiameter);
    width = canvasDiameter;
    height = canvasDiameter;

    // A zoom of 1 works perfectly with 460px diameter / 230px radius - zoom should equal ((rangeMi / milesPerPixel) / 460)
    zoom = canvasDiameter / 460;
}

function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
    var R = 6371; // Radius of the earth in km
    var dLat = deg2rad(lat2 - lat1);  // deg2rad below
    var dLon = deg2rad(lon2 - lon1);
    var a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c; // Distance in km

    return d;
}

function convertKmToMi(km) {
    return km * 0.62137;
}

function convertKmToNmi(km) {
    return (km / 1.852);
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}

function drawCircle(lat, lon) {
    var options = {
        map: map,
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35,
        center: new google.maps.LatLng(lat, lon),
        radius: 229648  // 124 nmi to meters
    };

    var circle = new google.maps.Circle(options);

    options.radius = 199559;    // 124 mi to meters
    var circle2 = new google.maps.Circle(options);
}

//var currentScan = 0;
//setInterval(function() {
//	var previous = (currentScan > 0) ? currentScan - 1 : 12;
//	hideElement(document.getElementById('nx' + previous));
//	showElement(document.getElementById('nx' + currentScan));
//	currentScan = (currentScan == 12) ? 0 : currentScan + 1;
//}, 75);

function renderNexrad(context, data) {
	var now = Date.now();
	renderComplete = false;
	var polyCount = 0;
	context.clearRect(0, 0, width, height);  // TODO change to actual size
	
	for (var i = 0; i < data.Symbology.RadialData.length; i++) {
		var radial = data.Symbology.RadialData[i];
		var radialPosition = 0;
		var angleDelta = radial.AngleDelta || 1;	// Always assume 1 if it's 0
		var radialAngle = radial.StartAngle;

		//if (radialAngle != 330) { continue; }	// Isolating radials for testing
		//if (angleDelta == 0) { console.log('Bad angle delta for radial: ' + radialAngle); }
		
		for (var j = 0; j < radial.ColorValues.length; j++) {
			var colorValue = radial.ColorValues[j];
			// Don't render 0 because it's no data in the case of reflectivity, don't render 1 for testing
			if (colorValue > 0)
			{
				var polygon = [];

				polygon.push([
					(Math.cos(radians(radialAngle - 90)) * radialPosition * zoom) + (width / 2.0),
					(Math.sin(radians(radialAngle - 90)) * radialPosition * zoom) + (height / 2.0)
				]);
				
				polygon.push([
					(Math.cos(radians((radialAngle - 90) + angleDelta)) * radialPosition * zoom) + (width / 2.0),
					(Math.sin(radians((radialAngle - 90) + angleDelta)) * radialPosition * zoom) + (height / 2.0)
				]);
				
                // meow removed while testing
				//if (polygonReduce) {
				//	while (j < radial.ColorValues.length && (colorValue == radial.ColorValues[j + 1])) {
				//		radialPosition++;
				//		j++;
				//	}
				//}
				
				polygon.push([
					(Math.cos(radians((radialAngle - 90) + angleDelta)) * (radialPosition + 1) * zoom) + (width / 2.0),
					(Math.sin(radians((radialAngle - 90) + angleDelta)) * (radialPosition + 1) * zoom) + (height / 2.0)
				]);
				
				polygon.push([
					(Math.cos(radians(radialAngle - 90)) * (radialPosition + 1) * zoom) + (width / 2.0),
					(Math.sin(radians(radialAngle - 90)) * (radialPosition + 1) * zoom) + (height / 2.0)
				]);

				var color = null;
				if (transparency) {
					var alpha = (colorValue < 4) ? .2 : 1;
					var c = hexToRgb(colorTable[colorValue]);
					var color = 'rgba(' + c.r + ', ' + c.g + ', ' + c.b + ', ' + alpha + ')';
				}
				else {
					color = colorTable[colorValue];
				}
				
				if (groupRendering) {
					addPolygon(context, polygon, color);
				}
				else {
					drawPolygon(context, polygon, color);
				}
				
				polyCount++;
			}
			
			radialPosition++;
		}
	}
	
	if (groupRendering) { renderPolygons(context); }
	
	return { polyCount: polyCount, renderTimer: Date.now() - now };
}

function drawPolygon(context, polygon, color) {
	context.beginPath();
	context.moveTo(polygon[0][0], polygon[0][1]);
	
	for (var i = 1; i < polygon.length; i++) {
		context.lineTo(polygon[i][0], polygon[i][1]);
	}
	
	context.lineTo(polygon[0][0], polygon[0][1]);
	context.closePath();
	context.fillStyle = color;
	context.fill();
}

function addPolygon(context, polygon, color) {
	var groupNumber = null;
	
	for (var i = 0; i < polygonGroups.length; i++) {
		if (polygonGroups[i].name != null && polygonGroups[i].name == color) {
			groupNumber = i;
			break;
		}
	}
	
	if (groupNumber == null) {
		polygonGroups.push({ name: color, data: [] });
		groupNumber = polygonGroups.length - 1;
	}
	
	polygonGroups[groupNumber].data.push(polygon);
}

// 10% performance gain from using integer points, but can't figure out how to do it without getting artifacts in image, so not doing it for now
function renderPolygons(context) {
	for (var i = 0; i < polygonGroups.length; i++) {
		context.beginPath();
		var polygonGroup = polygonGroups[i];
		
		for (var j = 0; j < polygonGroup.data.length; j++) {
			var currentPolygon = polygonGroup.data[j];
			context.moveTo(currentPolygon[0][0], currentPolygon[0][1]);
			
			for (var k = 1; k < currentPolygon.length; k++) {
				context.lineTo(currentPolygon[k][0], currentPolygon[k][1]);
			}
			
			context.lineTo(currentPolygon[0][0], currentPolygon[0][1]);
		}
		
		context.closePath();
		context.fillStyle = polygonGroup.name;
		context.fill();
	}
	
	polygonGroups = [];
}

// Bin hover info
$('#canvas').mousemove(function(event) {
	mouseMove = true;
	mouseElement = this;
	mouseEvent = event;
});

/*(function() {
	if (mouseMove) {
		mouseMove = false;
		var pos = findPos(mouseElement);
		var x = mouseEvent.pageX - pos.x;
		var y = mouseEvent.pageY - pos.y;

		//x = 240;
		//y = 240;

		var opp = { x: x, y: y };
		var origin = { x: 480, y: 480 };
		var adj = { x: x, y: origin.y };
		opp.length = Math.abs(adj.y - opp.y);
		adj.length = Math.abs(adj.x - origin.x);
		var theta = Math.atan(opp.length / adj.length);
		var radial = 0;

		// Quadrant 1
		if ((x > width / 2) && (y < height / 2)) {
			radial = 90 - Math.ceil(degrees(theta));
		}
		// Quadrant 2
		else if ((x > width / 2) && (y > height / 2)) {
			radial = 90 + Math.ceil(degrees(theta));
		}
		// Quadrant 3
		else if ((x < width / 2) && (y > height / 2)) {
			radial = 270 - Math.ceil(degrees(theta));
		}
		// Quadrant 4
		else {
			radial = 270 + Math.ceil(degrees(theta));
		}

		var position = Math.floor((Math.sqrt(Math.pow(adj.length, 2) + Math.pow(opp.length, 2))) / 2);
		console.log(radial);
		console.log(position);
		var colorPosition = radar.Symbology.RadialData[radial].ColorValues[position] + 1;
		var dbz = radar.Descriptor['Threshold' + colorPosition];
		infoContext.clearRect(0, 0, width, height);
		infoContext.fillText(dbz + ' dBz', 100, 100);
	}
}, 100);*/

function findPos(obj) {
	var curleft = 0, curtop = 0;
	if (obj.offsetParent) {
		do {
			curleft += obj.offsetLeft;
			curtop += obj.offsetTop;
		} while (obj = obj.offsetParent);
		
		return { x: curleft, y: curtop };
	}
	
	return undefined;
}

function rgbToHex(r, g, b) {
	if (r > 255 || g > 255 || b > 255) { throw "Invalid color component"; }
	
	return ((r << 16) | (g << 8) | b).toString(16);
}