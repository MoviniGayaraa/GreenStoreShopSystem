using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ShopGreenSystem
{
    public partial class TrackOrdersForm : Form
    {
        // Database connection string
        private string connectionString = "Server=localhost;Port=3306;Database=shopGreen;Uid=root;Pwd=1234;";
        private int customerId;
        private string customerName;

        public TrackOrdersForm(int userId, string userName)
        {
            InitializeComponent();
            this.customerId = userId;
            this.customerName = userName;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadOrders();
        }

        // Load all orders for the customer
        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                    o.order_id AS 'Order ID',
                                    DATE_FORMAT(o.order_date, '%Y-%m-%d %H:%i') AS 'Order Date',
                                    o.total_amount AS 'Total Amount (Rs.)',
                                    o.status AS 'Status',
                                    GROUP_CONCAT(CONCAT(p.product_name, ' (x', oi.quantity, ')') SEPARATOR ', ') AS 'Products'
                                   FROM orders o
                                   LEFT JOIN order_items oi ON o.order_id = oi.order_id
                                   LEFT JOIN products p ON oi.product_id = p.product_id
                                   WHERE o.user_id = @userId
                                   GROUP BY o.order_id, o.order_date, o.total_amount, o.status
                                   ORDER BY o.order_date DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@userId", customerId);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvOrders.DataSource = dt;

                        // Style DataGridView
                        StyleOrdersGridView();

                        // Show order count
                        lblOrderCount.Text = $"{dt.Rows.Count} Order(s) Found";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Style the orders DataGridView
        private void StyleOrdersGridView()
        {
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.MultiSelect = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.BackgroundColor = Color.White;
            dgvOrders.BorderStyle = BorderStyle.None;
            dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 125, 50);
            dgvOrders.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.RowTemplate.Height = 40;

            // Customize column widths
            if (dgvOrders.Columns["Order ID"] != null)
                dgvOrders.Columns["Order ID"].Width = 100;

            if (dgvOrders.Columns["Order Date"] != null)
                dgvOrders.Columns["Order Date"].Width = 150;

            if (dgvOrders.Columns["Total Amount (Rs.)"] != null)
                dgvOrders.Columns["Total Amount (Rs.)"].Width = 150;

            if (dgvOrders.Columns["Status"] != null)
                dgvOrders.Columns["Status"].Width = 120;

            if (dgvOrders.Columns["Products"] != null)
                dgvOrders.Columns["Products"].Width = 300;

            // Color code status
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    Color statusColor = GetStatusColor(status);
                    row.Cells["Status"].Style.ForeColor = statusColor;
                    row.Cells["Status"].Style.Font = new Font(dgvOrders.Font, FontStyle.Bold);
                }

                // Format currency
                if (row.Cells["Total Amount (Rs.)"].Value != null && row.Cells["Total Amount (Rs.)"].Value != DBNull.Value)
                {
                    decimal amount = Convert.ToDecimal(row.Cells["Total Amount (Rs.)"].Value);
                    row.Cells["Total Amount (Rs.)"].Value = $"Rs. {amount:N2}";
                    row.Cells["Total Amount (Rs.)"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        // Get color based on order status
        private Color GetStatusColor(string status)
        {
            switch (status.ToUpper())
            {
                case "PENDING":
                    return Color.FromArgb(255, 152, 0); // Orange
                case "SHIPPED":
                    return Color.FromArgb(33, 150, 243); // Blue
                case "DELIVERED":
                    return Color.FromArgb(46, 125, 50); // Green
                default:
                    return Color.Gray;
            }
        }

        // Filter orders by status
        private void FilterOrdersByStatus(string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                    o.order_id AS 'Order ID',
                                    DATE_FORMAT(o.order_date, '%Y-%m-%d %H:%i') AS 'Order Date',
                                    o.total_amount AS 'Total Amount (Rs.)',
                                    o.status AS 'Status',
                                    GROUP_CONCAT(CONCAT(p.product_name, ' (x', oi.quantity, ')') SEPARATOR ', ') AS 'Products'
                                   FROM orders o
                                   LEFT JOIN order_items oi ON o.order_id = oi.order_id
                                   LEFT JOIN products p ON oi.product_id = p.product_id
                                   WHERE o.user_id = @userId AND o.status = @status
                                   GROUP BY o.order_id, o.order_date, o.total_amount, o.status
                                   ORDER BY o.order_date DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@userId", customerId);
                        adapter.SelectCommand.Parameters.AddWithValue("@status", status);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvOrders.DataSource = dt;
                        StyleOrdersGridView();
                        lblOrderCount.Text = $"{dt.Rows.Count} {status} Order(s)";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // View order details
        private void ViewOrderDetails(int orderId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Get order header
                    string headerQuery = @"SELECT 
                                         order_id, 
                                         DATE_FORMAT(order_date, '%Y-%m-%d %H:%i:%s') as order_date,
                                         total_amount,
                                         status
                                         FROM orders 
                                         WHERE order_id = @orderId AND user_id = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(headerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderId", orderId);
                        cmd.Parameters.AddWithValue("@userId", customerId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string message = $"📦 **Order #{reader["order_id"]}**\n\n" +
                                               $"📅 Order Date: {reader["order_date"]}\n" +
                                               $"💰 Total Amount: Rs. {Convert.ToDecimal(reader["total_amount"]):N2}\n" +
                                               $"📊 Status: {reader["status"]}\n\n" +
                                               $"**Order Items:**\n";
                                reader.Close();

                                // Get order items
                                string itemsQuery = @"SELECT 
                                                    p.product_name,
                                                    oi.quantity,
                                                    oi.price,
                                                    (oi.quantity * oi.price) as item_total
                                                    FROM order_items oi
                                                    JOIN products p ON oi.product_id = p.product_id
                                                    WHERE oi.order_id = @orderId";

                                using (MySqlCommand itemsCmd = new MySqlCommand(itemsQuery, conn))
                                {
                                    itemsCmd.Parameters.AddWithValue("@orderId", orderId);
                                    using (MySqlDataReader itemsReader = itemsCmd.ExecuteReader())
                                    {
                                        while (itemsReader.Read())
                                        {
                                            message += $"• {itemsReader["product_name"]} " +
                                                      $"x{itemsReader["quantity"]} " +
                                                      $"@ Rs. {Convert.ToDecimal(itemsReader["price"]):N2} " +
                                                      $"= Rs. {Convert.ToDecimal(itemsReader["item_total"]):N2}\n";
                                        }
                                    }
                                }

                                MessageBox.Show(message, "Order Details",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Order not found!", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation Menu Buttons
        private void btnStore_Click(object sender, EventArgs e)
        {
            CustomerDashboardForm dashboardForm = new CustomerDashboardForm(customerId, customerName);
            dashboardForm.Show();
            this.Hide();
        }

        private void btnBrowseProducts_Click(object sender, EventArgs e)
        {
            BrowseProductsForm browseForm = new BrowseProductsForm(customerId, customerName);
            browseForm.Show();
            this.Hide();
        }

        private void btnMyCart_Click(object sender, EventArgs e)
        {
            MyCartForm cartForm = new MyCartForm(customerId, customerName);
            cartForm.Show();
            this.Hide();
        }

        private void btnTrackOrders_Click(object sender, EventArgs e)
        {
            // Already on this form
            LoadOrders();
        }

        private void btnMyProfile_Click(object sender, EventArgs e)
        {
            MyProfileForm profileForm = new MyProfileForm(customerId, customerName);
            profileForm.Show();
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

        // Filter buttons click events
        private void btnAllOrders_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void btnPendingOrders_Click(object sender, EventArgs e)
        {
            FilterOrdersByStatus("PENDING");
        }

        private void btnShippedOrders_Click(object sender, EventArgs e)
        {
            FilterOrdersByStatus("SHIPPED");
        }

        private void btnDeliveredOrders_Click(object sender, EventArgs e)
        {
            FilterOrdersByStatus("DELIVERED");
        }

        // View order details on double click
        private void dgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOrders.Rows[e.RowIndex].Cells["Order ID"].Value != null)
            {
                int orderId = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["Order ID"].Value);
                ViewOrderDetails(orderId);
            }
        }

        // Refresh orders
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
            MessageBox.Show("Orders refreshed! 📦", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Hover effects for menu buttons
        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(37, 99, 41);
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != btnTrackOrders) // Keep current page highlighted
            {
                btn.BackColor = Color.FromArgb(46, 125, 50);
            }
        }

        // Form closing
        private void TrackOrdersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Filter button hover effects
        private void FilterButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(230, 245, 233);
        }

        private void FilterButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.White;
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}