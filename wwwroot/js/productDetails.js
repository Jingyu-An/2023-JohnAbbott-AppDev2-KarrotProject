$(document).ready(function() {
    {
        $('.tab-content > div').hide();
        $('.nav-tabs a').click(function () {
            $('.tab-content > div').hide().filter(this.hash).fadeIn();
            $('.nav-tabs a').removeClass('active');
            $(this).addClass('active');
            return false;
        }).filter(':eq(0)').click();
    }
    $(".qtyminus").on("click",function(){
        var now = $(".qty").val();
        if ($.isNumeric(now)){
            if (parseInt(now) -1> 0)
            { now--;}
            $(".qty").val(now);
        }
    })
    $(".qtyplus").on("click",function(){
        var now = $(".qty").val();
        if ($.isNumeric(now)){
            $(".qty").val(parseInt(now)+1);
        }
    });
});

