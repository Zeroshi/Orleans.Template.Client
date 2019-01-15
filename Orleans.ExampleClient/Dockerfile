FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Orleans.ExampleClient/Orleans.ExampleClient.csproj", "Orleans.ExampleClient/"]
RUN dotnet restore "Orleans.ExampleClient/Orleans.ExampleClient.csproj"
COPY . .
WORKDIR "/src/Orleans.ExampleClient"
RUN dotnet build "Orleans.ExampleClient.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orleans.ExampleClient.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orleans.ExampleClient.dll"]