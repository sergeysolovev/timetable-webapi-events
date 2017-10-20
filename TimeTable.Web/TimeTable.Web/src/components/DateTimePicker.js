import moment from 'moment';
import 'moment/locale/ru';
import 'eonasdan-bootstrap-datetimepicker';

export function init() {
  var standartFormat = "YYYY-MM-DD";
  var url =  $('#week').data('url');
  var weekMonday = $('#week').data('weekmonday');

  $('.weektimepicker').datetimepicker({
    locale: 'ru',
    format: standartFormat,
    defaultDate: weekMonday
  });

  var isCalendarOpened = false;
  $('a#week').click(function (e) {
    if (!isCalendarOpened) {
        $('.weektimepicker').data('DateTimePicker').show();
    } else {
        $('.weektimepicker').data('DateTimePicker').hide();
    }
    isCalendarOpened = !isCalendarOpened;
    $('tr').has('td.day.active').css("background-color", "rgba(0,0,0,.25)");
    e.stopPropagation();
  });

  $(document).click(function () {
    $('.weektimepicker').data('DateTimePicker').hide();
    isCalendarOpened = false;
  });

  $('.weektimepicker').on('dp.change', function (e) {
    console.log('dp.change')
    var newWeekMonday = moment($(".weektimepicker").val(), standartFormat).day(1).locale("ru").format(standartFormat);
    window.location.href = location.protocol + "//" + location.host + url.replace("/" + weekMonday, '') + "/" + newWeekMonday;
  });
}
