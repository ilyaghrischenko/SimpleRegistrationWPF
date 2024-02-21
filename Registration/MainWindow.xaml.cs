using System.IO;
using System.Text.Json;
using System.Windows;

namespace Registration
{
    public partial class MainWindow : Window
    {
        private string path = "bin\\Debug\\net8.0-windows\\RegisteredUsers.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        private List<User> LoadRegisteredUsers()
        {
            List<User> registeredUsers = new List<User>();

            if (!File.Exists(path)) throw new FileNotFoundException();

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

            MessageBox.Show("You're logged in");
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
            catch (FileNotFoundException)
            {
                MessageBox.Show("Error: File not found!");
                return;
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