Feature: Trading

Scenario: LONG stop loss
	Given the BTC price is 19600 USD
	And the bet is LONG
	And you bought with 100 USDC
	# You got 0.005102040816327 BTC
	And the threshold is 3%
	When the price goes to 19000 USD
	# Bellow threshold
	Then 0.005102040816327 BTC is sold for 97 USDC
	
Scenario: LONG take profit
	Given the BTC price is 19600 USD
	And the bet is LONG
	And you bought with 100 USDC
	# You got 0.005102040816327 BTC
	And the threshold is 3%
	When the price goes to 19800 USD
	And the price goes to 20200 USD
	And the price goes to 20800 USD
	And the price goes to 21300 USD
	And the price goes to 20600 USD
	# Drop bellow threshold calculating from the biggest observed price (21300 USD)
	Then 0.005102040816327 BTC is sold for 105.1 USDC
	
Scenario: SHORT stop loss
	Given the BTC price is 19600 USD
	And the bet is SHORT
	And you sold 0.005 BTC
	# You got 98 USDC
	And the threshold is 3%
	When the price goes to 20200 USD
	# Above threshold
	Then 98 USDC is sold for 0.004854368932039 BTC
	
Scenario: SHORT take profit
	Given the BTC price is 19600 USD
	And the bet is SHORT
	And you sold 0.005 BTC
	# You got 98 USDC
	And the threshold is 3%
	When the price goes to 19300 USD
	And the price goes to 19100 USD
	And the price goes to 18400 USD
	And the price goes to 18200 USD
	And the price goes to 18800 USD
	# Raise above threshold calculating from the smallest observed price (18200 USD)
	Then 98 USDC is sold for 0.005212765957447 BTC
