$(document).ready(function () {
    $("#collapse-icon").click(function () {
        $(this).parents(".card-header").next(".card-body").toggle()
    })
});