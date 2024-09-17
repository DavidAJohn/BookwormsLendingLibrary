# Bookworms Lending Library
Bookworms Lending Library is a .NET application that combines a Blazor Server frontend with a Web API backend. It was built for an imaginary online library that mails books out to its borrowers.

This is an experimental project. It was not created with the aim of becoming a production-ready enterprise solution and it should not be used as the basis for a production application.

##### BookwormsUI project Build Status: 
[![Build Status - BookwormsUI](https://dev.azure.com/davidajohn/Bookworms%20Library/_apis/build/status/Bookworms%20Library-BookwormsUI?branchName=main)](https://dev.azure.com/davidajohn/Bookworms%20Library/_build/latest?definitionId=3&branchName=main)

##### BookwormsAPI project Build Status:
[![Build Status - BookwormsAPI](https://dev.azure.com/davidajohn/Bookworms%20Library/_apis/build/status/Bookworms%20Library%20-%20BookwormsAPI?branchName=main)](https://dev.azure.com/davidajohn/Bookworms%20Library/_build/latest?definitionId=4&branchName=main)

You can see a demo version of the site running on Microsoft Azure at: [https://bookwormslibrary.azurewebsites.net](https://bookwormslibrary.azurewebsites.net)


![Screenshot](https://bookwormslibrary.blob.core.windows.net/promo/bookworms_screenshot.png "Screenshot")


## Features

- .NET Web API backend using Entity Framework and SQL Server
- Implements a generic repository along with the specification pattern
- Custom exception middleware
- Authentication and authorisation using .NET Core Identity and JWT bearer tokens
- Allows pagination, sorting and filtering of data
- CRUD methods for each repository (including PATCH)
- Response caching
- Unit tests using bUnit

- Search pages for books and authors with search, sorting and pagination
- Image uploads to Azure Storage containers
- Individual author pages with biography and book list 
- Bookshelf component used to display custom ranges of books
- Bootstrap carousel for promotional images
- Individual book pages incorporating user borrowing requests
- Admin user interface for creating and editing books and authors
- Admin dashboard displaying current borrowing data

## Setup

To run the application on your local machine, after cloning or downloading it from GitHub, you'll need the following:

- The .NET 8 SDK installed locally
- SQL Server installed locally, or in a Docker container (see below)
- An Azure account with a storage container (if you want to enable image uploads in the admin section)

If you're using Visual Studio (not VS Code) as your code editor, you will need to be running the 2022 version (or higher). The free Community Edition is sufficient to run this application.

Assuming that you have Docker Desktop installed and running, to create a container using the official **Microsoft SQL Server 2017 Docker image**, either enter the following from a terminal (all on one line):

`docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd1234" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest`

*or*

you can make use of the **docker-compose.yml** file in the root directory of the application, and enter the following from a terminal window from within the application's root:

`docker-compose up -d`

The official Microsoft SQL Server 2017 image is 1.4Gb, so if you don't already have it downloaded in Docker this could take a few minutes, depending on the speed of your internet connection.

The application is set up to use this Docker container to provide its database, so if you use a local installation of SQL Server instead, you'll need to adjust the database connection strings in the **appsettings.Development.json** file in the **BookwormsAPI** folder accordingly.

This is a **Blazor Server** application. Both the UI and API projects will need to be running at the same time (unless you simply want to test the API in a separate client application such as Postman).

In **Visual Studio 2022**, you will need to set it to run multiple startup projects and select both the **BookwormsUI** and **BookwormsAPI** projects.

In **Visual Studio Code**, you will need two terminal windows. One where you '`cd`' into the **BookwormsAPI** folder and then type '`dotnet run`', and one where you do the same for the **BookwormsUI** folder.

The first time the application runs, it will seed sample book and author data into your database. 

It also creates an admin login automatically. The login details can be found in the **appsettings.Development.json** file in the **BookwormsAPI** folder. This should obviously only be used during development and testing and not in production.
