using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ShopGreenSystem
{
    public partial class ReportsForm : Form
    {
        // Database connection string
        private string connectionString = "Server=localhost;Port=3306;Database=shopGreen;Uid=root;Pwd=1234;";
        private int adminId;
        private string adminName;
        private DataTable currentReportData;

        public ReportsForm(int userId, string userName)
        {
            InitializeComponent();
            this.adminId = userId;
            this.adminName = userName;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Sales Report Button
        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                   o.order_id AS 'Order ID',
                                   u.name AS 'Customer Name',
                                   o.order_date AS 'Order Date',
                                   o.total_amount AS 'Amount',
                                   o.status AS 'Status'
                                   FROM orders o
                                   INNER JOIN users u ON o.user_id = u.user_id
                                   WHERE o.status = 'DELIVERED'
                                   ORDER BY o.order_date DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        currentReportData = dt;
                        dgvReport.DataSource = dt;

                        // Style DataGridView
                        StyleDataGridView();

                        // Calculate total sales
                        decimal totalSales = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            totalSales += Convert.ToDecimal(row["Amount"]);
                        }

                        lblReportTitle.Text = "📊 Sales Report (Delivered Orders)";
                        lblReportSummary.Text = $"Total Sales: Rs. {totalSales:N2} | Total Orders: {dt.Rows.Count}";

                        MessageBox.Show($"Sales Report Generated!\n\nTotal Sales: Rs. {totalSales:N2}\nTotal Orders: {dt.Rows.Count}",
                            "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating sales report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Stock Report Button
        private void btnStockReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                   product_id AS 'Product ID',
                                   product_name AS 'Product Name',
                                   category AS 'Category',
                                   price AS 'Price',
                                   stock AS 'Stock',
                                   status AS 'Status',
                                   supplier AS 'Supplier'
                                   FROM products
                                   ORDER BY stock ASC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        currentReportData = dt;
                        dgvReport.DataSource = dt;

                        // Style DataGridView
                        StyleDataGridView();

                        // Highlight low stock items
                        foreach (DataGridViewRow row in dgvReport.Rows)
                        {
                            if (row.Cells["Stock"].Value != null)
                            {
                                int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                                if (stock < 10)
                                {
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                                    row.DefaultCellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                                }
                            }
                        }

                        // Calculate statistics
                        int totalProducts = dt.Rows.Count;
                        int lowStockCount = 0;
                        int outOfStockCount = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            int stock = Convert.ToInt32(row["Stock"]);
                            if (stock == 0) outOfStockCount++;
                            else if (stock < 10) lowStockCount++;
                        }

                        lblReportTitle.Text = "📦 Stock Report";
                        lblReportSummary.Text = $"Total Products: {totalProducts} | Low Stock: {lowStockCount} | Out of Stock: {outOfStockCount}";

                        MessageBox.Show($"Stock Report Generated!\n\nTotal Products: {totalProducts}\nLow Stock (<10): {lowStockCount}\nOut of Stock: {outOfStockCount}",
                            "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating stock report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Customer Order History Button
        private void btnCustomerOrderHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                   u.user_id AS 'Customer ID',
                                   u.name AS 'Customer Name',
                                   u.email AS 'Email',
                                   COUNT(o.order_id) AS 'Total Orders',
                                   COALESCE(SUM(o.total_amount), 0) AS 'Total Spent',
                                   MAX(o.order_date) AS 'Last Order Date'
                                   FROM users u
                                   LEFT JOIN orders o ON u.user_id = o.user_id
                                   WHERE u.role = 'CUSTOMER'
                                   GROUP BY u.user_id, u.name, u.email
                                   ORDER BY COUNT(o.order_id) DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        currentReportData = dt;
                        dgvReport.DataSource = dt;

                        // Style DataGridView
                        StyleDataGridView();

                        // Calculate statistics
                        int totalCustomers = dt.Rows.Count;
                        int activeCustomers = 0;
                        decimal totalRevenue = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            int orderCount = Convert.ToInt32(row["Total Orders"]);
                            if (orderCount > 0) activeCustomers++;
                            totalRevenue += Convert.ToDecimal(row["Total Spent"]);
                        }

                        lblReportTitle.Text = "👥 Customer Order History";
                        lblReportSummary.Text = $"Total Customers: {totalCustomers} | Active: {activeCustomers} | Total Revenue: Rs. {totalRevenue:N2}";

                        MessageBox.Show($"Customer Order History Generated!\n\nTotal Customers: {totalCustomers}\nActive Customers: {activeCustomers}\nTotal Revenue: Rs. {totalRevenue:N2}",
                            "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating customer order history: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Low Stock Alert Report Button
        private void btnLowStockAlert_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                   product_id AS 'Product ID',
                                   product_name AS 'Product Name',
                                   category AS 'Category',
                                   stock AS 'Current Stock',
                                   supplier AS 'Supplier',
                                   status AS 'Status'
                                   FROM products
                                   WHERE stock < 10
                                   ORDER BY stock ASC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        currentReportData = dt;
                        dgvReport.DataSource = dt;

                        // Style DataGridView
                        StyleDataGridView();

                        // Highlight all rows in red
                        foreach (DataGridViewRow row in dgvReport.Rows)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                        }

                        lblReportTitle.Text = "⚠️ Low Stock Alert Report";
                        lblReportSummary.Text = $"Products with Low Stock: {dt.Rows.Count}";

                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show($"Low Stock Alert!\n\n{dt.Rows.Count} products need restocking.",
                                "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("All products have sufficient stock!", "No Alerts",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating low stock report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Monthly Sales Report Button
        private void btnMonthlySales_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                   DATE_FORMAT(order_date, '%Y-%m') AS 'Month',
                                   COUNT(order_id) AS 'Total Orders',
                                   SUM(total_amount) AS 'Total Sales',
                                   AVG(total_amount) AS 'Average Order Value'
                                   FROM orders
                                   WHERE status = 'DELIVERED'
                                   GROUP BY DATE_FORMAT(order_date, '%Y-%m')
                                   ORDER BY Month DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        currentReportData = dt;
                        dgvReport.DataSource = dt;

                        // Style DataGridView
                        StyleDataGridView();

                        lblReportTitle.Text = "📈 Monthly Sales Report";
                        lblReportSummary.Text = $"Showing sales data for {dt.Rows.Count} months";

                        MessageBox.Show($"Monthly Sales Report Generated!\n\nShowing data for {dt.Rows.Count} months",
                            "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating monthly sales report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Style DataGridView
        private void StyleDataGridView()
        {
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.MultiSelect = false;
            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.RowHeadersVisible = false;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 125, 50);
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvReport.EnableHeadersVisualStyles = false;
        }

        // Export to CSV Button
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentReportData == null || currentReportData.Rows.Count == 0)
                {
                    MessageBox.Show("Please generate a report first!", "No Data",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV Files|*.csv";
                    saveDialog.Title = "Export Report to CSV";
                    saveDialog.FileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder csv = new StringBuilder();

                        // Add column headers
                        for (int i = 0; i < currentReportData.Columns.Count; i++)
                        {
                            csv.Append(currentReportData.Columns[i].ColumnName);
                            if (i < currentReportData.Columns.Count - 1)
                                csv.Append(",");
                        }
                        csv.AppendLine();

                        // Add rows
                        foreach (DataRow row in currentReportData.Rows)
                        {
                            for (int i = 0; i < currentReportData.Columns.Count; i++)
                            {
                                csv.Append(row[i].ToString().Replace(",", ";"));
                                if (i < currentReportData.Columns.Count - 1)
                                    csv.Append(",");
                            }
                            csv.AppendLine();
                        }

                        File.WriteAllText(saveDialog.FileName, csv.ToString());

                        MessageBox.Show($"Report exported successfully!\n\nSaved to: {saveDialog.FileName}",
                            "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to CSV: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Print Report Button
        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentReportData == null || currentReportData.Rows.Count == 0)
                {
                    MessageBox.Show("Please generate a report first!", "No Data",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Print functionality would open print dialog.\n\n" +
                    "In a complete implementation, this would use PrintDocument class.",
                    "Print Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Print Error",
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
            OrderManagementForm orderForm = new OrderManagementForm(adminId, adminName);
            orderForm.Show();
            this.Hide();
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            // Already on this form
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

        private void ReportsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}