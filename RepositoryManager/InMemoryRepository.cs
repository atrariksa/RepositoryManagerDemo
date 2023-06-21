namespace RepositoryManager;

public sealed class InMemoryRepository : IRepository {
    private IValidator validator;
    private InMemoryDataHandler dataHandler;

    public InMemoryRepository() {
        dataHandler = InMemoryDataHandler.GetInstance();
        validator = new SimpleValidator(); 
    }

    public void Register(string itemName, string itemContent, int itemType) {
        if (!validator.Validate(itemContent, itemType)) {
            return;
        }
        dataHandler.AddData(itemName, itemContent, itemType);
    }

    public string Retrieve(string itemName) {
        if (dataHandler.GetData(itemName) != null) {
            return dataHandler.GetData(itemName).Content;
        }
        return "";
    }

    public int GetType(string itemName) {
        if (dataHandler.GetData(itemName) != null) {
            return dataHandler.GetData(itemName).Type;
        }
        return 0;
    }

    public void Deregister(string itemName) {
        dataHandler.RemoveData(itemName);
    }

    public void Initialize() {
    }
}