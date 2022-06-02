using ChatService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatService.Services;

public class MessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;

    public MessagesService(IOptions<ChatDatabaseSettings> messagesDatabaseSettings)
    {
        var mongoClient = new MongoClient(messagesDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(messagesDatabaseSettings.Value.DatabaseName);

        _messagesCollection = mongoDatabase.GetCollection<Message>(messagesDatabaseSettings.Value.MessagesCollectionName);
    }

    public async Task<List<Message>> GetAsync() =>
        await _messagesCollection.Find(message => true).ToListAsync();

    // get single message async
    public async Task<Message> GetAsync(string id) =>
        await _messagesCollection.Find(message => message.mid == id).FirstOrDefaultAsync();

    // create message async
    public async Task CreateAsync(Message message) =>
        await _messagesCollection.InsertOneAsync(message);

    // update message async
    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await _messagesCollection.ReplaceOneAsync(message => message.mid == id, updatedMessage);

    // delete message async
    public async Task DeleteAsync(string id) =>
        await _messagesCollection.DeleteOneAsync(message => message.mid == id);

}