Feature: Account Manager
		user story



Scenario: open a new bank account- without any fees
	Given Masoud as a customer
	And There is no any bank account for Masoud

	When Masoud opens a new bank account with 10000 toman
	Then A new bank account will be opened for Masoud with 10000 toman balance

Scenario: open a new bank account - calculating bank fees
	
	# context
	Given Masoud as a customer
	And There is no any bank account for Masoud
	And Bank fees are as follow
		| Sms | Charges |
		| 500 | 500     |

	 # Action
	When Masoud opens a new bank account with 10000 toman
	
	# Assert - outome
	Then A new bank account will be opened for Masoud with 9000 toman balance