FROM mcr.microsoft.com/dotnet/core/sdk:3.1

LABEL maintainer="Jon Ingi Olafsson"

COPY . /app
WORKDIR ./app/Cryptocop.Software.API

RUN dotnet restore
RUN dotnet publish -o out

ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app/Cryptocop.Software.API/out
ENTRYPOINT ["dotnet", "Cryptocop.Software.API.dll"]