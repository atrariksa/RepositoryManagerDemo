// https://refactoring.guru/design-patterns/singleton/csharp/example
// https://learn.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=net-6.0

namespace RepositoryManager;

sealed class InMemoryDataHandler {
    private static InMemoryDataHandler INSTANCE;
    private Dictionary<string, Item> data;
    private InMemoryDataHandler() {
        data = new Dictionary<string, Item>();
    }

    public static InMemoryDataHandler GetInstance() {
        if (INSTANCE == null) {
            INSTANCE = new InMemoryDataHandler();
        }
        return INSTANCE;
    }

    private Mutex mut = new Mutex();

    public void AddData(string itemName, string itemContent, int itemType) {
        mut.WaitOne();
        Item item;
        if (data.TryGetValue(itemName, out item)) {
            mut.ReleaseMutex();
            return;
        } else {
            item = new Item();
            item.Name = itemName;
            item.Content = itemContent;
            item.Type = itemType;
            data[itemName] = item;
        }
        mut.ReleaseMutex();
    }

    public Item GetData(string itemName) {
        mut.WaitOne();
        Item item;
        if (data.TryGetValue(itemName, out item)) {
            mut.ReleaseMutex();
            return item;
        }
        mut.ReleaseMutex();
        return item;
    }

    public void RemoveData(string itemName) {
        // Wait until it is safe to enter.
        Console.WriteLine("{0} is requesting the mutex", Thread.CurrentThread.Name);
        mut.WaitOne();
        Console.WriteLine("{0} has entered the protected area", Thread.CurrentThread.Name);

        data.Remove(itemName);
        Thread.Sleep(500);
        Console.WriteLine("{0} is leaving the protected area", Thread.CurrentThread.Name);

        mut.ReleaseMutex();
        Console.WriteLine("{0} has released the mutex", Thread.CurrentThread.Name);
    }
}