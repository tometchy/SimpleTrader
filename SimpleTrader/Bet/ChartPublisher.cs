using SimpleTrader.Events;

namespace SimpleTrader.Bet;

public static class ChartPublisher
{
    public static void Publish(NewTradeExecuted newMarketUpdate)
    {
        var fileName = $"{newMarketUpdate.Exchange}_{newMarketUpdate.PairTicker}.html".ToLower().Replace("/", "");
        var filePath = $"/var/simple-trader/chart/{fileName}";
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, emptyChartFile
                .Replace("%EXCHANGE%", newMarketUpdate.Exchange)
                .Replace("%TICKER%", newMarketUpdate.PairTicker));

        string text = File.ReadAllText(filePath);
        text = text.Replace("// ADDHERE",
            $"addMarketUpdate(\"{newMarketUpdate.Timestamp:MM/dd/yyyy HH:mm:ss.fff} GMT\", {newMarketUpdate.LastTradePrice}, {newMarketUpdate.LastTradeQuantity});\n// ADDHERE");
        File.WriteAllText(filePath, text);
    }

    private const string emptyChartFile = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">
<html>
<head>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
	<meta name=""robots"" content=""noindex,nofollow"">
	<title>Price chart</title>
	<link href=""./flot/style.css"" rel=""stylesheet"" type=""text/css"">
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.event.drag.js""></script>
    <script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.mousewheel.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.canvaswrapper.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.colorhelpers.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.saturated.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.browser.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.drawSeries.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.uiConstants.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.navigate.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.touchNavigate.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.hover.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.touch.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.selection.js""></script>

	<script language=""javascript"" type=""text/javascript"" src=""./flot/globalize.js""></script>
	<script language=""javascript"" type=""text/javascript"" src=""./flot/jquery.flot.time.js""></script>
	<script type=""text/javascript"">

	var lines = [1.714, 1.737, 1.81];
	var marketUpdates = [];
	var marketUpdatesDetails = {};
	var theSmallestDollarsAmountPerTrade = 0;
	var theBiggestDollarsAmountPerTrade = 0;

	var previousTimestampMillis = 0;
	function addMarketUpdate(timestamp, price, quantity){
		let timestampMillis = new Date(Date.parse(timestamp + "" GMT"")).getTime();
		if(timestampMillis == previousTimestampMillis)
			timestampMillis++;
		previousTimestampMillis = timestampMillis;

		marketUpdates.push([timestampMillis, price]);
		marketUpdatesDetails[timestampMillis] = {
			timestamp: timestamp,
			quantity: quantity
		};
	}

	$(function() {
		// addMarketUpdate(""12/27/2022 20:17:07.521 GMT"", 1.70000, 28.38522029);
		// addMarketUpdate(""12/27/2022 20:17:07.721 GMT"", 1.69000, 28.07821889);
		// addMarketUpdate(""12/27/2022 20:18:58.121 GMT"", 1.69000, 8.90000000);
// ADDHERE

		var plot = $.plot(""#placeholder"", [
			{ data: marketUpdates, label: ""sin(x)""}
		], {
			series: {
				lines: {
					show: true
				},
				points: {
					show: true
				}
			},
			grid: {
				hoverable: true,
				clickable: true
			},
			xaxis: {
					mode: ""time"",
					// min: (new Date(1990, 0, 1)).getTime(),
					// max: (new Date(2000, 0, 1)).getTime(),
					timeBase: ""milliseconds"",
			},
			yaxis: {
				// min: -1.2,
				// max: 1.2
			},
			zoom: {
				interactive: true
			},
			pan: {
				interactive: true,
				enableTouch: true
			}
		});

		// window.setInterval(function () {
			plot.setData([
				{ data: marketUpdates, label: ""sin(x)""}
			]);
		// }, 2000);

		$(""<div id='tooltip'></div>"").css({
			position: ""absolute"",
			display: ""none"",
			border: ""1px solid #fdd"",
			padding: ""2px"",
			""background-color"": ""#fee"",
			opacity: 0.80
		}).appendTo(""body"");

		$(""#placeholder"").bind(""plothover"", function (event, pos, item) {

			if (!pos.x || !pos.y) {
				return;
			}

			if ($(""#enablePosition:checked"").length > 0) {
				var str = ""("" + pos.x.toFixed(2) + "", "" + pos.y.toFixed(2) + "")"";
				$(""#hoverdata"").text(str);
			}

			// if ($(""#enableTooltip:checked"").length > 0) {
				if (item) {
					var timestampMillis = item.datapoint[0];//.toFixed(2),
					var price = item.datapoint[1];//.toFixed(2);

					$(""#tooltip"").html(""Price: "" + price + "" Quantity: "" + marketUpdatesDetails[timestampMillis].quantity + "" ("" + (price * marketUpdatesDetails[timestampMillis].quantity).toFixed(0) + ""$)"" + "" Timestamp: "" + marketUpdatesDetails[timestampMillis].timestamp)
						.css({top: item.pageY+5, left: item.pageX+5})
						.fadeIn(200);
				} else {
					$(""#tooltip"").stop().hide();
				// }
			}
		});

		$(""#placeholder"").bind(""plothovercleanup"", function (event, pos, item) {
				$(""#tooltip"").hide();
		});

		$(""#placeholder"").bind(""plotclick"", function (event, pos, item) {
			if (item) {
				$(""#clickdata"").text("" - click point "" + item.dataIndex + "" in "" + item.series.label);
				plot.highlight(item.series, item.datapoint);
			}
		});
	});

	</script>
</head>
<body>
	<div id=""header"">
		<h2>%EXCHANGE% %TICKER%</h2>
	</div>

	<div id=""content"">

		<div class=""demo-container"">
			<div id=""placeholder"" class=""demo-placeholder""></div>
		</div>

	</div>

</body>
</html>
";
}