Feature: Account Manager
		user story


Scenario: open a new bank account- without any fees
	Given Masoud as a customer
	And There is no any bank account for Masoud
	When Masoud opens a new bank account with 10000 toman
	Then A new bank account will be opened for Masoud with 10000 toman balance


Scenario: open a new bank account - calculating bank fees
	Given Masoud as a customer
	And There is no any bank account for Masoud
	And Bank fees are as follow
	 | Sms | Charges |
	 | 500 | 500     |

	When Masoud opens a new bank account with 10000 toman
	Then A new bank account will be opened for Masoud with 90000 toman balance