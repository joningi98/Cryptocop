FROM mcr.microsoft.com/dotnet/core/sdk:3.1

LABEL maintainer="Jon Ingi Olafsson"

COPY . /app
WORKDIR /app

RUN dotnet restore
RUN dotnet publish -o out
WORKDIR /app/out
ENTRYPOINT [ "dotnet", "cryptocop-payments.dll" ]