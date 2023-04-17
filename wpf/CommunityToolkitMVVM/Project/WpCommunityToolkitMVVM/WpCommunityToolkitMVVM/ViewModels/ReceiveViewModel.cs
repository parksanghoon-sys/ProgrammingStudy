using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpCommunityToolkitMVVM.Models;

namespace WpCommunityToolkitMVVM.ViewModels
{
    public partial class ReceiveViewModel : ObservableObject, IRecipient<ValueChangedMessage<User>>
    {
        public ReceiveViewModel()
        {
            //WeakReferenceMessenger.Default.Register<User>(this, (o, m) => OnUserChangedMessage(m));
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(ValueChangedMessage<User> message)
        {
            throw new NotImplementedException();
        }

        private void OnUserChangedMessage(User m)
        {
            throw new NotImplementedException();
        }
    }
}
