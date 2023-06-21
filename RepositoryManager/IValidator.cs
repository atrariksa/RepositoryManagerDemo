namespace RepositoryManager;

public interface IValidator {
    bool Validate(string content, int type);
}