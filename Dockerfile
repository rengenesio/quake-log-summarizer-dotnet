FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY . .
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS run
COPY --from=build build/QuakeLogSummarizer.Application/Release/publish /app
EXPOSE 80
ENTRYPOINT ["dotnet", "app/QuakeLogSummarizer.Application.dll"]
