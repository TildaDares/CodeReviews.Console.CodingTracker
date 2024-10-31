# Coding Tracker

Coding Tracker is a C# console application designed for users to log the occurrences of their coding time. It allows users to record the start and end time of their coding sessions. The application stores this data in an SQLite database and provides functionality to insert, delete, update, and view logged coding sessions.

## Requirements

- [x] This is an application where you’ll log coding sessions.
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] To show the data on the console, you should use the "Spectre.Console" library.
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the coding sessions will be stored.
- [x] The users should be able to insert, delete, update and view their coding sessions.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] You need to use Dapper ORM for the data access instead of ADO.NET.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.
- [x] Follow the DRY Principle, and avoid code repetition.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works.

## Technologies Used

- **.NET**: Framework version 8.0
- **.NET Core**
- **C#**: Version 12.0
- **SQLite**
- **Dapper ORM**
- **Spectre Console**

## Features
- Coding Tracker: Users can log their coding sessions.
- Database: Data is stored in an SQLite database. The application ensures the database and necessary tables are created on startup if they do not exist.
- CRUD Operations: Provides functionality to create, read, update, and delete coding sessions.
- Error Handling: Comprehensive error handling to ensure the application does not crash.
- Console Interface: The application interacts with the user via the console for all input and output operations.

## Usage

<img width="362" alt="A console interface displaying the welcome message for Coding Tracker and a menu with the following options: 1. Insert coding session 2. Get coding session 3. Get coding sessions 4. Update coding session 5. Delete coding session 6. Start coding session 7. Filter coding sessions by period 8. View coding session report by period 9. Goals 10. Exit" src="https://github.com/user-attachments/assets/5404fd7d-b0f8-4602-aea9-730ffe938d7a">

### Recording a coding sessions

1. Select the option to log a coding session.
1. Enter the start and end time of the coding session.

### Viewing coding sessions
1. Select the option to view all coding sessions to see a list of all your sessions.

### Updating a coding session
1. Select the option to update a coding session.
1. Enter the ID of the coding session you wish to update.
1. Follow the prompts to update the coding session details.

### Deleting a coding session
1. Select the option to delete a coding session.
1. Enter the ID of the coding session you wish to delete.

### Setting Goals

Users have the ability to set coding goals and see how far they are from reaching their coding goals, along with how many hours a day they would have to code to reach their goal. Users can currently:
1. Set goals
2. View goal(s)
3. Update a goal
4. Delete a goal

<img width="274" alt="A console interface displaying the menu for Coding Goals with the following options: 1. Set coding goal 2. Get coding goal 3. Get coding goals 4. Update coding goal 5. Delete coding goal 6. Exit" src="https://github.com/user-attachments/assets/ab0969af-eb91-465c-bb77-a3e46886ad10">


<img width="568" alt="A console display showing a user's coding goal and details on the goal's progress" src="https://github.com/user-attachments/assets/6863c285-f41f-4ab0-b950-643478d4d9e5">

### Stopwatch

Users can track their coding time via a stopwatch so the user can track the session as it happens.

<img width="568" alt="A console display of the stopwatch feature in the coding tracker app" src="https://github.com/user-attachments/assets/f3a8d6ab-d11a-4ed4-be6d-b3b847ba29cc">

### Coding Reports

Users can create reports where they can see their total and average coding session per period.

<img width="771" alt="A console display of the reporting functionality in the coding tracker app" src="https://github.com/user-attachments/assets/fdd08b22-e2e6-41d3-b60f-fe8ccbaa5ff6">

### Database
The application uses SQLite for data storage. On application start, it checks for the existence of the database and the habit logger table, creating them if needed.

## Useful Resources

- [Human friendly enums in c#](https://medium.com/engineered-publicis-sapient/human-friendly-enums-in-c-9a6c2291111)
- [Spectre Console](https://spectreconsole.net/)
- [Dapper](https://www.learndapper.com/saving-data/insert)