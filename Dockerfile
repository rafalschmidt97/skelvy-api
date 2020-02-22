FROM microsoft/dotnet:3.1-aspnetcore-runtime AS Base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:3.1-sdk AS build

# Restore dotnet before build to allow for caching
WORKDIR /
COPY StyleCop.json ./
COPY CodeAnalysis.ruleset ./
COPY Skelvy.CodeAnalysis.targets ./
COPY src/Skelvy.WebAPI/Skelvy.WebAPI.csproj /src/Skelvy.WebAPI/
COPY src/Skelvy.Application/Skelvy.Application.csproj /src/Skelvy.Application/
COPY src/Skelvy.Domain/Skelvy.Domain.csproj /src/Skelvy.Domain/
COPY src/Skelvy.Common/Skelvy.Common.csproj /src/Skelvy.Common/
COPY src/Skelvy.Infrastructure/Skelvy.Infrastructure.csproj /src/Skelvy.Infrastructure/
COPY src/Skelvy.Persistence/Skelvy.Persistence.csproj /src/Skelvy.Persistence/

RUN dotnet restore /src/Skelvy.WebAPI/Skelvy.WebAPI.csproj

# Copy source files and build
COPY . ./

RUN dotnet build /src/Skelvy.WebAPI/Skelvy.WebAPI.csproj --no-restore -c Release
RUN dotnet publish /src/Skelvy.WebAPI/Skelvy.WebAPI.csproj --no-restore -c Release -o /app

# Copy compiled app to runtime container
FROM base AS final
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Skelvy.WebAPI.dll"]
