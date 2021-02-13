#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Site/Presentation Layer/OBForumPostAPI/OBForumPostAPI.csproj", "Site/Presentation Layer/OBForumPostAPI/"]
COPY ["Infrastructure Layer/Persistence/OBForumPost.Persistence/OBForumPost.Persistence.csproj", "Infrastructure Layer/Persistence/OBForumPost.Persistence/"]
COPY ["Domain Layer/OBForum.Domain/OBForumPost.Domain.csproj", "Domain Layer/OBForum.Domain/"]
COPY ["Site/Application Layer/OBForm.Application/OBFormPost.Application.csproj", "Site/Application Layer/OBForm.Application/"]
RUN dotnet restore "Site/Presentation Layer/OBForumPostAPI/OBForumPostAPI.csproj"
COPY . .
WORKDIR "/src/Site/Presentation Layer/OBForumPostAPI"
RUN dotnet build "OBForumPostAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OBForumPostAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OBForumPostAPI.dll"]