$(function() {
    $('#server-graph').height($(window).height() - 150);
    $(window).resize(function () {
        $('#server-graph').height($(window).height() - 150);
    });

    $('.collapse').collapse({
        toggle: false
    });

    $('li').on('click', function () {
        $(window).resize();
    });

    setTimeout(function () {
        window.location.reload(1);
    }, 60000);

    $('a[data-toggle="tab"]').on('click', function (e) {
        localStorage.setItem('lastTab', $(e.target).attr('href'));
    });
    var lastTab = localStorage.getItem('lastTab');
    if (lastTab) {
        $('a[href="' + lastTab + '"]').click();
    }

    $('#chart-carouse').carousel({
        interval: 5000
    });
})