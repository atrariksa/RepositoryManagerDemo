# Repositor Manager (C#)
Following steps from : https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test
- Create demo solution :
dotnet new sln -o RepositoryManagerDemo

- Go to RepositoryManagerDemo : cd RepositoryManagerDemo

- Create new classlib with name RepositoryManager :
dotnet new classlib --name RepositoryManager

- add the class library project to the solution :
dotnet sln add ./RepositoryManager/RepositoryManager.csproj

- Create Unit Test :
dotnet new xunit -o RepositoryManager.Tests

- Add the test project to the solution :
dotnet sln add ./RepositoryManager.Tests/RepositoryManager.Tests.csproj

- Add class library as a dependency :
dotnet add ./RepositoryManager.Tests/RepositoryManager.Tests.csproj reference ./RepositoryManager/RepositoryManager.csproj

- Run Test on Folder “RepositoryManagerDemo” :
dotnet test
