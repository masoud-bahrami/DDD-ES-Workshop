Feature: Account Management
  As a user
  I want to be able to manage my account
  So that I can perform various account-related operations

Scenario: Open an Account
  Given there is no existing account for Masoud
  When Masoud opens a new account with an initial deposit of $100
  Then a new account should be created with a balance of $100 for Masoud