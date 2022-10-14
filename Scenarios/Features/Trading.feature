Feature: Trading

Scenario: LONG stop loss
	Given the BTC price is 19600 USD
	And the the bet is LONG
	And you bought with 100 USDC
	# You got 0.005102040816327 BTC
	And the threshold is 3%
	When the price goes to 19000 USD
	# Bellow threshold
	Then 0.005102040816327 BTC is sold for 97 USDC
	
Scenario: LONG take profit
	Given the BTC price is 19600 USD
	And the the bet is LONG
	And you bought with 100 USDC
	And the threshold is 3%
	When the price goes to 19800 USD
	And the price goes to 20200 USD
	And the price goes to 20800 USD
	And the price goes to 21300 USD
	And the price goes to 20600 USD
	# Drop bellow threshold calculating from top observed price (21300 USD)
	Then 0.005102040816327 BTC is sold for 105.1 USDC
