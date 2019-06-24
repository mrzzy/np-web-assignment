FROM microsoft/mssql-server-linux:2017-latest AS mssql
ARG ACCEPT_EULA=Y
ARG MSSQL_PID=Developer
ARG SA_PASSWORD

# copy & setup permissions on files required for setup
COPY Student_EPortfolio_Db_SetUp_Script.sql .
COPY wait-for-it.sh .
RUN chmod u+x ./wait-for-it.sh
COPY setup.sh .
RUN chmod u+x ./setup.sh

# perform setup of database
RUN env SA_PASSWORD=${SA_PASSWORD} sh ./setup.sh 
