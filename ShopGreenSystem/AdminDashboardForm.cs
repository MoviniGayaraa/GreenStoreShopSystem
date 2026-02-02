using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ShopGreenSystem
{
    public partial class AdminDashboardForm : Form
    {
        // Database connection string
        private string connectionString = "Server=localhost;Port=3306;Database=shopGreen;Uid=root;Pwd=1234;";
        private int adminId;
        private string adminName;

        public AdminDashboardForm(int userId, string userName)
        {
            InitializeComponent();
            this.adminId = userId;
            this.adminName = userName;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadDashboardData();
        }

        // Load Dashboard Statistics
        private void LoadDashboardData()
        {
            try
            {
                lblWelcome.Text = $"Welcome, {adminName}! 🌱";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Get Total Sales
                    string salesQuery = "SELECT COALESCE(SUM(total_amount), 0) FROM orders WHERE status = 'DELIVERED'";
                    using (MySqlCommand cmd = new MySqlCommand(salesQuery, conn))
                    {
                        decimal totalSales = Convert.ToDecimal(cmd.ExecuteScalar());
                        lblTotalSalesValue.Text = $"Rs. {totalSales:N2}";
                    }

                    // Get Products in Stock
                    string stockQuery = "SELECT COUNT(*) FROM products WHERE status = 'AVAILABLE'";
                    using (MySqlCommand cmd = new MySqlCommand(stockQuery, conn))
                    {
                        int productsInStock = Convert.ToInt32(cmd.ExecuteScalar());
                        lblProductsInStockValue.Text = productsInStock.ToString();
                    }

                    // Get Active Orders
                    string ordersQuery = "SELECT COUNT(*) FROM orders WHERE status IN ('PENDING', 'SHIPPED')";
                    using (MySqlCommand cmd = new MySqlCommand(ordersQuery, conn))
                    {
                        int activeOrders = Convert.ToInt32(cmd.ExecuteScalar());
                        lblActiveOrdersValue.Text = activeOrders.ToString();
                    }

                    // Get Total Customers
                    string customersQuery = "SELECT COUNT(*) FROM users WHERE role = 'CUSTOMER'";
                    using (MySqlCommand cmd = new MySqlCommand(customersQuery, conn))
                    {
                        int totalCustomers = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalCustomersValue.Text = totalCustomers.ToString();
                    }

                    // Get Low Stock Products
                    string lowStockQuery = "SELECT COUNT(*) FROM products WHERE stock < 10 AND status = 'AVAILABLE'";
                    using (MySqlCommand cmd = new MySqlCommand(lowStockQuery, conn))
                    {
                        int lowStockProducts = Convert.ToInt32(cmd.ExecuteScalar());
                        lblLowStockValue.Text = lowStockProducts.ToString();

                        if (lowStockProducts > 0)
                        {
                            lblLowStockValue.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation Menu Buttons
        private void btnHome_Click(object sender, EventArgs e)
        {
            // Refresh dashboard data
            LoadDashboardData();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            ManageUsersForm manageUsersForm = new ManageUsersForm(adminId, adminName);
            manageUsersForm.Show();
            this.Hide();
        }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            ProductManagementForm productForm = new ProductManagementForm(adminId, adminName);
            productForm.Show();
            this.Hide();
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            CustomerManagementForm customerForm = new CustomerManagementForm(adminId, adminName);
            customerForm.Show();
            this.Hide();
        }

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            OrderManagementForm orderForm = new OrderManagementForm(adminId, adminName);
            orderForm.Show();
            this.Hide();
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm(adminId, adminName);
            reportsForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
            }
        }

        // Hover Effects for Menu Buttons
        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(37, 99, 41);
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(46, 125, 50);
        }

        // Hover Effects for Cards
        private void Card_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void Card_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.White;
        }

        private void AdminDashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Refresh Button
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
            MessageBox.Show("Dashboard refreshed successfully! 🌱", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}