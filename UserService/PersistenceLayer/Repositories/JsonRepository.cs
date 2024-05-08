using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Newtonsoft.Json;
using PersistenceLayer.Entities;

namespace PersistenceLayer.Repositories;

public class JsonRepository : ILoginRepository, IRegisterRepository
{
    private const string FILE_PATH = @"./jsonDB.json"; //UserService/jsonDB.json 
    private readonly List<UserEntity> users;

    public JsonRepository()
    {
        Console.WriteLine("Initializing JSON-Based Repository.");
        string jsonContent = File.ReadAllText(FILE_PATH);
        users = JsonConvert.DeserializeObject<List<UserEntity>>(jsonContent)!;

        if (users == null)
        {
            Console.WriteLine("Deserialization returned null.");
            users = [];
        }

        Console.WriteLine("JSON-Based Repository is Ready!.");
    }

    public UserData GetUser(UserData userData)
    {
        for (int i = 0; i < users.Count; i++)
        {
            UserEntity u = users[i];
            if (u.Email == userData.Email) return u.GetUserData();
        }

        return new();
    }

    public bool Register(UserData user)
    {
        foreach (UserEntity t in users)
        {
            if (t.Email == user.Email)
            {
                return false;
            }
        }

        UserEntity entity = new(user);
        users.Add(entity);
        return true;
    }

    public bool IsEmailExisting(UserData request)
    {
        return users.Any(user => user.Email == request.Email);
    }
}
