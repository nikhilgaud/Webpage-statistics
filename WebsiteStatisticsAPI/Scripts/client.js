
$(document).ready(function () {

    HideElements();
    $('#urlForm').on('submit', function (e)
    {
        //add textbox validation
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();

        $('#ImageCarousel').remove();
        $("#WordCount").remove();
        $("#errorMessage").removeClass("error").addClass("noerror");
            var webUrl = $('#urlText').val();

            var JSONObject = {
                WebUrl: webUrl
            };

            //AJAX call to the Controller
            $.ajax({
                type: 'POST',
                url: '/Home/GetImages',
                data: JSON.stringify(JSONObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.success) {
                        var obj = jQuery.parseJSON(data.ImageData);
                        console.log(obj);
                        //$('#ImageCaraousel').dis();
                        $('<div id="ImageCarousel" class="carousel slide"><div class="carousel-inner"></div></div >').appendTo('#divForImages');
                        //caraousel functionality- adding images dynamically
                        console.log(obj.Images);
                        for (var i = 0; i < obj.Images.length; i++) {
                            $('<div class="item"><img src="' + obj.Images[i] + '"></div>').appendTo('.carousel-inner');
                            //$('<li data-target="#ImageCaraousel" data-slide-to="' + i + '"></li>').appendTo('.carousel-indicators')
                        }
                        $('.item').first().addClass('active');
                        //$('.carousel-indicators > li').first().addClass('active');
                        $('#ImageCarousel').carousel({ interval: 2000 });


                        $('<p id="WordCount" class="lead">The total word count for this web page is:' + obj.TotalWordsCount + '</p>').appendTo('#divForWords');


                        //load the google graph -to show top 10 most frequently used words
                        google.charts.load('current', { packages: ['corechart'] });
                        google.charts.setOnLoadCallback(loadGraph);

                        function loadGraph() {

                            var data = [];
                            var chart;

                            data = new google.visualization.DataTable();
                            data.addColumn('string', 'Word');
                            data.addColumn('number', 'Occurence');


                            $.each(obj.WordFrequency, function (key, value) {
                                data.addRows([
                                    [key, value]
                                ]);
                            });
                            var options = {
                                'title': 'Top 10 most occuring words on webpage',
                                haxis: { title: 'Words' },
                                vaxis: { title: 'Occurence' }
                            };

                            chart = new google.visualization.ColumnChart(document.getElementById('divForGraph'));
                            chart.draw(data, options);
                        }

                        $('#divForImages').show();
                        $('#divForGraph').show();
                        $('#divForWords').show();
                    }
                    else {
                        HideElements();
                        $("#errorMessage").removeClass("noerror").addClass("error");
                    }
                },    //end success
                error: function (data) {
                    HideElements();
                    $("#errorMessage").removeClass("noerror").addClass("error");
                } //end error
            });  //end ajax
    });  //end on click           

    function HideElements() {
        $('#divForImages').hide();
        $('#divForGraph').hide();
        $('#divForWords').hide();
    }
});