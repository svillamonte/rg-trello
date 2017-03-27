# RgTrello

Basic frontend integration with the Trello API. It features the following:
* OAuth authentication against Trello to get token that enables user impersonation.
* Basic display of user's boards.
* Basic display of cards belonging to a board.
* Basic display of card name and description. A form is provided to post a comment to the card currently being displayed.

## Installation

This solution is an ASP.NET MVC 5 project, along with some helper libraries. To run it, you'll need IIS 10 and above:
1. Unzip solution package.
2. Go to create a new website under the `Sites` categories.
3. Under `Physical path`, choose the path where the package was unzipped.
4. Make sure the AppPool created runs under .NET CLR v4 and the pipeline mode is `Integrated`.
5. Click on save.
6. Fire up a browser and go to the configured host name.

## Basic usage

Before performing any action, the app needs to be authenticated against Trello. Once this is done, the `Boards` menu will be enabled.

## Future work

A couple of concessions have been made to deliver this completely functional solution:

* The use of a persistence layer could not be justified. Right now, the user needs to perform an authentication against Trello every time the application is started. The obtained token is then stored in a singleton TokenManager class, which proved to be enough for the identified scope of the exercise. Future work could include persisting the user and related token in the database, avoiding using a singleton, which is considered an antipattern for large scale systems. This would allow:
** Having a record of all the users of the application, by making use of Trello as a third-party login provider. Each profile can be registered via cookies dropped in the client and on each new run, the user id from the cookie could be matched against the user's profile in the database to retrieve the proper token.
** Making proper use of token expiration and renewal by updating the recorded sets.

* Secrets and keys are stored in the source code. This is a security issue that would need to be solved by storing them in encrypted configuration files. For large scale solutions, secrets are stored outside source control, under the deployment pipeline tool infrastructure.

* Most of the controller actions follow the same pattern of performing logic and handling any exceptions. This could definitely be improved by refactoring common logic out to shared generic methods. In this case in particular, and given the fact that the solution scope is limited to this exercise, undergoing this work is probably not worth it.
