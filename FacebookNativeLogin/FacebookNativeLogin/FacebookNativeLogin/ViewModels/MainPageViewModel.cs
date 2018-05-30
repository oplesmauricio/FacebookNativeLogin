using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using FacebookNativeLogin.Services.Contracts;
using Prism.Services;
using FacebookNativeLogin.Models;

namespace FacebookNativeLogin.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
        #region Propriedades
        private readonly IFacebookManager _facebookManager;
		private readonly IPageDialogService _dialogService;
        private INavigationService _navigationService;

        public DelegateCommand FacebookLoginCommand { get; set; }
		public DelegateCommand FacebookLogoutCommand { get; set; }
        public DelegateCommand NavigateToView1PageCommand { get; private set; }

        private FacebookUser _facebookUser;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isLogedIn;
        public bool IsLogedIn
        {
            get { return _isLogedIn; }
            set { SetProperty(ref _isLogedIn, value); }
        }
        #endregion

        #region Construtores
        public MainPageViewModel(IFacebookManager facebookManager, IPageDialogService dialogService, INavigationService navigationService)
        {
            _facebookManager = facebookManager;
            _dialogService = dialogService;
            _navigationService = navigationService;

            IsLogedIn = false;
            FacebookLoginCommand = new DelegateCommand(FacebookLogin);
            FacebookLogoutCommand = new DelegateCommand(FacebookLogout);
            NavigateToView1PageCommand = new DelegateCommand(NavigateToView1Page);
        }
        #endregion

        #region Metodos Publicos
        public FacebookUser FacebookUser
		{
			get { return _facebookUser; }
			set { SetProperty(ref _facebookUser, value); }
		}

        public void NavigateToView1Page()
        {
            _navigationService.NavigateAsync("HomePage");
        }
        
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"];
        }
        #endregion

        #region Metodos Privados
        private void FacebookLogout()
		{
			_facebookManager.Logout();
			IsLogedIn = false;
		}

		private void FacebookLogin()
		{
			_facebookManager.Login(OnLoginComplete);
        }

		private void OnLoginComplete(FacebookUser facebookUser, string message)
		{
			if (facebookUser != null)
			{
				FacebookUser = facebookUser;
				IsLogedIn = true;
                NavigateToView1PageCommand.Execute();
            }
			else
			{
				_dialogService.DisplayAlertAsync("Deu merda em algum lugar", message, "Ok");
			}
		}
        #endregion
	}
}
