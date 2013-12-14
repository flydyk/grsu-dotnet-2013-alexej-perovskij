function drawcharts(url) {
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            url: url,
            cache: false,
            ifModified: true
        }).done(function (_data) {
            for (var i = 0; i < _data.length; i++) {
                var data = google.visualization.arrayToDataTable(_data[i]);
                var options = {
                    title: _data[i][0][1]
                };
                var chart = new google.visualization.PieChart(document.getElementById('piechart' + i));
                chart.draw(data, options);
            }
        });
    }
}