FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . ./TodoApp
RUN dotnet restore

WORKDIR /app/TodoApp
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/TodoApp/out ./
ENTRYPOINT ["dotnet", "TodoApp.dll"]