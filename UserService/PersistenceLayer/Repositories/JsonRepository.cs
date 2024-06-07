using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Newtonsoft.Json;
using PersistenceLayer.Entities;

namespace PersistenceLayer.Repositories;

public class JsonRepository : ILoginRepository, IRegisterRepository, IProfileRepository
{
    private const string FILE_PATH = @"./jsonDB.json"; //UserService/jsonDB.json 
    private static readonly Dictionary<long, UserEntity> users = new();
    private static long currentId = 0;

    public JsonRepository()
    {
        Console.WriteLine("Initializing JSON-Based Repository.");
        string jsonContent = File.ReadAllText(FILE_PATH);
        var userEntities = JsonConvert.DeserializeObject<List<UserEntity>>(jsonContent)!;

        foreach (UserEntity userEntity in userEntities)
        {
            if (currentId <= userEntity.Id)
            {
                currentId = userEntity.Id + 1;
            }

            users.TryAdd(userEntity.Id, userEntity);
        }

        Console.WriteLine("JSON-Based Repository is Ready!.");
    }

    public async Task<UserData> GetUser(UserData userData)
    {
        foreach (UserEntity u in users.Values)
        {
            if (u.Email == userData.Email) return u.GetUserData();
        }

        return new();
    }

    public bool Register(UserData user)
    {
        foreach (UserEntity t in users.Values)
        {
            if (t.Email == user.Email)
            {
                return false;
            }
        }

        UserEntity entity = new(user)
        {
            Id = currentId
        };

        users.Add(entity.Id, entity);
        currentId++;
        return true;
    }

    public bool IsEmailExisting(UserData request)
    {
        return users.Values.Any(user => user.Email == request.Email);
    }

    public async Task<UserData> Get(long userId)
    {
        try
        {
            return users.TryGetValue(userId, out UserEntity? user) ? user.GetUserData() : new();
        }
        catch
        {
            Console.WriteLine("issue in repo");
            return new();
        }
    }

    public async Task<UserData> Update(UserData request)
    {
        UserEntity entity = new(request);
        return entity.GetUserData();
    }

    public async Task<bool> Delete(long getId)
    {
        return users.Remove(getId);
    }
}
