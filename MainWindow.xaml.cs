
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1final
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptorBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password.");
                return;
            }
            string encryptedPassword = Encryptor.EncryptPassword(password);
            EncryptedPasswordTextBox.Text = encryptedPassword;

            PasswordTextBox.Text = string.Empty;  

            MessageBox.Show($"Password encrypted successfully.");
        }

        private async void BFattackBtn_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPassword = EncryptedPasswordTextBox.Text;
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                MessageBox.Show("Please enter or generate an encrypted password first.");
                return;
            }

            if (!int.TryParse(MaxThreadsTextBox.Text, out int maxThreads) || maxThreads <= 0)
            {
                MessageBox.Show("Please enter a valid number of threads.");
                return;
            }

            BFattackBtn.IsEnabled = false;
            StatusTextBlock.Text = "Brute force attack in progress...";
            StatusTextBlock.Foreground = System.Windows.Media.Brushes.Black;

            var stopwatch = Stopwatch.StartNew();

            string? result = await Task.Run(() => BFattack.PerformBruteForceAttack(encryptedPassword, maxThreads));

            stopwatch.Stop();

            if (result != null)
            {
                ResultTextBlock.Text = $"Primary password found: {result}";
                ResultTextBlock.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                ResultTextBlock.Text = "Password not found!";
                ResultTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }

            TimeElapsedTextBlock.Text = $"Time elapsed: {stopwatch.Elapsed.TotalSeconds} seconds";

            StatusTextBlock.Text = "";
            BFattackBtn.IsEnabled = true;
        }

        private void EncryptedPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EncryptedPasswordTextBox.Text))
            {
                StatusTextBlock.Text = "Please enter an encrypted password.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                BFattackBtn.IsEnabled = false;
            }
            else
            {
                StatusTextBlock.Text = "";
                BFattackBtn.IsEnabled = true;
            }
        }
    }
}
