using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Вторая_попытка
{
    public partial class App : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;

        public App()
        {            
            connection = new SqlConnection("Data Source=WIN-835DGRUVKSH;Initial Catalog=Restoran;Integrated Security=True");

            InitializeComponent();
            InitializeDatabase();
            LoadData();
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
                dataAdapter = new SqlDataAdapter("SELECT * FROM Блюдо", connection);
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


        private void SetNextDishID()
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                // Получаем последнее значение ID_блюда из DataTable
                int lastID = dataTable.AsEnumerable()
                    .Max(row => row.Field<int>("ID_блюда"));

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




        private void App_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "restoranDataSet.Блюдо". При необходимости она может быть перемещена или удалена.
            this.блюдоTableAdapter.Fill(this.restoranDataSet.Блюдо);

        }

        



        private void deleteButton_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // Получаем выделенную строку
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Формируем SQL-запрос на основе всех значений строки
                    StringBuilder queryBuilder = new StringBuilder("DELETE FROM Блюдо WHERE ");
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
                            MessageBox.Show("Запись была успешно удалена.");
                            LoadData(); // Обновляем данные в DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена.");
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








        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text;
            string cost = textBox3.Text;
            string price = textBox4.Text;

            // Проверка на корректность ввода
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(cost) || string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            //

            // Добавление нового блюда в базу данных
            string query = "INSERT INTO Блюдо (ID_блюда, Название, Себестоимость, Цена) VALUES (@id, @name, @cost, @price)";
            
            //SqlCommand command = new SqlCommand(query, connection);

            // command.Parameters.AddWithValue("@id", int.Parse(textBox1.Text)); // Используем автоматически сгенерированный ID            command.Parameters.AddWithValue("@name", name);
            //command.Parameters.AddWithValue("@cost", cost);
            //command.Parameters.AddWithValue("@price", price);

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", int.Parse(textBox1.Text)); // Используем автоматически сгенерированный ID
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@cost", decimal.Parse(cost)); // Преобразуем в decimal
                    command.Parameters.AddWithValue("@price", decimal.Parse(price)); // Преобразуем в decimal

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Блюдо успешно добавлено!");
                    LoadData(); // Обновляем DataGridView
                    ClearTextBoxes();
                    SetNextDishID(); // Устанавливаем следующий ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении блюда: " + ex.Message);
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main M = new Main();
            M.Show();
        }













        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 F1 = new Form1();
            F1.Show();
        }
    }
    }
