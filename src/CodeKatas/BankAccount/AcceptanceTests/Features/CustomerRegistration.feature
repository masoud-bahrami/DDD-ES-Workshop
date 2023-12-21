Feature: Customer Registration
		user story

Scenario: registring a new customer
	Given The latest given customer id is 1234-4321-2532-2532
	When A new customer called Masoud enroll in bank with following information
		| First Name | Last Name | National Code | Birth Date |
		| Masoud     | Bahrami   | 012345678     | 1990-04-28 |
	Then We will give him customer id 1234-4321-2532-2534 which he can use for further services