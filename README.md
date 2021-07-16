# Bookworms Lending Library
Bookworms Lending Library is a .NET application that combines a Blazor Server frontend with a Web API backend. It was built for an imaginary online library that mails books out to its borrowers.

##### BookwormsUI project Build Status: 
[![Build Status - BookwormsUI](https://dev.azure.com/davidajohn/Bookworms%20Library/_apis/build/status/Bookworms%20Library-BookwormsUI?branchName=main)](https://dev.azure.com/davidajohn/Bookworms%20Library/_build/latest?definitionId=3&branchName=main)

##### BookwormsAPI project Build Status:
[![Build Status - BookwormsAPI](https://dev.azure.com/davidajohn/Bookworms%20Library/_apis/build/status/Bookworms%20Library%20-%20BookwormsAPI?branchName=main)](https://dev.azure.com/davidajohn/Bookworms%20Library/_build/latest?definitionId=4&branchName=main)

You can see a demo version of the site running on Microsoft's Azure cloud service at: [https://bookworkslibrary.azurewebsites.net](https://bookworkslibrary.azurewebsites.net)


![Screenshot](https://bookwormslibrary.blob.core.windows.net/promo/bookworms_screenshot.png "Screenshot")


## Features

- .NET Web API backend using Entity Framework and SQL Server
- Implements a generic repository along with the specification pattern
- Custom exception middleware
- Authentication and authorisation using .NET Core Identity and JWT bearer tokens
- Allows pagination, sorting and filtering of data
- CRUD methods for each repository (including PATCH)
- Response caching

- Search pages for books and authors with search, sorting and pagination
- Image uploads to Azure Storage containers
- Individual author pages with biography and book list 
- Bookshelf component used to display custom ranges of books
- Bootstrap carousel for promotional images
- Individual book pages incorporating user borrowing requests
- Admin user interface for creating and editing books and authors
- Admin dashboard displaying current borrowing data
