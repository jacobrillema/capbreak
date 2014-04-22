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
var zoom = 1;
var width = 480;
var height = 480;
var polygonReduce = true;	// Enabling this cuts render time by ~100%, and has no adverse display impact
var transparency = true;	// Enabling this causes a 50% performance hit
var groupRendering = true;	// Enabling this cuts render time by 25-33%, and has no adverse display impact
var colorTable = [ '#000', '#00EAEC', '#01A0F6', '#0000F6', '#00FF00', '#00C800', '#009000', '#FFFF00', '#E7C000', '#FF9000', '#FF0000', '#D60000', '#C00000', '#FF00FF', '#9955C9', '#FFF' ];
//var nexradScans = [nx0, nx1, nx2, nx3, nx4, nx5, nx6, nx7, nx8, nx9, nx10, nx11, nx12];
var totalRenderTime = 0;
var polygonGroups = [];

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

$('#nxfetch').click(function () {
    // http://capbreak.com/wx/nexrad/latest?site=kmpx&product=n0r
    var site = $('#nxsite').val();
    var product = $('#nxproduct').val();
    var endpoint = 'http://capbreak.com/wx/nexrad/latest?site=' + site + '&product=' + product;
    var context = $('canvas').get(0).getContext('2d');
    $.getJSON(endpoint, function (data) {
        renderNexrad(context, data);
    });
});

function displayRadar(site, product) {
    // http://capbreak.com/wx/nexrad/latest?site=kmpx&product=n0r
    //var endpoint = 'http://capbreak.com/wx/nexrad/latest?site=' + site + '&product=' + product;
    var endpoint = 'http://localhost:20685/wx/nexrad/latest?site=' + site + '&product=' + product;
    var context = $('canvas#radar').get(0).getContext('2d');
    $.getJSON(endpoint, function (data) {
        renderNexrad(context, data);
    });
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
	context.clearRect(0, 0, 480, 480);  // TODO change to actual size
	
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
				
				if (polygonReduce) {
					while (j < radial.ColorValues.length && (colorValue == radial.ColorValues[j + 1])) {
						radialPosition++;
						j++;
					}
				}
				
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