var capbreak = capbreak || {};
capbreak.pop = {};
capbreak.pop.resize = function (el) {
    var page_width = window.innerWidth;
    var page_height = window.innerHeight;
    console.log(page_width + ', ' + page_height);
    var mgn = 20;
    var mw = page_width - 140;
    var mh = page_height - 140;
    var w = el.width;
    var h = el.height;
    var r = 0;

    if (w > mw) {
        r = mw / w;
        el.width = mw;
        el.height = h * r;
        w = w * r;
        h = h * r;
    }

    if (h > mh) {
        r = mh / h;
        el.width = w * r;
        el.height = mh;
    }

    var resizedWidth = el.width;
    var resizedHeight = el.height;
    $(el)
        .css('width', resizedWidth + mgn)
        .css('height', resizedHeight + mgn)
        .css('left', ((page_width - resizedWidth - mgn) / 2))
        .css('top', ((page_height - resizedHeight - mgn) / 2));

    $('#popimage').show();
    $('#popoverlay').show();
};

(function () {
    var popoverlay = $('<div />', { id: 'popoverlay' })
        .css('position', 'absolute')
        .css('top', 0)
        .css('left', 0)
        .css('width', '100%')
        .css('height', '100%')
        .css('opacity', .6)
        .css('display', 'none')
        .css('background', '#000')
        .css('user-select', 'none');
    $('body').append(popoverlay);

    $(document).keydown(function (e) {
        e = e || window.event;
        if (e.keyCode == 27) {
            $('#popoverlay').hide();
            $('#popimage').remove();
        }
    });

    $(document).ready(function () {
        $('img.pop').each(function () {
            $(this).click(function () {
                $('#popoverlay').hide();
                $('#popimage').remove();

                var popimage = $('<img />', { id: 'popimage', src: $(this).attr('src') })
                    .css('position', 'absolute')
                    .css('display', 'none')
                    .css('border', '5px solid #000')
                    .css('border-radius', 5)
                    .css('cursor', 'pointer')
                    .load(function (e) {
                        capbreak.pop.resize(e.target);
                    })
                    .click(function () {
                        $('#popoverlay').hide();
                        $(this).remove();
                    });

                $('body').append(popimage);
            });
        });
    });
})();