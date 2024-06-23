using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository profileRepository;
        private readonly IAuthorizationService auth;
        private readonly MessageService msgService;

        public ProfileService(IProfileRepository profileRepository, IAuthorizationService auth,
            MessageService msgService)
        {
            this.profileRepository = profileRepository;
            this.auth = auth;
            this.msgService = msgService;
        }

        public async Task<UserData> Get(string token)
        {
            UserData user = await profileRepository.Get(auth.GetId(token));
            return user.Copy();
        }

        public async Task<UserData> GetProfile(long id)
        {
            UserData unsafeUser = await profileRepository.Get(id);
            return unsafeUser.SafeCopy();
        }

        public async Task<UserData> Update(UserData request, string token)
        {
            long id = auth.GetId(token);
            string email = auth.GetEmail(token);
            request.Id = id;
            request.Email = email;
            UserData response = await profileRepository.Update(request);
            return response;
        }

        public async Task<bool> Delete(string token)
        {
            long id = auth.GetId(token);
            PropagateDeletion(id);
            return await profileRepository.Delete(id);
        }

        private void PropagateDeletion(long id)
        {
            MessageData data = MessageFactory.GetDeleteMessage(id);
            msgService.Publish(data.ExchangeName, data.RoutingKey, data.QueueName, data);
        }
    }
}
