# Folio - NP Web Assignment
This repository contains our WEB assignment source code.

## Setup 
### Docker
1. Install `docker` and `docker-compose`
2. Make a copy of the `dotenv` as `.env`and fill up the values to variables
3. Run `docker-compose up`
4. Open the application at http://localhost:5000

### Visual Studio
1. Configure & Run SQL Server with script in `db\Student_EPortfolio_Db_SetUp_Script.sql`
2. Open `folio\folio.sln` using the project
3. Run the project.
4. Open the application at http://localhost:5000

## Status
Current Project Status:
| No. | Step | Completition |
| --- | --- | --- |
| 1.1 | Complete Frontend Design for Folio | :heavy_check_mark: |
| 2.1 | Complete Database setup | :heavy_check_mark: |
| 2.2 | Setup Entity Framework Core | :heavy_check_mark: |
| 2.3 | Dockerize Project | :heavy_check_mark: |
| 2.4.1 | AutoScaffolding of Models| :heavy_check_mark: |
| 2.4.2 | Injecting Connection String | :heavy_check_mark: |
| 2.4.3 | Fixing Missing Models From Scaffolding | :heavy_check_mark: |
| 2.5.1 | Setup Unit Testing Framwork| :heavy_check_mark: |
| 2.5.2 | Intergration Test Models | :heavy_check_mark: |
| 2.6.1 | Skillset API | :construction: |
| 2.6.2 | Authentication API | :construction: |
| 2.6.3 | Image API | :construction: |

Future TODO:
- CORS allow host on the ui project

## Structure
- db - database setup, container
- design - frontend design
- dotenv - template for `.env` 
- folio - folio project source code
    - Controllers - controllers
        - API - api controllers
- folio_tests - folio project source code
- README.md - this file
