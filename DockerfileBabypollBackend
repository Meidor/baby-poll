FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./backend/babypoll.csproj", ""]
RUN dotnet restore "./babypoll.csproj"
COPY ./backend .
RUN dotnet build "babypoll.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "babypoll.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir db
ENTRYPOINT ["dotnet", "babypoll.dll"]