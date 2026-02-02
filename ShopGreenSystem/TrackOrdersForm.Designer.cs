namespace ShopGreenSystem
{
    partial class TrackOrdersForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Button btnStore;
        private System.Windows.Forms.Button btnBrowseProducts;
        private System.Windows.Forms.Button btnMyCart;
        private System.Windows.Forms.Button btnTrackOrders;
        private System.Windows.Forms.Button btnMyProfile;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Button btnAllOrders;
        private System.Windows.Forms.Button btnPendingOrders;
        private System.Windows.Forms.Button btnShippedOrders;
        private System.Windows.Forms.Button btnDeliveredOrders;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Label lblOrderCount;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTrackOrdersTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMenu = new Panel();
            pictureBoxLogo = new PictureBox();
            btnLogout = new Button();
            btnMyProfile = new Button();
            btnTrackOrders = new Button();
            btnMyCart = new Button();
            btnBrowseProducts = new Button();
            btnStore = new Button();
            panelTop = new Panel();
            btnRefresh = new Button();
            lblWelcome = new Label();
            lblTitle = new Label();
            panelMain = new Panel();
            lblOrderCount = new Label();
            panelFilters = new Panel();
            btnDeliveredOrders = new Button();
            btnShippedOrders = new Button();
            btnPendingOrders = new Button();
            btnAllOrders = new Button();
            dgvOrders = new DataGridView();
            lblTrackOrdersTitle = new Label();
            panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            panelTop.SuspendLayout();
            panelMain.SuspendLayout();
            panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(46, 125, 50);
            panelMenu.Controls.Add(pictureBoxLogo);
            panelMenu.Controls.Add(btnLogout);
            panelMenu.Controls.Add(btnMyProfile);
            panelMenu.Controls.Add(btnTrackOrders);
            panelMenu.Controls.Add(btnMyCart);
            panelMenu.Controls.Add(btnBrowseProducts);
            panelMenu.Controls.Add(btnStore);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Margin = new Padding(3, 4, 3, 4);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(250, 875);
            panelMenu.TabIndex = 0;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = Color.Transparent;
            pictureBoxLogo.Location = new Point(50, 25);
            pictureBoxLogo.Margin = new Padding(3, 4, 3, 4);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(150, 188);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 6;
            pictureBoxLogo.TabStop = false;
            // 
            // btnLogout
            // 
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(0, 800);
            btnLogout.Margin = new Padding(3, 4, 3, 4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(250, 62);
            btnLogout.TabIndex = 5;
            btnLogout.Text = "🚪 Logout";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnMyProfile
            // 
            btnMyProfile.Cursor = Cursors.Hand;
            btnMyProfile.FlatAppearance.BorderSize = 0;
            btnMyProfile.FlatStyle = FlatStyle.Flat;
            btnMyProfile.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnMyProfile.ForeColor = Color.White;
            btnMyProfile.Location = new Point(0, 500);
            btnMyProfile.Margin = new Padding(3, 4, 3, 4);
            btnMyProfile.Name = "btnMyProfile";
            btnMyProfile.Size = new Size(250, 62);
            btnMyProfile.TabIndex = 4;
            btnMyProfile.Text = "👤 My Profile";
            btnMyProfile.TextAlign = ContentAlignment.MiddleLeft;
            btnMyProfile.UseVisualStyleBackColor = true;
            btnMyProfile.Click += btnMyProfile_Click;
            btnMyProfile.MouseEnter += MenuButton_MouseEnter;
            btnMyProfile.MouseLeave += MenuButton_MouseLeave;
            // 
            // btnTrackOrders
            // 
            btnTrackOrders.BackColor = Color.FromArgb(37, 99, 41);
            btnTrackOrders.Cursor = Cursors.Hand;
            btnTrackOrders.FlatAppearance.BorderSize = 0;
            btnTrackOrders.FlatStyle = FlatStyle.Flat;
            btnTrackOrders.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnTrackOrders.ForeColor = Color.White;
            btnTrackOrders.Location = new Point(0, 425);
            btnTrackOrders.Margin = new Padding(3, 4, 3, 4);
            btnTrackOrders.Name = "btnTrackOrders";
            btnTrackOrders.Size = new Size(250, 62);
            btnTrackOrders.TabIndex = 3;
            btnTrackOrders.Text = "📦 Track Orders";
            btnTrackOrders.TextAlign = ContentAlignment.MiddleLeft;
            btnTrackOrders.UseVisualStyleBackColor = false;
            btnTrackOrders.Click += btnTrackOrders_Click;
            // 
            // btnMyCart
            // 
            btnMyCart.Cursor = Cursors.Hand;
            btnMyCart.FlatAppearance.BorderSize = 0;
            btnMyCart.FlatStyle = FlatStyle.Flat;
            btnMyCart.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnMyCart.ForeColor = Color.White;
            btnMyCart.Location = new Point(0, 350);
            btnMyCart.Margin = new Padding(3, 4, 3, 4);
            btnMyCart.Name = "btnMyCart";
            btnMyCart.Size = new Size(250, 62);
            btnMyCart.TabIndex = 2;
            btnMyCart.Text = "\U0001f6d2 My Cart";
            btnMyCart.TextAlign = ContentAlignment.MiddleLeft;
            btnMyCart.UseVisualStyleBackColor = true;
            btnMyCart.Click += btnMyCart_Click;
            btnMyCart.MouseEnter += MenuButton_MouseEnter;
            btnMyCart.MouseLeave += MenuButton_MouseLeave;
            // 
            // btnBrowseProducts
            // 
            btnBrowseProducts.Cursor = Cursors.Hand;
            btnBrowseProducts.FlatAppearance.BorderSize = 0;
            btnBrowseProducts.FlatStyle = FlatStyle.Flat;
            btnBrowseProducts.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnBrowseProducts.ForeColor = Color.White;
            btnBrowseProducts.Location = new Point(0, 275);
            btnBrowseProducts.Margin = new Padding(3, 4, 3, 4);
            btnBrowseProducts.Name = "btnBrowseProducts";
            btnBrowseProducts.Size = new Size(250, 62);
            btnBrowseProducts.TabIndex = 1;
            btnBrowseProducts.Text = "🔍 Browse Products";
            btnBrowseProducts.TextAlign = ContentAlignment.MiddleLeft;
            btnBrowseProducts.UseVisualStyleBackColor = true;
            btnBrowseProducts.Click += btnBrowseProducts_Click;
            btnBrowseProducts.MouseEnter += MenuButton_MouseEnter;
            btnBrowseProducts.MouseLeave += MenuButton_MouseLeave;
            // 
            // btnStore
            // 
            btnStore.Cursor = Cursors.Hand;
            btnStore.FlatAppearance.BorderSize = 0;
            btnStore.FlatStyle = FlatStyle.Flat;
            btnStore.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnStore.ForeColor = Color.White;
            btnStore.Location = new Point(0, 200);
            btnStore.Margin = new Padding(3, 4, 3, 4);
            btnStore.Name = "btnStore";
            btnStore.Size = new Size(250, 62);
            btnStore.TabIndex = 0;
            btnStore.Text = "🏪 Store";
            btnStore.TextAlign = ContentAlignment.MiddleLeft;
            btnStore.UseVisualStyleBackColor = true;
            btnStore.Click += btnStore_Click;
            btnStore.MouseEnter += MenuButton_MouseEnter;
            btnStore.MouseLeave += MenuButton_MouseLeave;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(lblWelcome);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(250, 0);
            panelTop.Margin = new Padding(3, 4, 3, 4);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(950, 125);
            panelTop.TabIndex = 1;
            panelTop.Paint += panelTop_Paint;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(46, 125, 50);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(800, 38);
            btnRefresh.Margin = new Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 50);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 12F);
            lblWelcome.ForeColor = Color.Gray;
            lblWelcome.Location = new Point(30, 75);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(192, 28);
            lblWelcome.TabIndex = 1;
            lblWelcome.Text = "Welcome, Customer!";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(46, 125, 50);
            lblTitle.Location = new Point(25, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(223, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ShopGreen 🌱";
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(245, 245, 245);
            panelMain.Controls.Add(lblOrderCount);
            panelMain.Controls.Add(panelFilters);
            panelMain.Controls.Add(dgvOrders);
            panelMain.Controls.Add(lblTrackOrdersTitle);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(250, 125);
            panelMain.Margin = new Padding(3, 4, 3, 4);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(20, 25, 20, 25);
            panelMain.Size = new Size(950, 750);
            panelMain.TabIndex = 2;
            // 
            // lblOrderCount
            // 
            lblOrderCount.AutoSize = true;
            lblOrderCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblOrderCount.ForeColor = Color.Gray;
            lblOrderCount.Location = new Point(720, 73);
            lblOrderCount.Name = "lblOrderCount";
            lblOrderCount.Size = new Size(146, 23);
            lblOrderCount.TabIndex = 3;
            lblOrderCount.Text = "0 Order(s) Found";
            lblOrderCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelFilters
            // 
            panelFilters.Controls.Add(btnDeliveredOrders);
            panelFilters.Controls.Add(btnShippedOrders);
            panelFilters.Controls.Add(btnPendingOrders);
            panelFilters.Controls.Add(btnAllOrders);
            panelFilters.Location = new Point(20, 73);
            panelFilters.Margin = new Padding(3, 4, 3, 4);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(910, 44);
            panelFilters.TabIndex = 2;
            // 
            // btnDeliveredOrders
            // 
            btnDeliveredOrders.BackColor = Color.White;
            btnDeliveredOrders.Cursor = Cursors.Hand;
            btnDeliveredOrders.FlatStyle = FlatStyle.Flat;
            btnDeliveredOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeliveredOrders.ForeColor = Color.FromArgb(46, 125, 50);
            btnDeliveredOrders.Location = new Point(310, 0);
            btnDeliveredOrders.Margin = new Padding(3, 4, 3, 4);
            btnDeliveredOrders.Name = "btnDeliveredOrders";
            btnDeliveredOrders.Size = new Size(150, 44);
            btnDeliveredOrders.TabIndex = 3;
            btnDeliveredOrders.Text = "✅ Delivered";
            btnDeliveredOrders.UseVisualStyleBackColor = false;
            btnDeliveredOrders.Click += btnDeliveredOrders_Click;
            btnDeliveredOrders.MouseEnter += FilterButton_MouseEnter;
            btnDeliveredOrders.MouseLeave += FilterButton_MouseLeave;
            // 
            // btnShippedOrders
            // 
            btnShippedOrders.BackColor = Color.White;
            btnShippedOrders.Cursor = Cursors.Hand;
            btnShippedOrders.FlatStyle = FlatStyle.Flat;
            btnShippedOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnShippedOrders.ForeColor = Color.FromArgb(33, 150, 243);
            btnShippedOrders.Location = new Point(470, 0);
            btnShippedOrders.Margin = new Padding(3, 4, 3, 4);
            btnShippedOrders.Name = "btnShippedOrders";
            btnShippedOrders.Size = new Size(150, 44);
            btnShippedOrders.TabIndex = 2;
            btnShippedOrders.Text = "🚚 Shipped";
            btnShippedOrders.UseVisualStyleBackColor = false;
            btnShippedOrders.Click += btnShippedOrders_Click;
            btnShippedOrders.MouseEnter += FilterButton_MouseEnter;
            btnShippedOrders.MouseLeave += FilterButton_MouseLeave;
            // 
            // btnPendingOrders
            // 
            btnPendingOrders.BackColor = Color.White;
            btnPendingOrders.Cursor = Cursors.Hand;
            btnPendingOrders.FlatStyle = FlatStyle.Flat;
            btnPendingOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPendingOrders.ForeColor = Color.FromArgb(255, 152, 0);
            btnPendingOrders.Location = new Point(150, 0);
            btnPendingOrders.Margin = new Padding(3, 4, 3, 4);
            btnPendingOrders.Name = "btnPendingOrders";
            btnPendingOrders.Size = new Size(150, 44);
            btnPendingOrders.TabIndex = 1;
            btnPendingOrders.Text = "⏳ Pending";
            btnPendingOrders.UseVisualStyleBackColor = false;
            btnPendingOrders.Click += btnPendingOrders_Click;
            btnPendingOrders.MouseEnter += FilterButton_MouseEnter;
            btnPendingOrders.MouseLeave += FilterButton_MouseLeave;
            // 
            // btnAllOrders
            // 
            btnAllOrders.BackColor = Color.FromArgb(46, 125, 50);
            btnAllOrders.Cursor = Cursors.Hand;
            btnAllOrders.FlatStyle = FlatStyle.Flat;
            btnAllOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAllOrders.ForeColor = Color.White;
            btnAllOrders.Location = new Point(0, 0);
            btnAllOrders.Margin = new Padding(3, 4, 3, 4);
            btnAllOrders.Name = "btnAllOrders";
            btnAllOrders.Size = new Size(150, 44);
            btnAllOrders.TabIndex = 0;
            btnAllOrders.Text = "📋 All Orders";
            btnAllOrders.UseVisualStyleBackColor = false;
            btnAllOrders.Click += btnAllOrders_Click;
            // 
            // dgvOrders
            // 
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(20, 125);
            dgvOrders.Margin = new Padding(3, 4, 3, 4);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.RowHeadersWidth = 51;
            dgvOrders.RowTemplate.Height = 24;
            dgvOrders.Size = new Size(910, 601);
            dgvOrders.TabIndex = 1;
            dgvOrders.CellDoubleClick += dgvOrders_CellDoubleClick;
            // 
            // lblTrackOrdersTitle
            // 
            lblTrackOrdersTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTrackOrdersTitle.ForeColor = Color.FromArgb(46, 125, 50);
            lblTrackOrdersTitle.Location = new Point(20, 25);
            lblTrackOrdersTitle.Name = "lblTrackOrdersTitle";
            lblTrackOrdersTitle.Size = new Size(910, 44);
            lblTrackOrdersTitle.TabIndex = 0;
            lblTrackOrdersTitle.Text = "📦 Track Your Orders";
            // 
            // TrackOrdersForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 875);
            Controls.Add(panelMain);
            Controls.Add(panelTop);
            Controls.Add(panelMenu);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "TrackOrdersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Track Orders - ShopGreen";
            FormClosing += TrackOrdersForm_FormClosing;
            panelMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            panelFilters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ResumeLayout(false);
        }
    }
}