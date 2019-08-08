#
# Web Development Module
# Web Practicals
# FOLIO api
#

# project to build the image from 
# folio - api/backend image
ARG PROJECT_NAME=folio

# stage development environment
FROM microsoft/dotnet:2.2-sdk AS develop
ARG PROJECT_NAME
# copy source files
WORKDIR /src
COPY . .
WORKDIR /src/${PROJECT_NAME}
# dotnet watch requires these directories to be accessable
RUN chmod a=rwx /tmp
# setup home directory
RUN chmod a=rwx /home
ENV HOME=/home
# run hot reload service
ENV PROJECT_NAME=$PROJECT_NAME
RUN touch .env # dummy dotenv file manually insert to /src
ENTRYPOINT dotnet watch --project ${PROJECT_NAME}.csproj run

# stage: build project
FROM develop AS build
ARG PROJECT_NAME
RUN dotnet restore ${PROJECT_NAME}.csproj
RUN dotnet build ${PROJECT_NAME}.csproj -c Release -o /app
RUN dotnet publish ${PROJECT_NAME}.csproj -c Release -o /app

# stage: production runtime environen 
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS production
ARG PROJECT_NAME
EXPOSE 49776
EXPOSE 44336
WORKDIR /app
COPY --from=build /app .
ENV PROJECT_NAME=$PROJECT_NAME
ENTRYPOINT dotnet ${PROJECT_NAME}.dll
