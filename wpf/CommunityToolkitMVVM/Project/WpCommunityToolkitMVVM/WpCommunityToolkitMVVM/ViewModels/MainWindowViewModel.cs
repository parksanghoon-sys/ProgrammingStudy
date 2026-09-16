using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpCommunityToolkitMVVM.Models;

namespace WpCommunityToolkitMVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanLogin))]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string? _id;

        [ObservableProperty]
        private object _userView;
        public bool CanLogin => !string.IsNullOrWhiteSpace(Id) && Id.Length >= 3;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(22000);
                Id = "Complete";
            }
            catch (TaskCanceledException)
            {
                Id = "Fail";
            }

        }
        [RelayCommand]
        private async Task Add(Tuple<string, string> input)
        {
            User user = new User().createUser(input.Item1,input.Item2);
            ProductChangedMessage productChangedMessage = new ProductChangedMessage("TEST");
            //AsyncRequestMessage<User> message = new AsyncRequestMessage<User>();
            //await WeakReferenceMessenger.Default.Send(message);
            //var response = await message.Response;

            ValueChangedMessage<User> message = new ValueChangedMessage<User>(user);
            WeakReferenceMessenger.Default.Send(message);

            AsyncRequestMessage<ProductChangedMessage> message2 = new AsyncRequestMessage<ProductChangedMessage>();
            await WeakReferenceMessenger.Default.Send(message2);
            var resopns = await message2.Response;
        }
        [RelayCommand]
        private void Cancel()
        {
            LoginCommand.Cancel();
        }
        //private string? _id;

        //[NotifyDataErrorInfo]
        //[MinLength(3)]        
        //public string? Id
        //{
        //    get =>_id;
        //    set => SetProperty(ref _id, value);
        //}
        public MainWindowViewModel()
        {
            UserView = Ioc.Default.GetService<ReceiveViewModel>();
        }


    }
}
