
$(document).ready(function () {

    $("select#AppointmentTime").hide();
    $("#progress").hide();
    
    $("#Appointment_ScheduledStart").datetimepicker({
        format: 'mm/dd/yyyy',
        startDate: '2013-01-01',
        minView: '2',
        autoclose: true
    })
    .on('changeDate', function (ev) {
        GetAvailableDate();
    });

    if ($("#Appointment_ScheduledStart").val()) {
        GetAvailableDate();
    }

    function GetAvailableDate() {
        
        LoadingText(true);
        $.ajax({
            url: "/Appointment/GetAvailableHours",
            type: "post",
            data: {
                date: $("#Appointment_ScheduledStart").val(),
                appointmentId: $("#Appointment_Id").val(),
                duration: 1
            },
            success: function(data) {
                $("select#AppointmentTime").empty();
                $("select#AppointmentTime").append(new Option("- Select Time -", ""));

                $.each(data, function (key, value) { 
                    $("select#AppointmentTime").append(new Option(value.text, value.value));
                    if ($("#CurrentAppointmentTime").val() == value.value)
                        $("select#AppointmentTime option:last").attr("selected", "selected");
                });

                LoadingText(false);
            }
        });
    }

    function LoadingText(show) {
       
        if (show) {
            $("select#AppointmentTime").hide();
            $("#progress").show();
            $('#AppointmentTime').attr('disabled', 'disabled');
        }
        else {
            $("select#AppointmentTime").show();
            $("#progress").hide();
            $('#AppointmentTime').removeAttr('disabled');
        }
    }
})