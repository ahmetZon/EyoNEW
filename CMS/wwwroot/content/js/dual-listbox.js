"use strict";
var KTDualListbox = {
    init: function () {
        $(".kt-dual-listbox").each(function () {
            var t = $(this),
                a = null != t.attr("data-available-title") ? t.attr("data-available-title") : "Departman Listesi",
                e = null != t.attr("data-selected-title") ? t.attr("data-selected-title") : "Eklenen Departman Listesi",
                l = null != t.attr("data-add") ? t.attr("data-add") : "Ekle",
                d = null != t.attr("data-remove") ? t.attr("data-remove") : "Secileni Sil",
                i = null != t.attr("data-add-all") ? t.attr("data-add-all") : "Tumunu Ekle",
                o = null != t.attr("data-remove-all") ? t.attr("data-remove-all") : "Tumunu Sil",
                n = [];


            $.LoadingOverlay("show");
            $.ajx('/kategori' + "/getKategoriDepartman", { id: Kategori_idUrl }, function (resultData) {

                $.each(resultData, function (i, item) {
                    var optionsAll = $("<option></option>").attr("value", item.Id).text(item.Ad);
                    if (item.selected)
                        $(optionsAll).attr('selected', "selected");
                    $(t).append(optionsAll);
                });

                t.children("option").each(function () {
                    var t = $(this).val(),
                        a = $(this).text(),
                        e = !!$(this).is(":selected");

                    n.push({ text: a, value: t, selected: e });
                });


                var r = null != t.attr("data-search") ? t.attr("data-search") : "";
                t.empty();
                var s = new DualListbox(t.get(0), {
                    addEvent: function (t) {
                        $.LoadingOverlay("show");
                        $.ajx('/kategori' + "/setKategoriDepartman", { KategoriId: Kategori_idUrl, DepartmanId: t, type: 'add' }, function (resultData) {
                            $.LoadingOverlay("hide");
                        }, function () { location.reload(); });
                    },
                    removeEvent: function (t) {
                        $.LoadingOverlay("show");
                        $.ajx('/kategori' + "/setKategoriDepartman", { KategoriId: Kategori_idUrl, DepartmanId: t, type: 'remove' }, function (resultData) {
                            $.LoadingOverlay("hide");
                        }, function () { location.reload(); });
                    },
                    availableTitle: a,
                    selectedTitle: e,
                    addButtonText: l,
                    removeButtonText: d,
                    addAllButtonText: i,
                    removeAllButtonText: o,
                    options: n
                });

                "false" == r && s.search.classList.add("dual-listbox__search--hidden")

                $.LoadingOverlay("hide");
            }, function () { location.reload(); });




        })
    }
};
$(function () {
    KTDualListbox.init();

});