#
# NP Web assignment
# db container setup script
#

/opt/mssql/bin/sqlservr & # run sql server in background
# wait for sqlserver port to open, then apply DB schema
./wait-for-it.sh -h localhost -p 1433  && \
    /opt/mssql-tools/bin/sqlcmd -U sa -P ${SA_PASSWORD} \
    -i Student_EPortfolio_Db_SetUp_Script.sql
