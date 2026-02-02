using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ShopGreenSystem
{
    public partial class OrderManagementForm : Form
    {
        // Database connection string
        private string connectionString = "Server=localhost;Port=3306;Database=shopGreen;Uid=root;Pwd=1234;";
        private int adminId;
        private string adminName;
        private int selectedOrderId = -1;

        public OrderManagementForm(int userId, string userName)
        {
            InitializeComponent();
            this.adminId = userId;
            this.adminName = userName;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadOrders();
        }

        // Load all orders into DataGridView
        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT o.order_id AS 'Order ID', 
                                   u.name AS 'Customer Name', 
                                   o.order_date AS 'Order Date', 
                                   o.total_amount AS 'Total Amount', 
                                   o.status AS 'Status'
                                   FROM orders o
                                   INNER JOIN users u ON o.user_id = u.user_id
                                   ORDER BY o.order_id DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvOrders.DataSource = dt;

                        // Style DataGridView
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

                        // Color code rows by status
                        foreach (DataGridViewRow row in dgvOrders.Rows)
                        {
                            string status = row.Cells["Status"].Value.ToString();
                            switch (status)
                            {
                                case "PENDING":
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224);
                                    row.DefaultCellStyle.ForeColor = Color.FromArgb(230, 81, 0);
                                    break;
                                case "SHIPPED":
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(227, 242, 253);
                                    row.DefaultCellStyle.ForeColor = Color.FromArgb(1, 87, 155);
                                    break;
                                case "DELIVERED":
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                                    row.DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32);
                                    break;
                            }
                        }

                        lblTotalOrders.Text = $"Total Orders: {dt.Rows.Count}";
                    }
                }

                // Load order statistics
                LoadOrderStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load order statistics
        private void LoadOrderStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Pending Orders
                    string pendingQuery = "SELECT COUNT(*) FROM orders WHERE status = 'PENDING'";
                    using (MySqlCommand cmd = new MySqlCommand(pendingQuery, conn))
                    {
                        int pending = Convert.ToInt32(cmd.ExecuteScalar());
                        lblPendingCount.Text = pending.ToString();
                    }

                    // Shipped Orders
                    string shippedQuery = "SELECT COUNT(*) FROM orders WHERE status = 'SHIPPED'";
                    using (MySqlCommand cmd = new MySqlCommand(shippedQuery, conn))
                    {
                        int shipped = Convert.ToInt32(cmd.ExecuteScalar());
                        lblShippedCount.Text = shipped.ToString();
                    }

                    // Delivered Orders
                    string deliveredQuery = "SELECT COUNT(*) FROM orders WHERE status = 'DELIVERED'";
                    using (MySqlCommand cmd = new MySqlCommand(deliveredQuery, conn))
                    {
                        int delivered = Convert.ToInt32(cmd.ExecuteScalar());
                        lblDeliveredCount.Text = delivered.ToString();
                    }

                    // Total Revenue
                    string revenueQuery = "SELECT COALESCE(SUM(total_amount), 0) FROM orders WHERE status = 'DELIVERED'";
                    using (MySqlCommand cmd = new MySqlCommand(revenueQuery, conn))
                    {
                        decimal revenue = Convert.ToDecimal(cmd.ExecuteScalar());
                        lblRevenueCount.Text = $"Rs. {revenue:N2}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update Status Button
        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedOrderId == -1)
                {
                    MessageBox.Show("Please select an order to update!", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbStatus.SelectedIndex == -1)
                {
                    errorProvider1.SetError(cmbStatus, "Please select a status");
                    return;
                }

                errorProvider1.Clear();

                // Update order status
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE orders SET status = @status WHERE order_id = @orderId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@orderId", selectedOrderId);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Order status updated successfully! 🌱", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadOrders();
                            ClearFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating status: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // View Order Details Button
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedOrderId == -1)
                {
                    MessageBox.Show("Please select an order to view details!", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load order items
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT oi.order_item_id AS 'Item ID', 
                                   p.product_name AS 'Product Name', 
                                   oi.quantity AS 'Quantity', 
                                   oi.price AS 'Price', 
                                   (oi.quantity * oi.price) AS 'Total'
                                   FROM order_items oi
                                   INNER JOIN products p ON oi.product_id = p.product_id
                                   WHERE oi.order_id = @orderId";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@orderId", selectedOrderId);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No items found for this order.", "No Items",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        dgvOrderDetails.DataSource = dt;

                        // Style DataGridView
                        dgvOrderDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvOrderDetails.ReadOnly = true;
                        dgvOrderDetails.AllowUserToAddRows = false;
                        dgvOrderDetails.RowHeadersVisible = false;
                        dgvOrderDetails.BackgroundColor = Color.White;
                        dgvOrderDetails.BorderStyle = BorderStyle.None;
                        dgvOrderDetails.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 125, 50);
                        dgvOrderDetails.DefaultCellStyle.SelectionForeColor = Color.White;
                        dgvOrderDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
                        dgvOrderDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        dgvOrderDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        dgvOrderDetails.EnableHeadersVisualStyles = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clear Button
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Clear all input fields
        private void ClearFields()
        {
            txtOrderId.Clear();
            txtCustomerName.Clear();
            txtOrderDate.Clear();
            txtTotalAmount.Clear();
            cmbStatus.SelectedIndex = -1;
            selectedOrderId = -1;
            dgvOrderDetails.DataSource = null;
            errorProvider1.Clear();
            lblSelectedOrder.Text = "No order selected";
        }

        // DataGridView cell click - populate fields
        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvOrders.Rows[e.RowIndex];

                    selectedOrderId = Convert.ToInt32(row.Cells["Order ID"].Value);
                    txtOrderId.Text = row.Cells["Order ID"].Value.ToString();
                    txtCustomerName.Text = row.Cells["Customer Name"].Value.ToString();
                    txtOrderDate.Text = Convert.ToDateTime(row.Cells["Order Date"].Value).ToString("yyyy-MM-dd HH:mm:ss");
                    txtTotalAmount.Text = $"Rs. {Convert.ToDecimal(row.Cells["Total Amount"].Value):N2}";
                    cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();

                    lblSelectedOrder.Text = $"Selected: Order #{selectedOrderId}";

                    // Auto-load order details when order is selected
                    btnViewDetails_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting order: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Search functionality
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadOrders();
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT o.order_id AS 'Order ID', 
                                   u.name AS 'Customer Name', 
                                   o.order_date AS 'Order Date', 
                                   o.total_amount AS 'Total Amount', 
                                   o.status AS 'Status'
                                   FROM orders o
                                   INNER JOIN users u ON o.user_id = u.user_id
                                   WHERE u.name LIKE @search OR o.order_id LIKE @search
                                   ORDER BY o.order_id DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvOrders.DataSource = dt;
                        lblTotalOrders.Text = $"Total Orders: {dt.Rows.Count}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Filter by status
        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFilterStatus.SelectedIndex == 0) // "All Orders"
                {
                    LoadOrders();
                    return;
                }

                string filterStatus = cmbFilterStatus.SelectedItem.ToString();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT o.order_id AS 'Order ID', 
                                   u.name AS 'Customer Name', 
                                   o.order_date AS 'Order Date', 
                                   o.total_amount AS 'Total Amount', 
                                   o.status AS 'Status'
                                   FROM orders o
                                   INNER JOIN users u ON o.user_id = u.user_id
                                   WHERE o.status = @status
                                   ORDER BY o.order_id DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@status", filterStatus);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvOrders.DataSource = dt;
                        lblTotalOrders.Text = $"Total Orders: {dt.Rows.Count}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Navigation Menu Buttons
        private void btnHome_Click(object sender, EventArgs e)
        {
            AdminDashboardForm adminDashboard = new AdminDashboardForm(adminId, adminName);
            adminDashboard.Show();
            this.Hide();
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
            // Already on this form
            LoadOrders();
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

        // Hover effects
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

        private void OrderManagementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Refresh Button
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
            ClearFields();
            cmbFilterStatus.SelectedIndex = 0;
            MessageBox.Show("Order data refreshed! 🌱", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}