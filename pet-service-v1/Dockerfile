#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Petshop/Petshop.csproj", "Petshop/"]
RUN dotnet restore "Petshop/Petshop.csproj"
COPY . .
WORKDIR "/src/Petshop"
RUN dotnet build "Petshop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Petshop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Petshop.dll"]