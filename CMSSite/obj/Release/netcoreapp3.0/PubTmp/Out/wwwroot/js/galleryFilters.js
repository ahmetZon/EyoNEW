jQuery(window).load(function () {
    $("#filters a").click(function () {
        $("#filters li a.selected").each(function () {
            $(this).removeClass("selected");
        });
        $(".galHeader").html($(this).html())
    }) 
});
