FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./MiniToDo.csproj", "src/"]
RUN dotnet restore "src/MiniToDo.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "MiniToDo.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "MiniToDo.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MiniToDo.dll
