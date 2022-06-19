using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IServerApi _serverApi;

        private string _errorMsgHolder;

        public string Token { get; set; }
        public string ErrorMsgHolder
        {
            get => _errorMsgHolder;
            set
            {
                _errorMsgHolder = value;
                OnPropertyChanged(nameof(ErrorMsgHolder));
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IServerApi serverAPI)
        {
            _serverApi = serverAPI;
            LoginCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(FindHostPage)}"));
            ForgotPasswordCommand = new Command(() => { });
        }

        private async void connect()
        {
            ErrorMsgHolder = string.Empty;

            await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}");

            //if (await _serverApi.LoginAsync(Token))
            //{
            //    await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}");
            //}
            //else
            //{
            //    ErrorMsgHolder = "Error occurred";
            //}
        }
    }
}
