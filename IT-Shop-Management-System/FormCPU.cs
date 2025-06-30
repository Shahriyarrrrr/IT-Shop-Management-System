using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormCPU : Form
    {
        public FormCPU()
        {
            InitializeComponent();
        }

        // ✅ Global Connection String
        string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";
        private int userID;

        // ✅ Load CPU products from the database
        private void LoadCPUProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM Products WHERE Category = 'CPU'";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCPU.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading CPU products: " + ex.Message);
            }
        }

        // ✅ Search CPU products
        private void SearchCPUProducts(string searchTerm)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM Products WHERE Category = 'CPU' AND ProductName LIKE @searchTerm";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCPU.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        // ✅ Add selected CPU to cart
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
           
        }

        // ✅ On form load
        private void FormCPU_Load(object sender, EventArgs e)
        {
            LoadCPUProducts();
            dgvCPU.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCPU.MultiSelect = false;
        }

        // ✅ When a row is clicked, fill in the textboxes
        private void dgvCPU_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCPU.Rows[e.RowIndex];

                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtProductPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }

        // ✅ Search button click
    

        // ✅ Optional text change events
        private void txtSearch_TextChanged(object sender, EventArgs e) { }
        private void txtProductPrice_TextChanged(object sender, EventArgs e) { }

        // ✅ View cart navigation
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Uncomment this if you have a CartForm ready
            // CartForm cartForm = new CartForm();
            // cartForm.Show();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            SearchCPUProducts(searchTerm);
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnAddToCart_Click_1(object sender, EventArgs e)
        {
            if (dgvCPU.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product first.");
                return;
            }

            int productId = Convert.ToInt32(dgvCPU.SelectedRows[0].Cells["ProductID"].Value);
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

        private void dgvCPU_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCPU.Rows[e.RowIndex];
                txtProductName.Text = row.Cells[1].Value.ToString(); // safer to use index
                txtProductPrice.Text = row.Cells[2].Value.ToString();
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
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
