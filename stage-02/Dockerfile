FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY *.csproj .
RUN dotnet restore --use-current-runtime  
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# copy everything else and build app
COPY . .
RUN dotnet publish -c release -o /app --sc false --ucr --no-restore
RUN dotnet ef database update

# build db

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
COPY --from=build /root/.local/share/people.db /root/.local/share/people.db
ENTRYPOINT ["dotnet", "hngx-duo.dll"]