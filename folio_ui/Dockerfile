#
# Web Development Module
# Web Practicals
# Folio UI
#

# project to build the image from 
ARG PROJECT_NAME=folio_ui

# stage: setup build runtime
FROM microsoft/dotnet:2.2-sdk AS build-runtime
ARG PROJECT_NAME
# install node.js & npm
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash - \
    && apt-get install -y nodejs
# copy source code
WORKDIR /src
COPY . .
WORKDIR /src/${PROJECT_NAME}
# setup home directory
RUN chmod a=rwx /home
ENV HOME=/home
# install npm modules
RUN cd /src/${PROJECT_NAME}/Views && npm install

# stage development environment
FROM build-runtime AS develop
ARG PROJECT_NAME
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
FROM build-runtime AS build
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
