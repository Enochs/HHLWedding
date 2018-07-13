

(function ($) {

	$.fn.hhldatePicker = function (date, timeOption) {

		if (date == null || date == "") {
			date = "reportrange";
		}
		if (timeOption == null) {
			timeOption = {
				StartName: "hideBegin",
				EndName: "hideEnds",
			};
		}
		timeOption.Format = "YYYY-MM-DD";


		var optionSet1 = {

			ranges: {
				'今天': [moment(), moment()],
				'昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
				'最近7天': [moment().subtract('days', 6), moment()],
				'最近30天': [moment().subtract('days', 29), moment()],
				'本月': [moment().startOf('month'), moment().endOf('month')],
				'上个月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')],
				'本季度': [moment().startOf('quarter'), moment().endOf('quarter')],
				'今年': [moment().startOf('year'), moment().endOf('year')],
			},

			buttonClasses: ['btn btn-default'],
			applyClass: 'btn-small btn-success',
			cancelClass: 'btn-small',
			locale: {
				format: 'YYYY-MM-DD',
				separator: ' - ',
				applyLabel: '确定',
				cancelLabel: '取消',
				customRangeLabel: '选择',
				daysOfWeek: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
				monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
				firstDay: 1
			},
			startDate: moment().startOf('month').format(timeOption.Format),
			endDate: moment().endOf('month').format(timeOption.Format),
			minDate: '01/01/2000',
			maxDate: '12/31/2999',
			//dateLimit: {
			//	days: 60
			//},
			showDropdowns: true,
			showWeekNumbers: true,

			timePickerIncrement: 1,
			timePicker: false,
			timePicker12Hour: false,
			timePicker24Hour: false,
			timePickerSeconds: false,
			opens: 'right'
		};

		//默认本月
		$('#' + date + ' span').html(optionSet1.startDate + ' - ' + optionSet1.endDate);

		$('#' + date).daterangepicker(optionSet1, function (start, end, label) {

			$('#' + date + ' span').html(start.format(timeOption.Format) + ' - ' + end.format(timeOption.Format));
			//隐藏域赋值
			$("#" + timeOption.StartName).val(start.format('YYYY-MM-DD 00:00:00'));
			$("#" + timeOption.EndName).val(end.format('YYYY-MM-DD 23:59:59'));

		});

	}


})(jQuery);