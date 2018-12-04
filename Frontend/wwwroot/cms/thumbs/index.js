function buildDial(thumb) {
    var chartHolder = buildElement("div", "thumb-dial")
    var chartData = google.visualization.arrayToDataTable(thumb.chartData)
    var chart = new google.visualization.Gauge(chartHolder)
    chart.draw(chartData, thumb.options)
}

function buildDialCard(thumb) {
    return buildElement("div", "card",
        buildElement("div", "card-header",
            buildElement("h5", undefined, thumb.title)
        ),
        buildElement("div", "card-block",
            buildDial(thumb)
        )
    )
}

/*
function loadThumbsData() {
    return $.getJSON("https://apps.guidance.prod.dop.corp.hmrc.gov.uk/thumbs") 
}
*/

function loadThumbsData(path) {
    // TODO: Stop assuming one top level organisation
    return $.Deferred().resolve({
        parent: null,
        title: "HMRC",
        level: "organisation",
        thumbs: {
            up: 10,
            down: 4
        },
        children: [{
            path: "/HMRC/CCG",
            parent: "HMRC",
            title: "CCG",
            level: "group",
            thumbs: {
                up: 1,
                down: 0
            }
        }, {
            title: "CSG",
            level: "group",
            thumbs: {
                up: 9,
                down: 3
            }
        }]
    })
}

/*

    ccg/tax90001
    csg/tag90001

    handleRoute(url)

        parts = url.path.split("/")

        parts = ["HMRC", "CCG", ]

        
        SELECT 
        (SELECT count(*) WHERE rating = true) as up,
        (select count(*) WHERE rating = false) as down FROM SELECT rating FROM thumbs 
           WHERE organisation = ? AND group = ? AND lob = ? AND process = ? 

        SELECT * FROM thumbs WHERE organisation = ? AND group = ? AND lob = ? <- per process 

        SELECT * FROM thumbs WHERE organisation = ? AND group = ? <- per lob 

        SELECT * FROM thumbs WHERE organisation = ? <- per group 




*/
function buildThumbDisplay(thumb) {
    var display = buildElement("div", "d-flex ")


    var parentDisplay = buildDialCard(thumb);
    var childrenDisplay = buildFragment();
    var children = thumb.children;
    for (i = 0; i < children.length; i += 1) {
        childrenDisplay.appendChild(buildDialCard(children[i]))
    }
}


function updateDisplay() {
    loadThumbsData()
        .done(function (json) {
            var
        })
}