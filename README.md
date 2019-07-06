# Folio - NP Web Assignment
This repository contains our WEB assignment source code.

## Setup 
### Docker
1. Install `docker` and `docker-compose`
2. Make a copy of the `dotenv` as `.env`and fill up the values to variables 
    - reference: https://drive.google.com/file/d/1xDCzM8pVr4Fqk8lzOBl7K6GLFjE86RMT/view?usp=sharing
3. Copy the `.env` file into `folio` and `folio_tests/` folders
4. Run `docker-compose up`
5. Open the application at http://localhost:5000

### Visual Studio
1. Configure & Run SQL Server with script in `db\Student_EPortfolio_Db_SetUp_Script.sql`
2. Make a copy of the `dotenv` as `.env.`and fill up the values to variables 
    - reference: https://drive.google.com/file/d/1EuTGCEuJMSgw-6AMFpyPTtbjgTF2hyhr/view?usp=sharing
3. Copy the `.env` file into `folio` and `folio_tests/` folders
4. Open `/np-web-assignment-1.sln` using the Visual Studio
5. Run the project.
6. Open the application at http://localhost:5000

## Status
Current Project Status:
- 1.1 | Complete Frontend Design for Folio | :heavy_check_mark: 
- 2.1 | Complete Database setup | :heavy_check_mark: 
- 2.2 | Setup Entity Framework Core | :heavy_check_mark:
- 2.3 | Dockerize Project | :heavy_check_mark:
- 2.4.1 | AutoScaffolding of Models| :heavy_check_mark: 
- 2.4.2 | Injecting Connection String | :heavy_check_mark:
- 2.4.3 | Fixing Missing Models From Scaffolding | :heavy_check_mark:
- 2.5.1 | Setup Unit Testing Framwork| :heavy_check_mark:
- 2.5.2 | Intergration Test Models | :heavy_check_mark:
- 2.6.1 | Skillset API | :heavy_check_mark:
- 2.6.2 | CDN Service - Google Cloud | :heavy_check_mark:
- 2.6.3 | Authentication API | :heavy_check_mark:
- 2.6.4 | File API | :heavy_check_mark:
- 2.7.1 | Landing Page | :construction:
- 2.7.1 | Sign Up Page | :construction:
- 2.7.1 | Login Page | :construction:
- 2.7.1 | Student Porfolio Page | :construction:
- 2.7.1 | Student Dashboard Page | :construction:

## Design
### Project Directory Structure
- db - database setup, container
- design - frontend design
- dotenv - template for `.env` 
- folio - folio API/backend source
    - Controllers - controllers
        - API - api controllers
    - Services - utility services
        - Auth - Authentication
        - Content - content delivery
- folio_ui - folio frontend source
    - Services - utility services
    - Controllers - controllers
        - view controllers
    - Views - views
    - wwwroot - static assets
        -  css - stylesheets
        -  js - javasript
        -  img - images
- folio_tests - folio project source code
- README.md - this file

### Stack
```
[Client]--[folio/api]---[DB]
   \           |
    \-----[folio]
```

- `[Client]` retrieve pages from `[folio]`
    - `[folio/ui]` retrieves data from `[folio/api]`
    - `[folio/api]` retrieves persistent data from `[DB]`
- `[Client]` performs data manpulation operations on `[folio/api]`
    - `[folio/api]` updates persistent data in `[DB]`

> Note that `[folio]` only concerns itself with the rendering of the data but
> does not handle data manuplation.

### Security
#### Authentication
**TODO: elaborate**
JWT Authorization Bearer token
