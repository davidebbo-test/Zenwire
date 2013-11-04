$(document).ready(function () {
if($.fullCalendar)
    $('#calendar').fullCalendar({
        events: eventSource,
        allDayDefault: false,
        editable: true,
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        ignoreTimezone: false,
        eventRender: function (event, element) { 
            console.log(event.start); 
        },
        eventClick: function (calEvent, jsEvent, view) {
            var eventTime = $.fullCalendar.formatDate(calEvent.start, "<b>dddd, MMM d yyyy</b> - h:mm tt");
            console.log(calEvent.start.toUTCString());
            var content = '<p><br/><b>' + calEvent.customer + '</b><br/>' + calEvent.address + '<br/>' + calEvent.city + '</p>' +
                '<p>' + eventTime + '<br/></p>';
            $('#myModal div.modal-header').html('<b>' + calEvent.title + '</b>');
            $('#myModal div.modal-body').html(content);
            $('#myModal button#btn-edit').click(function () {
                document.location.href = editLink + "/" + calEvent.id;
            });
            $('#myModal button#btn-assign').hide();
            if (calEvent.title == "Unassigned") {
                $('#myModal button#btn-assign').show();
                $('#myModal button#btn-assign').click(function () {
                    document.location.href = assignLink + "/" + calEvent.id;
                });
            }
            $('#myModal').modal('show');

        },

        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            if (!confirm('Are you sure ?'))
                revertFunc();
            else {
                $.ajax({
                    url: "/Appointment/ChangeTime",
                    type: "post",
                    data: {
                        days: dayDelta,
                        minutes: minuteDelta,
                        appointmentId: event.id
                    },
                    success: function (data) {
                        if (data && data.IsSuccess)
                            return true;
                        else {
                            if (confirm('There is no staff available on this date and time. \nContinue reschedule ?')) {
                                $.ajax({
                                    url: "/Appointment/ChangeTime",
                                    type: "post",
                                    data: {
                                        days: dayDelta,
                                        minutes: minuteDelta,
                                        appointmentId: event.id,
                                        confirmUpdate: true
                                    },
                                    success: function (data) {
                                        return true;
                                    },
                                    error: function () {
                                        revertFunc();
                                    }

                                });

                            } else
                                revertFunc();
                        }
                    }
                });

            }
        }
        // put your options and callbacks here
    });

});