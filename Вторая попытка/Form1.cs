using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Вторая_попытка
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=WIN-835DGRUVKSH;Initial Catalog=Restoran;Integrated Security=True"; 

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string username = LogBox.Text;
            string password = PassBox.Text;

            if (ValidateUser(username, password))
            {
                // Переход на вторую форму
                Main form2 = new Main();
                form2.Show();
                this.Hide(); // Скрыть первую форму
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль.");
            }
        }

        private bool ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Сотрудник WHERE Логин = @username AND Пароль = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count == 1; // Если пользователь найден, вернуть true
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main form2 = new Main();
            form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }
    }
}
