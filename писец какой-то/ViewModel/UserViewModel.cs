using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    internal class UserViewModel : INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public static UserViewModel userViewModel;
        private AddUser addUser = AddUser.addUser;
        private DeleteUser deleteUser = DeleteUser.deleteUser;
        public static DataBaseModel dataBaseModel = new DataBaseModel();
        private ObservableCollection<UserModel> users;
        public ObservableCollection<UserModel> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }


        private UserModel selectedUser;
        public UserModel SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }
        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
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

        private int accessLevel;
        public int AccessLevel
        {
            get => accessLevel;
            set
            {
                if (IsValidAccessLevel(value))
                {
                    accessLevel = value;
                    OnPropertyChanged(nameof(AccessLevel));
                }
                else
                {
                    accessLevel = 2;
                }
            }
        }

        public string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private UserModel CurrentUser;
        public string Name
        {
            get => CurrentUser.FirstName;
        }
        public UserViewModel()
        {
            userViewModel = this;
            LoadCurrentUser();
            Visible();
            UpdateUserCollection();
            AddUserFormCommand = new RelayCommand(OpenAddUserForm);
            AddUserCommand = new RelayCommand(AddNewUser);
            Users = new ObservableCollection<UserModel>(dataBaseModel.GetAllUsers());
            DeleteUserFormCommand = new RelayCommand(OpenDeleteUserForm);
            DeleteUserCommand = new RelayCommand(DeletedUser);
        }

        public ICommand DeleteUserCommand { get; private set; }
        public void DeletedUser(object parameter)
        {
            if (dataBaseModel.LoginExists(Login))
            {
                dataBaseModel.Delete(Login, FirstName, LastName);
                UpdateUserCollection();
                ErrorMessage = "You have successfully deleted the account. Re-visit the page to see the current user list.";
            }
            else
            {
                ErrorMessage = "This user does not exist for deletion.";
            }
        }

        public ICommand DeleteUserFormCommand { get; private set; }
        public void OpenDeleteUserForm(object parameter)
        {
            deleteUser = new DeleteUser();
            deleteUser.ShowDialog();
        }
        public ICommand AddUserFormCommand { get; private set; }
        private void OpenAddUserForm(object parameter)
        {
            addUser = new AddUser();
            addUser.ShowDialog();
        }
        public ICommand AddUserCommand { get; private set; }
        private void UpdateUserCollection()
        {
            Users = new ObservableCollection<UserModel>(dataBaseModel.GetAllUsers());
        }
        private void AddNewUser(object parameter)
        {
            if (!dataBaseModel.LoginExists(Login))
            {
                dataBaseModel.CreateUser(Login, Password, FirstName, LastName);
                UpdateUserCollection();
                ErrorMessage = "User successfully registered.";
            }
            else
            {
                ErrorMessage = "User already registered.";
            }
        }
        private bool IsValidAccessLevel(int level)
        {
            return level == 2 || level == 1;
        }

        public Visibility level = Visibility.Hidden;
        public Visibility levelVisibility
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged(nameof(level));
            }
        }
        public int CurrentAccessLevel
        {
            get => CurrentUser.AccessLevel;
        }
        private void LoadCurrentUser()
        {
            LoginViewModel loginViewModel = LoginViewModel.loginViewModel;
            CurrentUser = loginViewModel.CurrentUser;
        }
        private void Visible()
        {
            if (CurrentAccessLevel == 1)
            {
                levelVisibility = Visibility.Visible;
            }
        }
    }
}
