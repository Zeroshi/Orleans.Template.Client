FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Orleans.Template.Client.csproj", "Orleans.Template.Client/"]
RUN dotnet restore "Orleans.Template.Client/Orleans.Template.Client.csproj"
COPY . .
WORKDIR "/src/Orleans.Template.Client"
RUN dotnet build "Orleans.Template.Client.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orleans.Template.Client.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orleans.Template.Client.dll"]