FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleOJ/SimpleOJ.csproj", "SimpleOJ/"]
RUN dotnet restore "SimpleOJ/SimpleOJ.csproj"
COPY . .
WORKDIR "/src/SimpleOJ"
RUN dotnet build "SimpleOJ.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleOJ.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleOJ.dll"]
