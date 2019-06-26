#
# Web Development Module
# Web Practicals
# Project Dockerfile 
#


# stage development environment
FROM microsoft/dotnet:2.2-sdk AS develop
WORKDIR /src
COPY . .
# dotnet watch requires these directories to be accessable
RUN mkdir /.dotnet && chmod a=rwx /.dotnet
RUN mkdir /.nuget && chmod a=rwx /.nuget
RUN chmod a=rwx /tmp
ENTRYPOINT dotnet watch --project folio/folio.csproj run

# stage: build project
FROM develop AS build
ARG PROJECT_NAME=folio
RUN dotnet restore ${PROJECT_NAME}.csproj
RUN dotnet build ${PROJECT_NAME}.csproj -c Release -o /app
RUN dotnet publish ${PROJECT_NAME}.csproj -c Release -o /app

# stage: production runtime environen 
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS production
EXPOSE 49776
EXPOSE 44336
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT dotnet ${PROJECT_NAME}.dll
