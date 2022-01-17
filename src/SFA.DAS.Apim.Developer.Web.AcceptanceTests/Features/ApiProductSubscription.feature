Feature: ProviderViewKeys
	Simple calculator for adding two numbers

@WireMockServer @Provider
Scenario: Viewing available subscription products for provider
	Given I navigate to the following url: /10000001/subscriptions
	Then an http status code of 200 is returned
	And the page content includes the following: API list
	And there is a row for each product subscription
	And the subscribed link is shown
	And the non-subscribed link is shown
	
@WireMockServer @Provider
Scenario: When trying to view another providers subscriptions
	Given I navigate to the following url: /10010001/subscriptions
	Then an http status code of 403 is returned
	