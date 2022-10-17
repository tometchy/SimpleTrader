Feature: Trading

Scenario: LONG stop loss
	Given the BTC price is 19600 USD, I bet LONG with 3% threshold and bought for 100 USDC
	# I got 0.005102040816327 BTC
	When the price goes to 19000 USD
	# Bellow threshold
	Then 0.005102040816327 BTC is sold for 97 USDC
	# Loss: 3USDC (3%)
	
Scenario: LONG take profit
	Given the BTC price is 19600 USD, I bet LONG with 3% threshold and bought for 100 USDC
	# I got 0.005102040816327 BTC
	When the price goes to 19800 USD
	And the price goes to 20200 USD
	And the price goes to 20800 USD
	And the price goes to 21300 USD
	And the price goes to 20600 USD
	# Drop bellow threshold calculating from the biggest observed price (21300 USD)
	Then 0.005102040816327 BTC is sold for 105.1 USDC
	# Profit: 5.1 USDC (5.1%), top observed price raise was 8.7%
	
Scenario: SHORT stop loss
	Given the BTC price is 19600 USD, I bet SHORT with 3% threshold and sold 0.005
	# I got 98 USDC
	And the threshold is 3%
	When the price goes to 20200 USD
	# Above threshold
	Then 98 USDC is sold for 0.004854368932039 BTC
	# Loss: 0,000145631 BTC (3%)
	
Scenario: SHORT take profit
	Given the BTC price is 19600 USD, I bet SHORT with 3% threshold and sold 0.005
	# I got 98 USDC
	And the threshold is 3%
	When the price goes to 19300 USD
	And the price goes to 19100 USD
	And the price goes to 18400 USD
	And the price goes to 18200 USD
	And the price goes to 18800 USD
	# Raise above threshold calculating from the smallest observed price (18200 USD)
	Then 98 USDC is sold for 0.005212765957447 BTC
	# Profit: 0,000212766 BTC (4.26%), top observed price drop was 7.15%
