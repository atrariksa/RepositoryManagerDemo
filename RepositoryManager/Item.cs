namespace RepositoryManager;

class Item {
    private string name;
    public string Name {
        get { return name; }
        set { name = value; }
    }

    private string content;
    public string Content {
        get { return content; }
        set { content = value; }
    }
    private int type;
    public int Type {
        get { return type; }
        set { type = value; }
    }
}