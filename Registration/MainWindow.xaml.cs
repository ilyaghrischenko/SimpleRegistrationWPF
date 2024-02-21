using System.IO;
using System.Text.Json;
using System.Windows;

namespace Registration
{
    public partial class MainWindow : Window
    {
        private string path = "RegisteredUsers.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        private List<User> LoadRegisteredUsers()
        {
            List<User> registeredUsers = new List<User>();

            if (!File.Exists(path))
            {
                File.Create(path);
                return registeredUsers;
            }

            string jsonString = File.ReadAllText(path);
            if (jsonString == string.Empty) return registeredUsers;

            try
            {
                registeredUsers = JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return registeredUsers;
        }

        private void LogInEvent(object sender, RoutedEventArgs e)
        {
            string login = loginInput.Text;
            string password = passInput.Password;

            User user = new();
            try
            {
                user.Login = login;
                user.Password = password;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }

            List<User> registeredUsers = new();
            try
            {
                registeredUsers = LoadRegisteredUsers();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            bool isFind = false;
            foreach (var item in registeredUsers)
            {
                if (item.Login == user.Login && item.Password == user.Password) isFind = true;
            }

            if (isFind) MessageBox.Show("You're logged in");
            else MessageBox.Show("This user does not exists");
        }

        private void RegistrationEvent(object sender, RoutedEventArgs e)
        {
            string login = loginInput.Text;
            string password = passInput.Password;

            List<User> registeredUsers = new();
            try
            {
                registeredUsers = LoadRegisteredUsers();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                User user = new(login, password);
                if (!user.IsValid(registeredUsers)) throw new ArgumentException("Error: This login already exists");
                registeredUsers.Add(user);

                string jsonString = JsonSerializer.Serialize(registeredUsers);
                MessageBox.Show(jsonString);
                File.WriteAllText(path, jsonString);

                MessageBox.Show("You have been successfuly registered!");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}