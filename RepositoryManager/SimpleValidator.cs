namespace RepositoryManager;

public class SimpleValidator : IValidator {
    private JSONValidator jSONValidator = new JSONValidator();
    private XMLValidator xMLValidator = new XMLValidator();
    public bool Validate(string content, int type) {
        switch (type) {
            case 1 : 
                return jSONValidator.Validate(content);
            case 2 : 
                return xMLValidator.Validate(content);
            default:
                return false;
        }
    }
}

class JSONValidator {
    public bool Validate(string content) {
        // Simple validation that only check "{" at first char.
        if (content.StartsWith("{")) {
            return true;
        }
        return false;
    }
}

class XMLValidator {
    public bool Validate(string content) {
        // Simple validation that only check "<" at first char
        if (content.StartsWith("<")) {
            return true;
        }
        return false;
    }
}