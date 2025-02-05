using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Вторая_попытка
{
    public partial class Empl : Form
    {

        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        public Empl()
        {
            connection = new SqlConnection("Data Source=WIN-835DGRUVKSH;Initial Catalog=Restoran;Integrated Security=True");

            InitializeComponent();
            InitializeDatabase();
            LoadData();
            PopulateComboBoxes();

        }

        private void InitializeDatabase()
        {
            // Укажите вашу строку подключения к базе данных
            string connectionString = "Data Source=WIN-835DGRUVKSH;Initial Catalog=Restoran;Integrated Security=True";
            connection = new SqlConnection(connectionString);
        }


        private void LoadData()
        {
            try
            {


                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT * FROM Сотрудник", connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // dataGridView - это ваш DataGridView на форме

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eгor: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        private void SetNextEmplID()
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                // Получаем последнее значение ID_сотрудника из DataTable
                int lastID = dataTable.AsEnumerable()
                    .Max(row => row.Field<int>("ID_сотрудника"));

                // Увеличиваем последнее значение на 1
                int nextID = lastID + 1;

                // Подставляем новое значение в TextBox1
                textBox1.Text = nextID.ToString();
            }
            else
            {
                // Если таблица пуста, начинаем с 1
                textBox1.Text = "1";
            }
        }


        private void Empl_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "restoranDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.restoranDataSet.Сотрудник);

        }
        private void PopulateComboBoxes()
        {
            // Заполнение выпадающего меню для номеров столиков
            comboBox1.Items.AddRange(new string[] { "1", "2", "3"});
            comboBox1.SelectedIndex = 0;
        }










        private void buttonCreateEmpl_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox1.Text);
            string Fam = textBox2.Text;
            string Name = textBox3.Text;
            string Otch = textBox4.Text;
            int idrole = int.Parse(comboBox1.Text);
            string Tel = textBox5.Text;
            string Log = textBox6.Text;
            string Pass = textBox7.Text;



            // Проверка на корректность ввода
            if (string.IsNullOrEmpty(Fam) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Pass) || string.IsNullOrEmpty(Tel) || string.IsNullOrEmpty(Log))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            //

            // Добавление нового Сотрудника в базу данных
            string query = "INSERT INTO Сотрудник (ID_сотрудника, Фамилия, Имя, Отчество, ID_роли, Телефон, Логин, Пароль) VALUES (@id, @Fam, @Name, @Otch, @idrole, @Tel, @Log, @Pass)";

            //SqlCommand command = new SqlCommand(query, connection);

            // command.Parameters.AddWithValue("@id", int.Parse(textBox1.Text)); // Используем автоматически сгенерированный ID            command.Parameters.AddWithValue("@name", name);
            //command.Parameters.AddWithValue("@cost", cost);
            //command.Parameters.AddWithValue("@price", price);

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", int.Parse(textBox1.Text)); // Используем автоматически сгенерированный ID
                    command.Parameters.AddWithValue("@Fam", Fam);
                    command.Parameters.AddWithValue("@Name", Name); // Преобразуем в decimal
                    command.Parameters.AddWithValue("@Otch", Otch); // Преобразуем в decimal
                    command.Parameters.AddWithValue("@idrole", idrole); // Используем автоматически сгенерированный ID
                    command.Parameters.AddWithValue("@Tel", Tel);
                    command.Parameters.AddWithValue("@Log", Log); // Преобразуем в decimal
                    command.Parameters.AddWithValue("@Pass", Pass); // Преобразуем в decimal

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Сотрудник успешно добавлен!");
                    LoadData(); // Обновляем DataGridView
                    ClearTextBoxes();
                    SetNextEmplID(); // Устанавливаем следующий ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении сотрудника: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }


        }
        private void ClearTextBoxes()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }








        private void buttonDeleteEmpl_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // Получаем выделенную строку
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Формируем SQL-запрос на основе всех значений строки
                    StringBuilder queryBuilder = new StringBuilder("DELETE FROM Сотрудник WHERE ");
                    for (int i = 0; i < selectedRow.Cells.Count; i++)
                    {
                        string columnName = dataGridView1.Columns[i].Name;
                        object cellValue = selectedRow.Cells[i].Value;

                        if (cellValue != null)
                        {
                            if (i > 0)
                            {
                                queryBuilder.Append(" AND ");
                            }
                            queryBuilder.Append($"{columnName} = @{columnName}");
                        }
                    }

                    string query = queryBuilder.ToString();

                    // Создаем команду и добавляем параметры
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        for (int i = 0; i < selectedRow.Cells.Count; i++)
                        {
                            string columnName = dataGridView1.Columns[i].Name;
                            object cellValue = selectedRow.Cells[i].Value;

                            if (cellValue != null)
                            {
                                command.Parameters.AddWithValue($"@{columnName}", cellValue);
                            }
                        }

                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        connection.Close();
                        if (result > 0)
                        {
                            MessageBox.Show("Запись сотрудника успешно удалена.");
                            LoadData(); // Обновляем данные в DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Запись о сотруднике не найдена.");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }




        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Main M = new Main();
            M.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 F1 = new Form1();
            F1.Show();
        }


    }
}
