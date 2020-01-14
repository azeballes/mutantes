FROM mcr.microsoft.com/dotnet/core/sdk as build

WORKDIR /src

COPY . /src

RUN dotnet publish Mutants.Api -o ../app -c Release

FROM mcr.microsoft.com/dotnet/core/aspnet

WORKDIR /app

COPY --from=build /app /app

ENTRYPOINT dotnet Mutants.Api.dll