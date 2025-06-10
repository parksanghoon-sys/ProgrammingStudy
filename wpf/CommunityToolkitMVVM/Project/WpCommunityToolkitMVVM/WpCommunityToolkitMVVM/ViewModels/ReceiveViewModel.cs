using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpCommunityToolkitMVVM.Models;

namespace WpCommunityToolkitMVVM.ViewModels
{
    public partial class ReceiveViewModel : ObservableRecipient, IRecipient<AsyncRequestMessage<ProductChangedMessage>>
    {
        [ObservableProperty]
        private ObservableCollection<User> _users;
        public ReceiveViewModel()
        {
            Users = new();

            //WeakReferenceMessenger.Default.Register(this);
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<User>>(this, HandleMessage);
            //WeakReferenceMessenger.Default.Register<ValueChangedMessage<ProductChangedMessage>>(this, HandleMessage2);
            IsActive = true;
        }

        private void HandleMessage2(object recipient, ValueChangedMessage<ProductChangedMessage> message)
        {            
            
        }
        private async Task<ProductChangedMessage> ProvideValueAsync()
        {
            ProductChangedMessage value = new ProductChangedMessage("TEST");

            await Task.Delay(2000);
            return value;
        }
        private void HandleMessage(object recipient, ValueChangedMessage<User> message)
        {
            Users.Add(message.Value);
        }

        public void Receive(ValueChangedMessage<User> message)
        {
            Users.Add(message.Value);
        }

        private void OnUserChangedMessage(User m)
        {
            Users.Add(m);
        }

        public void Receive(ProductChangedMessage message)
        {
            throw new NotImplementedException();
        }

        public void Receive(User message)
        {
            Users.Add(message);
        }

        public void Receive(AsyncRequestMessage<ProductChangedMessage> message)
        {
            message.Reply(ProvideValueAsync());
        }
    }
}
