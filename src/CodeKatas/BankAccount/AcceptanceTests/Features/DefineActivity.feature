Feature: DefineActivity

Scenario: Write an Activity
	Given 'Sara' is a User
	And her activity list is empty
	When she writes an activity with text 'Eat Breakfast at 8 am'
	And she assigned following tags '#Breakfast'
	Then she activities should contain following information
	| Text                  | Tags       |
	| Eat Breakfast at 8 am | #Breakfast |
