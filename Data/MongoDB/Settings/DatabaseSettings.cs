using MongoDB.Driver;

public class DatabaseSettings
{
    public static string? ConnectionString { get; set; }
    public static string? DatabaseName { get; set; }
    public static bool IsSSL { get; set; }
    public static string? CollectionName { get; set; }

    public IMongoDatabase _database { get; }

    public DatabaseSettings()
    {
        try
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));

            if (IsSSL)
                settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

            var mongoClient = new MongoClient(settings);

            _database = mongoClient.GetDatabase(DatabaseName);
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to connect to server", ex);
        }
    }

    public IMongoCollection<Todo> Todos =>
        _database.GetCollection<Todo>(CollectionName);
}