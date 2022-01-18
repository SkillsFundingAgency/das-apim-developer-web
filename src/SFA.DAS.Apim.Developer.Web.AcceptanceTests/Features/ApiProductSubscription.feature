Feature: ViewSubscriptions
	View subscriptions as Provider, Employer and External

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

@WireMockServer @Employer
Scenario: Viewing available subscription products for employer
	Given I navigate to the following url: /accounts/ABC123/subscriptions
	Then an http status code of 200 is returned
	And the page content includes the following: API list
	And there is a row for each product subscription
	And the subscribed link is shown
	And the non-subscribed link is shown

@WireMockServer @Employer
Scenario: When trying to view another employer subscriptions
	Given I navigate to the following url: /accounts/XYZ345/subscriptions
	Then an http status code of 403 is returned

@WireMockServer @External
Scenario: Viewing available subscription products for external
	Given I navigate to the following url: /384a56e3-14f9-4133-80b2-e10572890f3d/subscriptions
	Then an http status code of 200 is returned
	And the page content includes the following: API list
	And there is a row for each product subscription
	And the subscribed link is shown
	And the non-subscribed link is shown

@WireMockServer @External
Scenario: When trying to view another employer external
	Given I navigate to the following url: /484a56e3-14f9-4133-80b2-e10572890f3d/subscriptions
	Then an http status code of 403 is returned
