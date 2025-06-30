using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormMonitor : Form
    {
        public FormMonitor()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";
        private int userID;
       

        // ✅ Load Monitor Products
        private void FormMonitor_Load(object sender, EventArgs e)
        {
            LoadMonitorProducts();
        }

        // ✅ Search Monitor Products
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadMonitorProducts(searchTerm);
        }

        // ✅ Reusable Method to Load Data
        private void LoadMonitorProducts(string searchTerm = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM products WHERE Category = 'MONITOR'";
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " AND ProductName LIKE @searchTerm";
                    }

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    }

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMonitor.DataSource = dt;

                    // ✅ Ensure currency format is applied to Price column
                    if (dgvMonitor.Columns.Contains("Price"))
                    {
                        dgvMonitor.Columns["Price"].DefaultCellStyle.Format = "C2";
                    }
                }

                dgvMonitor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvMonitor.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Monitor products: " + ex.Message);
            }
        }

        // ✅ Populate textboxes on click
        private void dgvMonitor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMonitor.Rows[e.RowIndex];
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtProductPrice.Text = row.Cells["Price"].FormattedValue.ToString();  // Show with $ in textbox
            }
        }

        // ✅ Add to Cart
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvMonitor.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product first.");
                return;
            }

            int productId = Convert.ToInt32(dgvMonitor.SelectedRows[0].Cells["ProductID"].Value);
            int quantity = 1;
            int customerId = LoggedInUser.UserID;

            string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Create cart only once
                    if (LoggedInUser.CartID == 0)
                    {
                        SqlCommand cmdCart = new SqlCommand("INSERT INTO Cart (CustomerID) VALUES (@CustomerID); SELECT SCOPE_IDENTITY();", con, transaction);
                        cmdCart.Parameters.AddWithValue("@CustomerID", customerId);
                        int newCartId = Convert.ToInt32(cmdCart.ExecuteScalar());

                        LoggedInUser.CartID = newCartId; // ✅ Set the active CartID
                    }

                    // Add product to cart
                    SqlCommand cmdItem = new SqlCommand("INSERT INTO CartItem (CartID, ProductID, Quantity) VALUES (@CartID, @ProductID, @Quantity)", con, transaction);
                    cmdItem.Parameters.AddWithValue("@CartID", LoggedInUser.CartID);
                    cmdItem.Parameters.AddWithValue("@ProductID", productId);
                    cmdItem.Parameters.AddWithValue("@Quantity", quantity);
                    cmdItem.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("✅ Item added to cart!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("❌ Failed to add to cart: " + ex.Message);
                }
            }
        }


        // ✅ Other Event Handlers (Optional UI)
        private void txtProductName_TextChanged(object sender, EventArgs e) { }
        private void txtSearch_TextChanged(object sender, EventArgs e) { }
        private void dgvMonitor_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CustomerDashboard c = new CustomerDashboard(userID);
            c.Show();
            this.Hide();
        }

        private void linkGoToCart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCartItem f = new FormCartItem();
            f.Show();
            this.Hide();
        }
    }
}
