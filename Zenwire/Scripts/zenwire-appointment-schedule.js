
//var restrictedHour = [];
//$(document).ready(function () {
//    //$(".datetimepicker").datetimepicker();
//    $(".datetimepicker").datepicker({
//        minDate: 0,
//        onSelect: GetUnavailableDate,
//        dateFormat: 'd/m/yy'
//    });

//    GetUnavailableDate();

//    $('#AppointmentTime').timepicker(
//    {
//        onHourShow: OnHourShowCallback,
//        onMinuteShow: OnMinuteShowCallback
//    });

//    function OnHourShowCallback(hour) {

//        if ($.inArray(hour, restrictedHour) >= 0) {
//            return false;
//        }
//        return true;
//    }
//    function OnMinuteShowCallback(hour, minute) {
//        if ((hour == 18) && (minute >= 0)) { return false; } // not valid
//        if ((hour == 9) && (minute < 30)) { return false; }   // not valid
//        return true;  // valid
//    }

//    function GetUnavailableDate() {
//        LoadingText(true);
//        $.ajax({
//            url: unavailableSource,
//            type: "post",
//            data: { date: $("#Appointment_ScheduledTime").val() },
//            success: function (data) {
//                restrictedHour = data;
//                LoadingText(false);
//            }
//        })
//    }

//    function LoadingText(show) {
//        if (show) {
//            $('#AppointmentTime').attr('disabled', 'disabled');
//            $('#AppointmentTime').parent().append("<span id='loadingText'>Loading...</span>");

//        }
//        else {
//            $('#AppointmentTime').removeAttr('disabled');
//            $("span#loadingText").remove();
//        }
//    }
//})