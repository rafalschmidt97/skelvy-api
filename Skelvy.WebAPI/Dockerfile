FROM microsoft/dotnet:2.2-aspnetcore-runtime AS Base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build

# Restore dotnet before build to allow for caching
WORKDIR /
COPY Skelvy.Application/Skelvy.Application.csproj /src/Skelvy.Application/
COPY Skelvy.Domain/Skelvy.Domain.csproj /src/Skelvy.Domain/
COPY Skelvy.Common/Skelvy.Common.csproj /src/Skelvy.Common/
COPY Skelvy.Infrastructure/Skelvy.Infrastructure.csproj /src/Skelvy.Infrastructure/
COPY Skelvy.Persistence/Skelvy.Persistence.csproj /src/Skelvy.Persistence/
COPY Skelvy.WebAPI/Skelvy.WebAPI.csproj /src/Skelvy.WebAPI/

RUN dotnet restore /src/Skelvy.WebAPI/Skelvy.WebAPI.csproj

# Copy source files and build
COPY . /src

RUN dotnet build /src/Skelvy.WebAPI/Skelvy.WebAPI.csproj --no-restore -c Release
RUN dotnet publish /src/Skelvy.WebAPI/Skelvy.WebAPI.csproj --no-restore -c Release -o /app

# Copy compiled app to runtime container
FROM base AS final
COPY --from=build /app .
CMD ["dotnet", "Skelvy.WebAPI.dll"]
