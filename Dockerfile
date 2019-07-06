#
# Web Development Module
# Web Practicals
# Project Dockerfile 
#

# project to build the image from 
# folio - api/backend image
# folio_ui - frontend image
ARG PROJECT_NAME=folio

# stage development environment
FROM microsoft/dotnet:2.2-sdk AS develop
ARG PROJECT_NAME
WORKDIR /src
COPY . .
# dotnet watch requires these directories to be accessable
RUN mkdir /.dotnet && chmod a=rwx /.dotnet
RUN mkdir /.nuget && chmod a=rwx /.nuget
RUN chmod a=rwx /tmp
ENV PROJECT_NAME=$PROJECT_NAME
ENTRYPOINT dotnet watch --project ${PROJECT_NAME}/${PROJECT_NAME}.csproj run

# stage: build project
FROM develop AS build
ARG PROJECT_NAME
ENV PROJECT_NAME=$PROJECT_NAME
RUN dotnet restore ${PROJECT_NAME}/${PROJECT_NAME}.csproj
RUN dotnet build ${PROJECT_NAME}/${PROJECT_NAME}.csproj -c Release -o /app
RUN dotnet publish ${PROJECT_NAME}/${PROJECT_NAME}.csproj -c Release -o /app

# stage: production runtime environen 
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS production
ARG PROJECT_NAME
EXPOSE 49776
EXPOSE 44336
WORKDIR /app
COPY --from=build /app .
ENV PROJECT_NAME=$PROJECT_NAME
ENTRYPOINT dotnet ${PROJECT_NAME}.dll
