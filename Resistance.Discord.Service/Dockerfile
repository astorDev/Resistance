FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Resistance.Discord.Service/Resistance.Discord.Service.csproj", "Resistance.Discord.Service/"]
COPY ["Resistance.Domain/Resistance.Domain.csproj", "Resistance.Domain/"]
RUN dotnet restore "Resistance.Discord.Service/Resistance.Discord.Service.csproj"
COPY . .
WORKDIR "/src/Resistance.Discord.Service"
RUN dotnet build "Resistance.Discord.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Resistance.Discord.Service.csproj" -c Release -o /app

FROM base AS final

RUN ln -sf /usr/share/zoneinfo/Europe/Moscow /etc/localtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT dotnet Resistance.Discord.Service.dll