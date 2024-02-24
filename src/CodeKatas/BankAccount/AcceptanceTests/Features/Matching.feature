Feature: Matching

Scenario: Matching By Tags
	Given there are following activies
	| Tags   | Owner |
	| #Food  | Ali   |
	| #Sport | Ali   |
	| #Food  | Sara  |
	| #Sport | Sara  |
	When 'Sara' explores for similiar people
	Then 'Ali' should suggests to her

Scenario: Matching 
	Given there are following activies
	| Tags   | Owner |
	| #Food  | Ali   |
	| #Sport | Ali   |
	| #Food  | Ali   |
	| #Food  | Ali   |
	| #Dance | Ali   |
	| #Study |  Ali  |
	| #Food  | Sara  |
	| #Sport | Sara  |
	| #Food  | Sara  |
	When 'Sara' explores for similiar people
	Then 'Ali' should suggests to her
