using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace IT_Shop_Management_System
{
    public partial class FormGPU : Form
    {
        public FormGPU()
        {
            InitializeComponent();
        }

        // ✅ Global Connection String
        string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";
        private int userID;

        // ✅ Load GPU products from the database
        private void LoadGPUProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM products WHERE Category = 'GPU'";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvGPU.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading GPU products: " + ex.Message);
            }
        }

        // ✅ Search GPU products
        private void SearchGPUProducts(string searchTerm)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM products WHERE Category = 'GPU' AND ProductName LIKE @searchTerm";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvGPU.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        // ✅ Add selected GPU to cart
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvGPU.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product first.");
                return;
            }

            int productId = Convert.ToInt32(dgvGPU.SelectedRows[0].Cells["ProductID"].Value);
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


        // ✅ On form load
        private void FormGPU_Load(object sender, EventArgs e)
        {
            LoadGPUProducts();
            dgvGPU.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGPU.MultiSelect = false;
        }

        // ✅ On DataGrid row click
        private void dgvGPU_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvGPU.Rows[e.RowIndex];

                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtProductPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }

        // ✅ Search Button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            SearchGPUProducts(searchTerm);
        }

        // ✅ Optional TextChanged Events
        private void txtProductPrice_TextChanged(object sender, EventArgs e) { }

        private void txtSearch_TextChanged(object sender, EventArgs e) { }

        // ✅ Optional View Cart Navigation
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCartItem f = new FormCartItem();
            f.Show();
            this.Hide();
       
        }

        private void lblProductPrice_Click(object sender, EventArgs e)
        {

        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblProductName_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CustomerDashboard c = new CustomerDashboard(userID);
            c.Show();
            this.Hide();
        }
    }
}
