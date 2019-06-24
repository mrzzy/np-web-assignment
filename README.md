# Folio - NP Web Assignment
This repository contains our WEB assignment source code.

## Setup 
### Docker
1. Install `docker` and `docker-compose`
2. Make a copy of the `dotenv` as `.env`and fill up the values to variables
3. Run `docker-compose up`
4. Open the application at http://localhost:5000

### Visual Studio
1. Configure SQL Server with script in `db\Student_EPortfolio_Db_SetUp_Script.sql`
2. Open `folio\folio.csproj` using the project
3. Run the project.
4. Open the application at http://localhost:5000

## Status
Current Project Status:
| No. | Step | Completition |
| --- | --- | --- |
| 1.1 | Complete Frontend Design for Folio | :heavy_checkmark: |
| 2.1 | Complete Database setup | |

## Structure
- db - database setup, container
- design - frontend design
- dotenv - template for `.env` 
- folio - folio project source code
- README.md - this file
