using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace ShopGreenSystem
{
    public partial class BrowseProductsForm : Form
    {
        // Database connection string
        private string connectionString = "Server=localhost;Port=3306;Database=shopGreen;Uid=root;Pwd=1234;";
        private int customerId;
        private string customerName;
        private DataTable allProducts;
        private string currentSort = "Product Name ASC";
        private Image defaultProductImage;

        public BrowseProductsForm(int userId, string userName)
        {
            InitializeComponent();
            this.customerId = userId;
            this.customerName = userName;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create default image
            CreateDefaultImage();

            LoadCategories();
            LoadAllProducts();
            UpdateCartCount();
            UpdateWelcomeMessage();
        }

        // Create a default product image
        private void CreateDefaultImage()
        {
            try
            {
                defaultProductImage = new Bitmap(80, 80);
                using (Graphics g = Graphics.FromImage(defaultProductImage))
                {
                    g.Clear(Color.WhiteSmoke);
                    using (Font font = new Font("Arial", 8))
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        g.DrawString("No\nImage", font, Brushes.Gray,
                            new RectangleF(0, 0, 80, 80), sf);

                        g.DrawRectangle(Pens.LightGray, 0, 0, 79, 79);
                    }
                }
            }
            catch
            {
                // If creating image fails, use a simple placeholder
                defaultProductImage = new Bitmap(80, 80);
            }
        }

        private void UpdateWelcomeMessage()
        {
            lblWelcome.Text = $"Welcome, {customerName}! 🛍️";
        }

        // Load all products
        private void LoadAllProducts()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                    product_id AS 'ID',
                                    product_name AS 'Product Name', 
                                    category AS 'Category', 
                                    price AS 'Price', 
                                    stock AS 'Stock',
                                    discount AS 'Discount',
                                    supplier AS 'Supplier',
                                    image_path AS 'ImagePath',
                                    CASE 
                                        WHEN discount = 1 THEN price * 0.9 
                                        ELSE price 
                                    END AS 'Discounted Price'
                                   FROM products 
                                   WHERE status = 'AVAILABLE'
                                   ORDER BY product_name";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        allProducts = new DataTable();
                        adapter.Fill(allProducts);

                        // Apply current sort
                        ApplySorting();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Apply sorting to DataGridView
        private void ApplySorting()
        {
            if (allProducts == null) return;

            DataView dv = new DataView(allProducts);

            switch (currentSort)
            {
                case "Product Name ASC":
                    dv.Sort = "[Product Name] ASC";
                    btnSort.Text = "Sort: Name (A-Z)";
                    break;
                case "Product Name DESC":
                    dv.Sort = "[Product Name] DESC";
                    btnSort.Text = "Sort: Name (Z-A)";
                    break;
                case "Price Low to High":
                    dv.Sort = "[Price] ASC";
                    btnSort.Text = "Sort: Price (Low-High)";
                    break;
                case "Price High to Low":
                    dv.Sort = "[Price] DESC";
                    btnSort.Text = "Sort: Price (High-Low)";
                    break;
                case "Discount ASC":
                    dv.Sort = "[Discounted Price] ASC";
                    btnSort.Text = "Sort: Discount (Low)";
                    break;
                case "Discount DESC":
                    dv.Sort = "[Discounted Price] DESC";
                    btnSort.Text = "Sort: Discount (High)";
                    break;
            }

            dgvProducts.DataSource = dv;
            StyleProductsGridView();
        }

        // Style the products DataGridView
        private void StyleProductsGridView()
        {
            if (dgvProducts.Rows.Count == 0) return;

            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 125, 50);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.RowTemplate.Height = 80; // Set row height
            dgvProducts.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvProducts.DefaultCellStyle.Padding = new Padding(5);
            dgvProducts.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

            // Configure columns
            ConfigureDataGridViewColumns();

            // Highlight rows
            HighlightRows();

            lblProductCount.Text = $"{dgvProducts.Rows.Count} Product(s) Found";
        }

        // Configure DataGridView columns
        private void ConfigureDataGridViewColumns()
        {
            // Hide ID and ImagePath columns
            if (dgvProducts.Columns["ID"] != null)
                dgvProducts.Columns["ID"].Visible = false;

            if (dgvProducts.Columns["ImagePath"] != null)
                dgvProducts.Columns["ImagePath"].Visible = false;

            // Add Image column if not exists
            if (!dgvProducts.Columns.Contains("Image"))
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                imageColumn.Name = "Image";
                imageColumn.HeaderText = "Image";
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvProducts.Columns.Insert(0, imageColumn);
            }

            // Set column order and widths
            if (dgvProducts.Columns["Image"] != null)
            {
                dgvProducts.Columns["Image"].Width = 80;
                dgvProducts.Columns["Image"].DisplayIndex = 0;
            }

            if (dgvProducts.Columns["Product Name"] != null)
            {
                dgvProducts.Columns["Product Name"].Width = 250;
                dgvProducts.Columns["Product Name"].DisplayIndex = 1;
            }

            if (dgvProducts.Columns["Category"] != null)
            {
                dgvProducts.Columns["Category"].Width = 120;
                dgvProducts.Columns["Category"].DisplayIndex = 2;
            }

            if (dgvProducts.Columns["Price"] != null)
            {
                dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
                dgvProducts.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvProducts.Columns["Price"].Width = 100;
                dgvProducts.Columns["Price"].DisplayIndex = 3;
            }

            if (dgvProducts.Columns["Discounted Price"] != null)
            {
                dgvProducts.Columns["Discounted Price"].DefaultCellStyle.Format = "C2";
                dgvProducts.Columns["Discounted Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvProducts.Columns["Discounted Price"].Visible = false;
            }

            if (dgvProducts.Columns["Stock"] != null)
            {
                dgvProducts.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvProducts.Columns["Stock"].Width = 80;
                dgvProducts.Columns["Stock"].DisplayIndex = 4;
            }

            if (dgvProducts.Columns["Discount"] != null)
            {
                dgvProducts.Columns["Discount"].Visible = false;
            }

            if (dgvProducts.Columns["Supplier"] != null)
            {
                dgvProducts.Columns["Supplier"].Width = 150;
                dgvProducts.Columns["Supplier"].DisplayIndex = 5;
            }

            // Load images into the Image column
            LoadImagesIntoGrid();
        }

        // Load images into the grid
        private void LoadImagesIntoGrid()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["ImagePath"].Value != null && !string.IsNullOrEmpty(row.Cells["ImagePath"].Value.ToString()))
                {
                    string imagePath = row.Cells["ImagePath"].Value.ToString();
                    row.Cells["Image"].Value = GetProductImage(imagePath);
                }
                else
                {
                    row.Cells["Image"].Value = defaultProductImage;
                }

                // Add discount icon to product name if discounted
                if (row.Cells["Discount"].Value != null && Convert.ToBoolean(row.Cells["Discount"].Value))
                {
                    string productName = row.Cells["Product Name"].Value?.ToString() ?? "";
                    row.Cells["Product Name"].Value = $"🔥 {productName}";
                    row.Cells["Product Name"].Style.ForeColor = Color.FromArgb(230, 81, 0);
                    row.Cells["Product Name"].Style.Font = new Font(dgvProducts.Font, FontStyle.Bold);
                }
            }
        }

        // Highlight rows based on conditions
        private void HighlightRows()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                // Highlight discounted products
                if (row.Cells["Discount"].Value != null && Convert.ToBoolean(row.Cells["Discount"].Value))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224);
                }

                // Highlight low stock
                if (row.Cells["Stock"].Value != null)
                {
                    int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                    if (stock < 10 && stock > 0)
                    {
                        row.Cells["Stock"].Style.ForeColor = Color.Red;
                        row.Cells["Stock"].Style.Font = new Font(dgvProducts.Font, FontStyle.Bold);
                    }
                    else if (stock == 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        row.Cells["Stock"].Value = "Out of Stock";
                    }
                }
            }
        }

        // Load categories into combo box
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All Categories");
            cmbCategory.Items.Add("ORGANIC_GROCERY");
            cmbCategory.Items.Add("ECO_PRODUCT");
            cmbCategory.SelectedIndex = 0;
        }

        // Filter products based on search criteria
        private void FilterProducts()
        {
            try
            {
                if (allProducts == null) return;

                string searchText = txtSearch.Text.Trim().ToLower();
                string selectedCategory = cmbCategory.SelectedItem.ToString();

                DataTable filteredProducts = allProducts.Clone();

                foreach (DataRow row in allProducts.Rows)
                {
                    bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                         row["Product Name"].ToString().ToLower().Contains(searchText) ||
                                         row["Category"].ToString().ToLower().Contains(searchText) ||
                                         row["Supplier"].ToString().ToLower().Contains(searchText);

                    bool matchesCategory = selectedCategory == "All Categories" ||
                                           row["Category"].ToString() == selectedCategory;

                    if (matchesSearch && matchesCategory)
                    {
                        filteredProducts.ImportRow(row);
                    }
                }

                dgvProducts.DataSource = filteredProducts;
                StyleProductsGridView();
                lblProductCount.Text = $"{filteredProducts.Rows.Count} Product(s) Found";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Get product image
        private Image GetProductImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return defaultProductImage;

            try
            {
                // Try to load from full path
                if (File.Exists(imagePath))
                    return Image.FromFile(imagePath);

                // Try to load from application directory
                string appDir = Application.StartupPath;
                string fileName = Path.GetFileName(imagePath);
                string localPath = Path.Combine(appDir, "Images", fileName);

                if (File.Exists(localPath))
                    return Image.FromFile(localPath);

                return defaultProductImage;
            }
            catch
            {
                return defaultProductImage;
            }
        }

        // Show product details
        private void ShowProductDetails(int productId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                    product_name,
                                    category,
                                    price,
                                    stock,
                                    discount,
                                    supplier,
                                    image_path,
                                    created_at
                                   FROM products 
                                   WHERE product_id = @productId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string productName = reader["product_name"].ToString();
                                string category = reader["category"].ToString();
                                decimal price = Convert.ToDecimal(reader["price"]);
                                int stock = Convert.ToInt32(reader["stock"]);
                                bool hasDiscount = Convert.ToBoolean(reader["discount"]);
                                string supplier = reader["supplier"].ToString();
                                string imagePath = reader["image_path"] != DBNull.Value ?
                                                   reader["image_path"].ToString() : "";
                                DateTime createdDate = Convert.ToDateTime(reader["created_at"]);

                                Image productImage = GetProductImage(imagePath);

                                // Create custom dialog for product details
                                using (var detailsForm = new ProductDetailsDialog())
                                {
                                    detailsForm.ProductName = productName;
                                    detailsForm.Category = category;
                                    detailsForm.Price = price;
                                    detailsForm.Stock = stock;
                                    detailsForm.HasDiscount = hasDiscount;
                                    detailsForm.Supplier = supplier;
                                    detailsForm.ProductImage = productImage;
                                    detailsForm.CreatedDate = createdDate;
                                    detailsForm.ProductId = productId;

                                    if (detailsForm.ShowDialog() == DialogResult.Yes)
                                    {
                                        // Add to cart
                                        AddToCart(productId, productName);
                                    }
                                    else if (detailsForm.DialogResult == DialogResult.No)
                                    {
                                        // Buy now
                                        BuyNow(productId, productName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sort button click
        private void btnSort_Click(object sender, EventArgs e)
        {
            // Cycle through sort options
            switch (currentSort)
            {
                case "Product Name ASC":
                    currentSort = "Product Name DESC";
                    break;
                case "Product Name DESC":
                    currentSort = "Price Low to High";
                    break;
                case "Price Low to High":
                    currentSort = "Price High to Low";
                    break;
                case "Price High to Low":
                    currentSort = "Discount ASC";
                    break;
                case "Discount ASC":
                    currentSort = "Discount DESC";
                    break;
                case "Discount DESC":
                    currentSort = "Product Name ASC";
                    break;
            }

            ApplySorting();
        }

        // Product Details Dialog Class
        private class ProductDetailsDialog : Form
        {
            public string ProductName { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public bool HasDiscount { get; set; }
            public string Supplier { get; set; }
            public Image ProductImage { get; set; }
            public DateTime CreatedDate { get; set; }
            public int ProductId { get; set; }

            private PictureBox pictureBox;
            private Label lblName;
            private Label lblCategory;
            private Label lblPrice;
            private Label lblDiscountedPrice;
            private Label lblStock;
            private Label lblSupplier;
            private Label lblCreatedDate;
            private Button btnAddToCart;
            private Button btnBuyNow;
            private Button btnClose;

            public ProductDetailsDialog()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Product Details";
                this.Size = new Size(600, 500);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.BackColor = Color.White;

                // Picture Box
                pictureBox = new PictureBox
                {
                    Location = new Point(20, 20),
                    Size = new Size(200, 200),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Product Name
                lblName = new Label
                {
                    Location = new Point(240, 20),
                    Size = new Size(320, 40),
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.FromArgb(46, 125, 50)
                };

                // Category
                lblCategory = new Label
                {
                    Location = new Point(240, 70),
                    Size = new Size(320, 25),
                    Font = new Font("Segoe UI", 11)
                };

                // Price
                lblPrice = new Label
                {
                    Location = new Point(240, 105),
                    Size = new Size(320, 25),
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                };

                // Discounted Price
                lblDiscountedPrice = new Label
                {
                    Location = new Point(240, 140),
                    Size = new Size(320, 25),
                    Font = new Font("Segoe UI", 11),
                    ForeColor = Color.Red
                };

                // Stock
                lblStock = new Label
                {
                    Location = new Point(240, 175),
                    Size = new Size(320, 25),
                    Font = new Font("Segoe UI", 11)
                };

                // Supplier
                lblSupplier = new Label
                {
                    Location = new Point(240, 210),
                    Size = new Size(320, 25),
                    Font = new Font("Segoe UI", 11)
                };

                // Created Date
                lblCreatedDate = new Label
                {
                    Location = new Point(240, 245),
                    Size = new Size(320, 25),
                    Font = new Font("Segoe UI", 11)
                };

                // Buttons
                btnAddToCart = new Button
                {
                    Text = "🛒 Add to Cart",
                    Location = new Point(20, 400),
                    Size = new Size(170, 45),
                    BackColor = Color.FromArgb(255, 193, 7),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    DialogResult = DialogResult.Yes
                };

                btnBuyNow = new Button
                {
                    Text = "🚀 Buy Now",
                    Location = new Point(210, 400),
                    Size = new Size(170, 45),
                    BackColor = Color.FromArgb(46, 125, 50),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    DialogResult = DialogResult.No
                };

                btnClose = new Button
                {
                    Text = "Close",
                    Location = new Point(400, 400),
                    Size = new Size(170, 45),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    DialogResult = DialogResult.Cancel
                };

                // Add controls to form
                this.Controls.Add(pictureBox);
                this.Controls.Add(lblName);
                this.Controls.Add(lblCategory);
                this.Controls.Add(lblPrice);
                this.Controls.Add(lblDiscountedPrice);
                this.Controls.Add(lblStock);
                this.Controls.Add(lblSupplier);
                this.Controls.Add(lblCreatedDate);
                this.Controls.Add(btnAddToCart);
                this.Controls.Add(btnBuyNow);
                this.Controls.Add(btnClose);

                // Set data when form is shown
                this.Shown += (s, e) => LoadProductDetails();
            }

            private void LoadProductDetails()
            {
                pictureBox.Image = ProductImage ?? new Bitmap(200, 200);
                lblName.Text = ProductName;
                lblCategory.Text = $"Category: {Category}";
                lblPrice.Text = $"Price: Rs. {Price:N2}";

                if (HasDiscount)
                {
                    decimal discountedPrice = Price * 0.9m;
                    lblDiscountedPrice.Text = $"🔥 Discounted: Rs. {discountedPrice:N2} (Save Rs. {Price - discountedPrice:N2})";
                    lblDiscountedPrice.Visible = true;
                    lblPrice.Font = new Font(lblPrice.Font, FontStyle.Strikeout);
                    lblPrice.ForeColor = Color.Gray;
                }
                else
                {
                    lblDiscountedPrice.Visible = false;
                }

                lblStock.Text = $"Stock: {Stock} units";
                lblSupplier.Text = $"Supplier: {Supplier}";
                lblCreatedDate.Text = $"Added: {CreatedDate:yyyy-MM-dd}";

                // Disable buttons if out of stock
                if (Stock <= 0)
                {
                    btnAddToCart.Enabled = false;
                    btnBuyNow.Enabled = false;
                    btnAddToCart.Text = "Out of Stock";
                    btnBuyNow.Text = "Out of Stock";
                }
            }
        }

        // Add product to cart
        private void AddToCart(int productId, string productName)
        {
            try
            {
                // Check stock availability
                int availableStock = GetProductStock(productId);
                if (availableStock <= 0)
                {
                    MessageBox.Show($"Sorry, '{productName}' is out of stock!", "Out of Stock",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ask for quantity
                using (QuantityDialog quantityDialog = new QuantityDialog(availableStock, "Add to Cart"))
                {
                    if (quantityDialog.ShowDialog() == DialogResult.OK)
                    {
                        int quantity = quantityDialog.Quantity;

                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            // Check if product already exists in cart
                            string checkQuery = "SELECT quantity FROM cart WHERE user_id = @userId AND product_id = @productId";
                            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                            {
                                checkCmd.Parameters.AddWithValue("@userId", customerId);
                                checkCmd.Parameters.AddWithValue("@productId", productId);
                                object result = checkCmd.ExecuteScalar();

                                if (result != null)
                                {
                                    // Update existing cart item
                                    int existingQty = Convert.ToInt32(result);
                                    if (existingQty + quantity > availableStock)
                                    {
                                        MessageBox.Show($"Cannot add {quantity} items. You already have {existingQty} in cart, " +
                                            $"and only {availableStock} are available in stock!", "Stock Limit",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    string updateQuery = "UPDATE cart SET quantity = quantity + @quantity " +
                                                        "WHERE user_id = @userId AND product_id = @productId";
                                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@quantity", quantity);
                                        updateCmd.Parameters.AddWithValue("@userId", customerId);
                                        updateCmd.Parameters.AddWithValue("@productId", productId);
                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // Add new item to cart
                                    string insertQuery = "INSERT INTO cart (user_id, product_id, quantity) " +
                                                        "VALUES (@userId, @productId, @quantity)";
                                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                                    {
                                        insertCmd.Parameters.AddWithValue("@userId", customerId);
                                        insertCmd.Parameters.AddWithValue("@productId", productId);
                                        insertCmd.Parameters.AddWithValue("@quantity", quantity);
                                        insertCmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            // Update product stock
                            string updateStockQuery = "UPDATE products SET stock = stock - @quantity " +
                                                     "WHERE product_id = @productId";
                            using (MySqlCommand stockCmd = new MySqlCommand(updateStockQuery, conn))
                            {
                                stockCmd.Parameters.AddWithValue("@quantity", quantity);
                                stockCmd.Parameters.AddWithValue("@productId", productId);
                                stockCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show($"Successfully added {quantity} x '{productName}' to cart! ✅", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh product list and cart count
                            LoadAllProducts();
                            UpdateCartCount();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to cart: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Buy Now - Direct Purchase
        private void BuyNow(int productId, string productName)
        {
            try
            {
                // Check stock availability
                int availableStock = GetProductStock(productId);
                if (availableStock <= 0)
                {
                    MessageBox.Show($"Sorry, '{productName}' is out of stock!", "Out of Stock",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ask for quantity
                using (QuantityDialog quantityDialog = new QuantityDialog(availableStock, "Buy Now"))
                {
                    if (quantityDialog.ShowDialog() == DialogResult.OK)
                    {
                        int quantity = quantityDialog.Quantity;
                        decimal price = GetProductPrice(productId);
                        bool hasDiscount = GetProductDiscount(productId);
                        decimal unitPrice = hasDiscount ? price * 0.9m : price;
                        decimal totalPrice = unitPrice * quantity;

                        // Ask for confirmation
                        DialogResult confirmResult = MessageBox.Show(
                            $"Order Summary:\n\n" +
                            $"Product: {productName}\n" +
                            $"Quantity: {quantity}\n" +
                            $"Unit Price: Rs. {unitPrice:N2}\n" +
                            $"Total Amount: Rs. {totalPrice:N2}\n\n" +
                            $"Do you want to proceed with the purchase?",
                            "Confirm Purchase",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                conn.Open();

                                // Start transaction
                                using (MySqlTransaction transaction = conn.BeginTransaction())
                                {
                                    try
                                    {
                                        // Create order
                                        string orderQuery = @"INSERT INTO orders (user_id, total_amount, status) 
                                                           VALUES (@userId, @totalAmount, 'PENDING');
                                                           SELECT LAST_INSERT_ID();";

                                        int orderId;
                                        using (MySqlCommand orderCmd = new MySqlCommand(orderQuery, conn, transaction))
                                        {
                                            orderCmd.Parameters.AddWithValue("@userId", customerId);
                                            orderCmd.Parameters.AddWithValue("@totalAmount", totalPrice);
                                            orderId = Convert.ToInt32(orderCmd.ExecuteScalar());
                                        }

                                        // Add order item
                                        string itemQuery = @"INSERT INTO order_items (order_id, product_id, quantity, price) 
                                                           VALUES (@orderId, @productId, @quantity, @price)";
                                        using (MySqlCommand itemCmd = new MySqlCommand(itemQuery, conn, transaction))
                                        {
                                            itemCmd.Parameters.AddWithValue("@orderId", orderId);
                                            itemCmd.Parameters.AddWithValue("@productId", productId);
                                            itemCmd.Parameters.AddWithValue("@quantity", quantity);
                                            itemCmd.Parameters.AddWithValue("@price", unitPrice);
                                            itemCmd.ExecuteNonQuery();
                                        }

                                        // Update product stock
                                        string stockQuery = "UPDATE products SET stock = stock - @quantity WHERE product_id = @productId";
                                        using (MySqlCommand stockCmd = new MySqlCommand(stockQuery, conn, transaction))
                                        {
                                            stockCmd.Parameters.AddWithValue("@quantity", quantity);
                                            stockCmd.Parameters.AddWithValue("@productId", productId);
                                            stockCmd.ExecuteNonQuery();
                                        }

                                        // Commit transaction
                                        transaction.Commit();

                                        MessageBox.Show(
                                            $"🎉 Purchase Successful!\n\n" +
                                            $"Order ID: #{orderId}\n" +
                                            $"Product: {productName}\n" +
                                            $"Quantity: {quantity}\n" +
                                            $"Total: Rs. {totalPrice:N2}\n\n" +
                                            $"Your order has been placed successfully!",
                                            "Success",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                                        // Refresh product list
                                        LoadAllProducts();
                                        UpdateCartCount();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        throw new Exception($"Transaction failed: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing purchase: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Get product price
        private decimal GetProductPrice(int productId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT price FROM products WHERE product_id = @productId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // Get product discount status
        private bool GetProductDiscount(int productId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT discount FROM products WHERE product_id = @productId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToBoolean(result) : false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get product stock
        private int GetProductStock(int productId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT stock FROM products WHERE product_id = @productId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // Update cart item count in label
        private void UpdateCartCount()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM cart WHERE user_id = @userId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", customerId);
                        int cartCount = Convert.ToInt32(cmd.ExecuteScalar());
                        lblCartCount.Text = $"🛒 Cart ({cartCount})";
                    }
                }
            }
            catch (Exception)
            {
                lblCartCount.Text = "🛒 Cart (0)";
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
            LoadAllProducts();
        }

        private void btnMyCart_Click(object sender, EventArgs e)
        {
            MyCartForm cartForm = new MyCartForm(customerId, customerName);
            cartForm.Show();
            this.Hide();
        }

        private void btnTrackOrders_Click(object sender, EventArgs e)
        {
            TrackOrdersForm trackForm = new TrackOrdersForm(customerId, customerName);
            trackForm.Show();
            this.Hide();
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

        // Filter and search events
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbCategory.SelectedIndex = 0;
            currentSort = "Product Name ASC";
            LoadAllProducts();
        }

        // Add to cart on double click
        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Rows[e.RowIndex].Cells["ID"].Value != null)
            {
                int productId = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["ID"].Value);
                string productName = dgvProducts.Rows[e.RowIndex].Cells["Product Name"].Value.ToString();
                ShowProductDetails(productId);
            }
        }

        // View details on right-click
        private void dgvProducts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                if (dgvProducts.Rows[e.RowIndex].Cells["ID"].Value != null)
                {
                    int productId = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["ID"].Value);
                    ShowProductDetails(productId);
                }
            }
        }

        // Refresh products
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllProducts();
            UpdateCartCount();
            MessageBox.Show("Products refreshed! 🛒", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Hover effects for menu buttons
        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                btn.BackColor = Color.FromArgb(37, 99, 41);
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn != btnBrowseProducts)
            {
                btn.BackColor = Color.FromArgb(46, 125, 50);
            }
        }

        // Form closing
        private void BrowseProductsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Quick filter buttons
        private void btnShowDiscounted_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dv = new DataView(allProducts);
                dv.RowFilter = "Discount = true";
                dgvProducts.DataSource = dv;
                StyleProductsGridView();
                lblProductCount.Text = $"{dv.Count} Discounted Product(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering discounted products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowInStock_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dv = new DataView(allProducts);
                dv.RowFilter = "Stock > 0";
                dgvProducts.DataSource = dv;
                StyleProductsGridView();
                lblProductCount.Text = $"{dv.Count} In-Stock Product(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering in-stock products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Quick action buttons for selected product
        private void btnQuickAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvProducts.SelectedRows[0];
                if (selectedRow.Cells["ID"].Value != null)
                {
                    int productId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    string productName = selectedRow.Cells["Product Name"].Value.ToString();
                    AddToCart(productId, productName);
                }
            }
            else
            {
                MessageBox.Show("Please select a product first!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQuickBuyNow_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvProducts.SelectedRows[0];
                if (selectedRow.Cells["ID"].Value != null)
                {
                    int productId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    string productName = selectedRow.Cells["Product Name"].Value.ToString();
                    BuyNow(productId, productName);
                }
            }
            else
            {
                MessageBox.Show("Please select a product first!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvProducts.SelectedRows[0];
                if (selectedRow.Cells["ID"].Value != null)
                {
                    int productId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    ShowProductDetails(productId);
                }
            }
            else
            {
                MessageBox.Show("Please select a product first!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Show discounted prices
        private void btnShowDiscountedPrices_Click(object sender, EventArgs e)
        {
            if (dgvProducts.Columns["Discounted Price"] != null)
            {
                bool isVisible = dgvProducts.Columns["Discounted Price"].Visible;
                dgvProducts.Columns["Discounted Price"].Visible = !isVisible;

                if (!isVisible)
                {
                    MessageBox.Show("Discounted prices are now visible! 🔥", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Quick checkout button
        private void btnQuickCheckout_Click(object sender, EventArgs e)
        {
            btnMyCart_Click(sender, e);
        }

        // Quantity Dialog Class
        private class QuantityDialog : Form
        {
            private NumericUpDown numQuantity;
            private Button btnOK;
            private Button btnCancel;
            private Label lblMessage;

            public int Quantity { get; private set; }
            private int maxStock;

            public QuantityDialog(int maxStock, string title = "Select Quantity")
            {
                this.maxStock = maxStock;
                InitializeDialog(title);
            }

            private void InitializeDialog(string title)
            {
                this.Text = title;
                this.Size = new Size(350, 200);
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                lblMessage = new Label
                {
                    Text = $"Enter quantity (Max: {maxStock}):",
                    Location = new Point(20, 20),
                    Size = new Size(300, 30),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                numQuantity = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = maxStock,
                    Value = 1,
                    Location = new Point(20, 60),
                    Size = new Size(300, 30),
                    Font = new Font("Segoe UI", 10F),
                    ThousandsSeparator = true
                };

                btnOK = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(20, 110),
                    Size = new Size(140, 40),
                    BackColor = Color.FromArgb(46, 125, 50),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(180, 110),
                    Size = new Size(140, 40),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                btnOK.Click += (s, e) => { Quantity = (int)numQuantity.Value; };
                btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; };

                this.Controls.Add(lblMessage);
                this.Controls.Add(numQuantity);
                this.Controls.Add(btnOK);
                this.Controls.Add(btnCancel);

                numQuantity.Select(0, numQuantity.Text.Length);
                numQuantity.Focus();
            }
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom painting if needed
        }
    }
}