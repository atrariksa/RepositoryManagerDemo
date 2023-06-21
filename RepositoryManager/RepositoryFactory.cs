namespace RepositoryManager;

public enum MemoryType 
{
  InMemoryStorage,
  MySQL
}


public class RepositoryFactory {
    public IRepository GetRepository(MemoryType type) {
        switch (type) {
            case MemoryType.InMemoryStorage:
                return new InMemoryRepository();
            default:
                throw new NotImplementedException("This type of repository is not implemented : " + type);
        }
    }
}