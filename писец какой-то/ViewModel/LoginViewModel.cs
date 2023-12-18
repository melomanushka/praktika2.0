using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using писец_какой_то.Model;
using писец_какой_то.View;

namespace писец_какой_то.ViewModel
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public static LoginViewModel loginViewModel;
        public LoginView loginView = LoginView.loginView;
        public LoginPage loginPage = LoginPage.loginPage;
        public static DataBaseModel dataBaseModel = new DataBaseModel();
  
        private UserModel currentUser;
        public UserModel CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(currentUser));
            }
        }
        public LoginViewModel()
        {
            loginViewModel = this;
            LoginCommand = new RelayCommand(SignIn, CanSignIn);
            RegCommand = new RelayCommand(SignUp, CanSignUp);
        }
        private string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }


        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }


        private string firstPasswordForSignUp;
        public string FirstPasswordForSignUp
        {
            get => firstPasswordForSignUp;
            set
            {
                firstPasswordForSignUp = value;
                OnPropertyChanged(nameof(FirstPasswordForSignUp));
            }
        }


        private string secondPasswordForSignUp;
        public string SecondPasswordForSignUp
        {
            get => secondPasswordForSignUp;
            set
            {
                secondPasswordForSignUp = value;
                OnPropertyChanged(nameof(SecondPasswordForSignUp));
            }
        }

        public ICommand LoginCommand { get; }
        private bool CanSignIn(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }
        private void SignIn(object parameter)
        {
            if (dataBaseModel.IsValidLogin(Login, Password))
            {
                CurrentUser = dataBaseModel.GetUserByLogin(Login);
                MainWindow window = new MainWindow();
                window.Show();
                loginView.Close();
            }
            else
            {
                ErrorMessage = "*Invalid username or password*";
            }
        }

        public ICommand RegCommand { get; }

        private bool CanSignUp(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(FirstPasswordForSignUp) && !string.IsNullOrWhiteSpace(SecondPasswordForSignUp);
        }

        private void SignUp(object parameter)
        {
            if (dataBaseModel.LoginExists(Login))
            {
                ErrorMessage = "*This user already exists*";
            }
            else if (FirstPasswordForSignUp != SecondPasswordForSignUp)
            {
                ErrorMessage = "*The first password does not match the second password*";
            }
            else
            {
                dataBaseModel.CreateUser(Login, FirstPasswordForSignUp, FirstName, LastName);
                Clear();
                ErrorMessage = "*You have successfully registered, try logging in*";
            }
        }
        public void Clear()
        {
            Login = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            FirstPasswordForSignUp = string.Empty;
            SecondPasswordForSignUp = string.Empty;
        }
    }
}
