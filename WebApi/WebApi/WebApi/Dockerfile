FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY ./publish .
EXPOSE 9000/tcp
ENTRYPOINT ["dotnet", "WebApi.dll"]