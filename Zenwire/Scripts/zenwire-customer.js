$(function ()
{
    var ajaxFormSubmit = function() {
        var $form = $(this);

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        };

        $.ajax(options).done(function(data)
        {
            var $target = $($form.attr("data-zenwire-target"));
            $target.replace(data);
            alert("Whoa!");
        });

        return false;
    };

    $("form[data-zenwire-ajax='true']").submit(ajaxFormSubmit);
    
});