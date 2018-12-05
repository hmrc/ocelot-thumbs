"use strict";

var BASE_URL = "https://apps.guidance.prod.dop.corp.hmrc.gov.uk/Thumbs/Report/"

function pad2(num) {
    if (num < 10) { 
        return "0" + num;
    } else {
        return "" + num;
    }
}

function fakeRequest() {
    return $.Deferred().resolve({"product" : "HMRC","count" : 40,"up" : 10,"down" : 30,"percent" : 25.00 ,"children":[{"product" : "CSG","count" : 16,"up" : 6,"down" : 10,"percent" : 37.50 ,"children":[]},{"product" : "CDIO","count" : 24,"up" : 4,"down" : 20,"percent" : 16.66 ,"children":[]}]})
}

function formatISO(date) {
    return date.getUTCFullYear() + "-" +
        pad2(date.getUTCMonth() + 1) + "-" +
        pad2(date.getUTCDate()) + "T" +
        pad2(date.getUTCHours()) + ":" +
        pad2(date.getUTCMinutes()) + ":" + 
        pad2(date.getUTCSeconds()) + "Z"
}

function parseISO(string) {
    var m = string.match(/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})Z$/)
    
    if (m) {
        m.shift()
        m = m.map(function (x) { return parseInt(x, 10) });
        
        return new Date(Date.UTC(m[0], m[1] - 1, m[2], m[3], m[4], m[5], 0));
    } else {
        throw new Error("'" + string + "' is not an ISO date")
    }
}

function getMidnight(date) {
    var result = new Date(date)
    result.setUTCHours(0, 0, 0, 0)
    return result;
}

function getData(product, startDate, endDate) {
    console.log("Fetching ", product, startDate, endDate)
    return $.getJSON(BASE_URL + product, {startDate: formatISO(startDate), endDate: formatISO(endDate)})
   //return fakeRequest()
}

function getStartOfWeek(date) {
    var result = new Date(date);
    while (result.getDay() !== 1) {
        result.setDate(result.getDate() - 1)
    }
    return result;
}

function addWeeks(weeks, date) {
    var then = new Date(date)
    then.setDate(then.getDate() + weeks * 7)
    return then
}

function getThisWeek(product, when) {
    return getData(product, when, addWeeks(1, when))
}

function getLast8Weeks(product, when) {    
    return getData(product, addWeeks(-8, when), addWeeks(1, when))
}

function getThumbsData(product, when) {    
    return $.when(
        getThisWeek(product, when),
        getLast8Weeks(product, when)
    )
}

function buildWeekUrl(product, when) {
    return "?p=" + product + "&w=" + formatISO(when)
}

function calcPercent(thumb) {
    if (thumb.up + thumb.down === 0) {
        return 50
    }
    
    var pct = thumb.up / (thumb.up + thumb.down) * 100
    return Math.round(pct * 10)/10
}

function buildDial(thumb) {    
    var div = buildElement("div", {style: "width: 165px; height: 165px;"})
    google.charts.setOnLoadCallback(function () {
        var data = google.visualization.arrayToDataTable([
            ['Label', 'Value'],
            ['%', calcPercent(thumb)]
        ])
        
        var options = {
            width: 165,
            height: 165,
            min: 0, max: 100,
            redFrom: 0, redTo: 50,
            yellowFrom: 50, yellowTo: 80,
            greenFrom: 80, greenTo: 100,
            minorTicks: 5
        }

        var chart = new google.visualization.Gauge(div)    
        chart.draw(data, options)
    })
    return div
}

function buildColumns(thisWeek, last8) {
    var div = buildElement("div", {style: "height: 314px;"})
    google.charts.setOnLoadCallback(function () {
        var i, table, options, chart, childThisWeek, childLast8;
        
        table = [
            ["", 'This week', 'Last 8 weeks']        
        ];
        for (i = 0; i < thisWeek.children.length; i += 1) {
            childThisWeek = thisWeek.children[i];
            childLast8 = last8.children[i]
            
            table.push([
                childThisWeek.product,
                calcPercent(childThisWeek),
                calcPercent(childLast8)            
            ])
        }
        
        options = {
            legend: {
                position: 'top'
            },
            vAxis: {
                minValue: 0,
                maxValue: 100
            },
            height: 314
        }
        
        chart = new google.visualization.ColumnChart(div)
        chart.draw(google.visualization.arrayToDataTable(table), options)
    })
    return div;
}

function buildDialPanel(product, when, thisWeek, last8) {
    return buildElement("div", "card m-3", 
        buildElement("h5", "card-header text-center", 
            buildElement("a", {href:buildWeekUrl(product, when), class:"extLink"}, product)
        ),
        buildElement("div", "card-body d-flex justify-content-between",
            buildDial(thisWeek),
            buildDial(last8)        
        ),
        buildElement("table", "table table-hover table-sm mb-0",
            buildElement("thead", undefined, 
                buildElement("tr", undefined, 
                    buildElement("th", "text-center", "This Week"), 
                    buildElement("th", "text-center", "Last 8 weeks")
                )
            ),
            buildElement("tbody", undefined,
                buildElement("tr", "table-sucess",
                    buildElement("td", "text-center", thisWeek.up),
                    buildElement("td", "text-center", last8.up)                    
                ),
                buildElement("tr", "table-danger",
                    buildElement("td", "text-center", thisWeek.down),
                    buildElement("td", "text-center", last8.down)
                ),
                buildElement("tr", "table-info",
                    buildElement("td", "text-center", thisWeek.up + thisWeek.down),
                    buildElement("td", "text-center", last8.down + last8.down)
                )
            )
        )
    )
}

function buildColumnPanel(product, thisWeek, last8) {
    return buildElement("div", "card m-3",
        buildElement("h5", "card-header text-center", product),
        buildElement("div", "card-body", buildColumns(thisWeek,last8))
    )
}

function updateTitle(product, when) {
    $("#title").text("Thumbs Analysis for Week Commencing " + formatDate(when))
    
    $("#week-back").attr("href", buildWeekUrl(product, addWeeks(-1, when)))
    $("#week-next").attr("href", buildWeekUrl(product, addWeeks( 1, when)))
}

function showThumbs(product, when) {
    updateTitle(product, when)
    getThumbsData(product, when)
        .done(function (thisWeekXHR, last8XHR) {
            var i, childThisWeek, childLast8;
            var thisWeek = thisWeekXHR //[0]
            var last8 = last8XHR //[0]
            
            $("#overall").append(buildDialPanel(product, when, thisWeek, last8))
            $("#bars").append(buildColumnPanel(product, thisWeek, last8))
            
            for (i = 0; i < thisWeek.children.length; i += 1) {
                childThisWeek = thisWeek.children[i]
                childLast8 = last8.children[i]
                $("#hook").append(buildDialPanel(childThisWeek.product, when, childThisWeek, childLast8))
            }
        })
        .fail(function () {
            $("#hook").empty().append(buildElement("div", "alert alert-danger", "Sorry, there was a problem fetching the Thumbs data."))
        })
}

function getParam(raw) {
	raw = raw || window.location.search;
	var parts, tmp, key, value, i, param = {};

	if (raw === undefined || raw === "")
		return param;

	raw = raw.substring(1); // remove leading '?';

	parts = raw.split("&");

	for (i = 0; i < parts.length; i += 1) {
		tmp = parts[i].split("=");
		key = decodeURIComponent(tmp[0]);
		value = decodeURIComponent(tmp[1]);

		if (key in param) {
			if (typeof param[key] === "string") {
				param[key] = [param[key], value];
			} else {
				param[key].push(value);
			}
		} else {
			param[key] = value;
		}
	}

	return param;
}

$(function () {
    google.charts.load('current', {'packages':['gauge', 'corechart']});
        
    var when = getMidnight(getStartOfWeek(new Date())), product = "HMRC"
    var param = getParam();
    
    if ("p" in param) {
        product = param.p
    }

    if ("w" in param) {
        when = getMidnight(getStartOfWeek(parseISO(param.w)))
    }

    showThumbs(product, when)
})

