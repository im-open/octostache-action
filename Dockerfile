# Set the base image as the .NET 6.0 SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

# Copy everything and publish the release
COPY ./src ./
RUN dotnet publish ./OctostacheCmd/OctostacheCmd.csproj -c Release -o out --no-self-contained

# Clean image with just the build output
FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/OctostacheCmd.dll" ]
