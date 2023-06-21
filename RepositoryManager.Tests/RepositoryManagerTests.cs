using Xunit;
using System;
using System.Threading;

namespace RepositoryManager.Tests;

public class RepositoryTests {

    [Fact]
    public void Validator_ValidateJSON() {
        string json = "{\"username\":\"anyusername\"}";
        string invalidJSON = "invalid json";
        int type = 1;
        SimpleValidator validator = new SimpleValidator();
        Assert.True(validator.Validate(json, type));
        Assert.False(validator.Validate(invalidJSON, type));
    }

    [Fact]
    public void Validator_ValidateXML() {
        string xml = "<node><name>anyname</name></node>";
        string invalidXML = "invalid xml";
        int type = 2;
        SimpleValidator validator = new SimpleValidator();
        Assert.True(validator.Validate(xml, type));
        Assert.False(validator.Validate(invalidXML, type));
    }

    [Fact]
    public void Validator_Validate_Non_Existing_Type() {
        string invalidContent = "invalid content";
        int invalidType = 7;
        SimpleValidator validator = new SimpleValidator();
        Assert.False(validator.Validate(invalidContent, invalidType));
    }

    // Tests below is enough using one content type

    [Fact]
    public void Register_Success() {
        string itemName = "json_01";
        string expectedJSON = "{\"username\":\"anyusername\"}";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, expectedJSON, 1);
        Assert.Equal(expectedJSON, repository.Retrieve(itemName));
    }

    [Fact]
    public void Register_Skip_WrongType() {
        string itemName = "json_02";
        string itemContent = "{\"username\":\"anyusername\"}";
        string expectedEmpty = "";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, itemContent, 2);
        Assert.Equal(expectedEmpty, repository.Retrieve(itemName));
    }

    [Fact]
    public void Register_Skip_WrongContent() {
        string itemName = "json_03";
        string expectedEmpty = "";
        string wrongContent = "<node><name>anyname</name></node>";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, wrongContent, 1);
        Assert.Equal(expectedEmpty, repository.Retrieve(itemName));
    }

    [Fact]
    public void Register_Skip_SameItemName() {
        string itemName = "json_04";
        string expectedJSON = "{\"username\":\"anyusername\"}";
        string toOverwriteJSON = "{\"username\":\"none\"}";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, expectedJSON, 1);
        repository.Register(itemName, toOverwriteJSON, 1);
        Assert.Equal(expectedJSON, repository.Retrieve(itemName));
    }

    [Fact]
    public void Retrieve_Found() {
        string itemName = "json_05";
        string expectedJSON = "{\"username\":\"anyusername\"}";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, expectedJSON, 1);
        Assert.Equal(expectedJSON, repository.Retrieve(itemName));
    }

    [Fact]
    public void Retrieve_Not_Found() {
        string itemName = "json_06";
        string expectedEmpty = "";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        Assert.Equal(expectedEmpty, repository.Retrieve(itemName));
    }

    [Fact]
    public void GetType_Found() {
        string itemName = "json_07";
        string json = "{\"username\":\"anyusername\"}";
        int expectedType = 1;
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, json, expectedType);
        Assert.Equal(expectedType, repository.GetType(itemName));
    }

    [Fact]
    public void GetType_Not_Found() {
        string itemName = "json_08";
        int expectedType = 0;
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        Assert.Equal(expectedType, repository.GetType(itemName));
    }

    [Fact]
    public void Deregister() {
        string itemName = "json_09";
        string expectedJSON = "{\"username\":\"anyusername\"}";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, expectedJSON, 1);
        Assert.Equal(expectedJSON, repository.Retrieve(itemName));

        string expectedEmpty = "";
        repository.Deregister(itemName);
        Assert.Equal(expectedEmpty, repository.Retrieve(itemName));
    }

    [Fact]
    public void Not_Implemented_Repository() {
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository;
        try {
            repositoryFactory.GetRepository(MemoryType.MySQL);
        } catch(Exception e) {
            Exception expectedException = new NotImplementedException("This type of repository is not implemented : " + MemoryType.MySQL);
            Assert.Equal(expectedException.GetType(), e.GetType());
        }
    }

    // only on RemoveData which is called by Deregister that logs the process
    [Fact]
    public void Multithreading_Deregister() {
        string itemName = "json_10";
        string expectedJSON = "{\"username\":\"anyusername\"}";
        RepositoryFactory repositoryFactory = new RepositoryFactory();
        IRepository repository = repositoryFactory.GetRepository(MemoryType.InMemoryStorage);
        repository.Register(itemName, expectedJSON, 1);
        Assert.Equal(expectedJSON, repository.Retrieve(itemName));

        // Create the threads that will use the protected resource.
        for(int i = 0; i < 4; i++) {
            Thread thread = new Thread(() => {
                repository.Deregister(itemName);
            });
            thread.Name = String.Format("Thread{0}", i + 1);
            thread.Start();
        }
    }
}