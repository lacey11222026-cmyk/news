if ($('.weather')) {
	$.ajax({
		url: 'http://truelife.vn/offica/infograber/action',
		jsonp:"jsoncallback",
		dataType: 'jsonp',
		type: 'GET',
		data: {
			_f: 1
		},
		success : function(o) {
			if (o.success) {
				var obj = o.object;
				var strHTML = "";
				for (var i=0; i< obj.length; i++) {
					if (obj[i].cityName == 'Hà Nội') {
						$("#w-desc").html(obj[i].description);
						var temperature = obj[i].currentTemperature  + '&ordm;C';
						if (obj[i].currentTemperature == 0) {
							temperature = obj[i].lowest + ' - ' + obj[i].highest + '&ordm;C';
						}
						$('#w-temp').html(temperature);
						$('#w-sky').attr('src', '/images/weather/' + obj[i].imageName);
						strHTML += '<option selected="selected" value="/images/weather/' + obj[i].imageName +
										'|' + obj[i].lowest +
										'|' + obj[i].highest +
										'|' + obj[i].description +
										'|' + obj[i].currentTemperature
										+ '">' + obj[i].cityName + '</option>';
					} else {
					    strHTML += '<option value="/images/weather/' + obj[i].imageName +
										'|' + obj[i].lowest +
										'|' + obj[i].highest +
										'|' + obj[i].description +
										'|' + obj[i].currentTemperature
										+ '">' + obj[i].cityName + '</option>';
					}
				}
				$('#locationnew').html(strHTML);
			}
		}
	});
	function change_weather_data(strInfo) {
		var wImage = strInfo.split("|")[0];
		var wLow = strInfo.split("|")[1];
		var wHigh = strInfo.split("|")[2];
		var wDescription = strInfo.split("|")[3];
		var wCurrent = strInfo.split("|")[4];
		document.getElementById('w-sky').src =  wImage;
		document.getElementById('w-desc').innerHTML =wDescription;
		if (wCurrent != 0){
			document.getElementById('w-temp').innerHTML =  wCurrent + '&ordm;C';
		}else {
			document.getElementById('w-temp').innerHTML =  wLow + ' - ' + wHigh + '&ordm;C';
		}
	}
}
