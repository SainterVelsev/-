using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Вторая_попытка
{
    public partial class Order : Form
    {
        private SqlConnection connection;
        private DataTable ordersTable;
        private DataTable dishesTable;
        private DataTable waitersTable;

        public Order()
        {
            InitializeComponent();
            InitializeDatabase(); // Инициализация подключения к базе данных
            LoadData();
            SetNextOrderID();
            PopulateComboBoxes();
        }

        private void InitializeDatabase()
        {
            // Инициализация подключения к базе данных
            string connectionString = "Data Source=WIN-835DGRUVKSH;Initial Catalog=Restoran;Integrated Security=True";
            connection = new SqlConnection(connectionString);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Обновляем Label9 при изменении выбора
            UpdateLabel9();
        }

        private void UpdateLabel9()
        {
            // Получаем выбранный ID_сотрудника
            if (comboBox1.SelectedValue != null)
            {
                int selectedWaiterID = (int)comboBox1.SelectedValue;
                label9.Text = $"ID_сотрудника: {selectedWaiterID}";
            }
        }

        private void LoadData()
        {
            try
            {
                connection.Open();

                // Загрузка данных из таблицы Заказы
                string ordersQuery = "SELECT * FROM Заказ";
                SqlDataAdapter ordersAdapter = new SqlDataAdapter(ordersQuery, connection);
                ordersTable = new DataTable();
                ordersAdapter.Fill(ordersTable);
                заказDataGridView.DataSource = ordersTable;

                // Загрузка данных из таблицы Блюдо
                string dishesQuery = "SELECT * FROM Блюдо";
                SqlDataAdapter dishesAdapter = new SqlDataAdapter(dishesQuery, connection);
                dishesTable = new DataTable();
                dishesAdapter.Fill(dishesTable);

                // Загрузка данных из таблицы Официанты
                string waitersQuery = "SELECT * FROM Сотрудник";
                SqlDataAdapter waitersAdapter = new SqlDataAdapter(waitersQuery, connection);
                waitersTable = new DataTable();
                waitersAdapter.Fill(waitersTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void SetNextOrderID()
        {
            if (ordersTable != null && ordersTable.Rows.Count > 0)
            {
                // Получаем последнее значение ID_заказа из DataTable
                int lastID = ordersTable.AsEnumerable()
                    .Max(row => row.Field<int>("ID_заказа"));

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

        private void PopulateComboBoxes()
        {
            // Заполнение выпадающего меню для официантов
            comboBox1.DataSource = waitersTable;
            comboBox1.DisplayMember = "Имя"; // Отображаемое имя официанта
            comboBox1.ValueMember = "ID_сотрудника"; // Значение ID официанта

            // Обработка события выбора официанта
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            // Установка начального значения Label9
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; // Выбираем первый элемент по умолчанию
                UpdateLabel9();
            }

            // Заполнение выпадающего меню для блюд
            comboBox2.DataSource = dishesTable;
            comboBox2.DisplayMember = "Название"; // Замените на имя столбца с названиями блюд
            comboBox2.ValueMember = "ID_блюда"; // Замените на имя столбца с ID блюд

            // Заполнение выпадающего меню для статусов
            comboBox3.Items.AddRange(new string[] { "Оплачен", "Исполнен", "Готовится", "Принят" });
            comboBox3.SelectedIndex = 0; // Устанавливаем первый элемент по умолчанию

            // Заполнение выпадающего меню для номеров столиков
            comboBox4.Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            comboBox4.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        public void CalculateTotal()
        {
            decimal total = 0;

            // Суммируем цены выбранных блюд
            foreach (var item in listBox1.Items)
            {
                var dish = dishesTable.AsEnumerable()
                    .FirstOrDefault(row => row.Field<string>("Название") == item.ToString());

                if (dish != null)
                {
                    total += dish.Field<decimal>("Цена");
                }
            }

            // Отображаем общую сумму
            labelTotal.Text = $"Счёт: {total} руб.";
        }

        private void buttonAddDish_Click_1(object sender, EventArgs e)
        {
            // Добавляем выбранное блюдо в список
            if (comboBox2.SelectedItem != null)
            {
                listBox1.Items.Add(comboBox2.Text);
                CalculateTotal();
            }
        }

        private void buttonCreateOrder_Click_1(object sender, EventArgs e)
        {
            // Получаем данные из формы
            int orderID = int.Parse(textBox1.Text);
            int waiterID = (int)comboBox1.SelectedValue;
            int tableNumber = int.Parse(comboBox4.Text); // Предполагаем, что comboBox4 содержит номера столиков
            string status = comboBox3.Text;
            string orderTime = textBox2.Text;
            int totals = int.Parse(textBox3.Text);

            // Создаем новый заказ
            string query = "INSERT INTO Заказ (ID_заказа, ID_Официанта, Номер_столика, Дата_и_время, Счёт, Статус) VALUES (@id, @waiterID, @tableNumber, @orderTime, @total, @status)";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@id", orderID);
                    command.Parameters.AddWithValue("@waiterID", waiterID);
                    command.Parameters.AddWithValue("@tableNumber", tableNumber);
                    command.Parameters.AddWithValue("@orderTime", orderTime);
                    command.Parameters.AddWithValue("@total", totals);
                    command.Parameters.AddWithValue("@status", status);

                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Заказ успешно удален!");
                    LoadData(); // Обновляем DataGridView
                    SetNextOrderID(); // Устанавливаем следующий ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании заказа: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void Order_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "restoranDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.restoranDataSet.Сотрудник);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "restoranDataSet.Блюдо". При необходимости она может быть перемещена или удалена.
            this.блюдоTableAdapter.Fill(this.restoranDataSet.Блюдо);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "restoranDataSet.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.заказTableAdapter.Fill(this.restoranDataSet.Заказ);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 F1 = new Form1();
            F1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Main M = new Main();
            M.Show();
        }


    }
}