
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
USER root
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TTechEcommerceApi/TTechEcommerceApi.csproj", "TTechEcommerceApi/"]
COPY ["TTechEcommerce.BLL/TTechEcommerce.BLL.csproj", "TTechEcommerce.BLL/"]
COPY ["TTechEcommerce.DAL/TTechEcommerce.DAL.csproj", "TTechEcommerce.DAL/"]
RUN dotnet restore "TTechEcommerceApi/TTechEcommerceApi.csproj"
COPY . .
WORKDIR "/src/TTechEcommerceApi"
RUN dotnet build "TTechEcommerceApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TTechEcommerceApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
#RUN apk update && apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TTechEcommerceApi.dll"]