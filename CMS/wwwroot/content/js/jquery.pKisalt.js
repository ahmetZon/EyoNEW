
(function ($) {
    $.fn.pKisalt = function (ayarlar) {

        var ayar = $.extend({
            'limit': 100,
            'nokta': false,
            'goster': true,
            'gizle': true,
            'text': '...',
            'text2': 'Hide'
        }, ayarlar);
        return this.each(function () {
            var diger = '';
            var gizle = '';
            var nesne = $(this);
            var length = nesne.text().length;
            if (length > ayar.limit) {
                nesne.wrap('<div class="pKisalt_kisaltilmis"></div>');
                if (ayar.gizle == true) {
                    gizle = '<a href="javascript:;" style="float:none;" class="pKisalt_gizle _df">' + ayar.text2 + '</a>';
                }
                nesne.after('<div class="moreCustom pKisalt_orjinal" style="display: none">' + nesne.html() + gizle + '</div>');
                if (ayar.nokta == true) {
                    diger = '..';
                }
                if (ayar.goster == true) {
                    diger += '<a href="javascript:;" class="pKisalt_goster _df">' + ayar.text + '<em class="_b"></em></a>';
                }
                var text = nesne.html();
                var kisaltilmis = text.substr(0, ayar.limit) + diger;
                $(this).html(kisaltilmis);
            }

            $("a.pKisalt_goster").click(function () {
                if ($(".moreCustom").length > 0) {
                    $(this).parent().hide().next(".moreCustom.pKisalt_orjinal").show();
                    return false
                }
            });
            $("a.pKisalt_gizle").click(function () {
                if ($(".moreCustom").length > 0) {
                    $(this).parent().hide().prev().show();
                    $("html, body").animate({ scrollTop: "0" }, 400);
                    return false
                }
            });



        });

    }

})(jQuery);