$.ajax({
	type: "GET",
	url: 'https://localhost:44389/api/Dashboard/GetMonthPurchaseApi',
	contentType: "application/json; charset=UTF-8",
	dataType: "json",
	success: function (data) {
		$("#MonthPurchase tbody").empty();
		$.each(data, function (i, item) {
			var rows = "<tr>" +
				"<td >" + item.Ac_Desc + "</td>" +
				"<td >" + item.Opening + "</td>" +
				"<td> " + item.DebitAmt + "</td>" +
				"<td> " + item.CreditAmt + "</td>" +
				"<td> " + item.BALANCE + "</td>" +
				"</tr>";
			$('#MonthPurchase').append(rows);

		}); //End of foreach Loop



	}, //End of AJAX Success function
	complete: function (data) {
		$('#loaderTable').hide();
	},
	failure: function (data) {
		alert(data.responseText);
	}, //End of AJAX failure function
	error: function (data) {
		alert(data.responseText);
	} //End of AJAX error function

});