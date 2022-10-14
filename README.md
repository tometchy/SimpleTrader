# CAUTION!
It's a hobby project, use it at your own risk!

# How it works
You need to have BTC or ETH and USDC under your Kraken account and will be constantly switching between stable coins and BTC or ETH.

When you think that crypto price will raise, put 10% of your dollars in it and set here LONG bet with threshold, for example 3%.
App will watch crypto price and whenever it will drop bellow threshold, the bet will be closed.

When you think that crypto price will drop, spend 10% of your crypto into dollars (USDC) and set here SHORT bet with threshold, for example 3%.
App will watch crypto price and whenever it will raise above threshold, the bet will be closed.

See [bdd tests](SimpleTraderTests/BettingTests.cs) for more specific examples.
