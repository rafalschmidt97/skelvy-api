This repository is a part of **skelvy** project. It includes api for mobile app.
Created with dotnet core and aspnet. Crafted with love to new technologies! 

[![Build Status](https://travis-ci.com/rafalschmidt97/skelvy-api.svg?token=z2Ugw1HzqG3BK6LFW2LT&branch=master)](https://travis-ci.com/rafalschmidt97/skelvy-api)

There are other parts of the system which are located within different repositories:
- [Mobile apps (Android and iOS) - Ionic 4 and Angular 8](https://github.com/rafalschmidt97/skelvy-client)
- [Website - Wordpress](https://github.com/rafalschmidt97/skelvy-website)
- [Utilities - additional tools, graphics](https://github.com/rafalschmidt97/skelvy-utilities)

## Idea

The idea for Skelvy is extremely simple as it a tool for arranging and finding meetings over favourite activities in the city. Users log in to the app, fill in the meeting parameters with when, where and what they are planning or simply would like to do. Meetings always might be open to the public so other people in the area would see them in their explore tab.

![Mockup](docs/mockup.png)

## Tech stack

C#, REST with ASP.NET Core 3.0 (the structure heavily inspired by jasontaylordev/NorthwindTraders - with CQS approach, FluentValidation and EF configs in external files), MSSQL (docker), Entity Framework, Dapper, Redis (docker) for cache and services backplane, Scheduling tasks with Coravel, OpenAPI with Swagger, Email Templates with Razor, Geolocation with Google Maps, Authorization with Facebook and Google (JWT), Websocket with SignalR for real time communication (chat + in app notifications), Firebase Messaging for Push Notifications, TDD, Xunit, Moq, Sqlite for testing, Logging with Serilog, Linting with StyleCop and FxCop, Azure Storage for uploading photos, CI with TravisCI, deploy as docker image to Azure App Service (with Azure MSSQL and Redis).

The app is translated into 9 languages:
- Polish
- English
- German
- Spanish
- French
- Italian
- Russian
- Finnish

## Database schema

![Schema](docs/schema.png)

## Video introduction

[![Video](docs/youtube.png)](https://youtu.be/_uS4fBgzgC4)

- [English Youtube Video](https://youtu.be/_uS4fBgzgC4)
- [Polish Youtube Video](https://youtu.be/kRucIyZtuGA)

## Contributing guidelines

We really hope that good instructions for contributing will make the history of our app more readable. 
Take a look [here](CONTRIBUTING.md).

## Development

Current state of development is available to see on [trello](https://trello.com/b/MCzNyRJf).

## Stay in touch

* Website - [skelvy.com](https://skelvy.com/)
* Facebook - [skelvyapp](https://facebook.com/skelvyapp/)
* Twitter - [skelvyapp](https://twitter.com/skelvyapp/)
* Instagram - [skelvyapp](https://instagram.com/skelvyapp/)
* Email - [contact.skelvy@gmail.com](mailto:contact.skelvy@gmail.com)
