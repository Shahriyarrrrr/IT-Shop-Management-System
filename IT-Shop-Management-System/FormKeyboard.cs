using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormKeyboard : Form
    {
        public FormKeyboard()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";
        private int userID;

        private void FormKeyboard_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM products WHERE Category = 'MECHANICAL KEYBOARD'";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvKeyboard.DataSource = dt;
                }

                dgvKeyboard.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvKeyboard.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Price FROM products WHERE Category = 'MECHANICAL KEYBOARD' AND ProductName LIKE @searchTerm";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvKeyboard.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        private void dgvKeyboard_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKeyboard.Rows[e.RowIndex];
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtProductPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvKeyboard.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product first.");
                return;
            }

            int productId = Convert.ToInt32(dgvKeyboard.SelectedRows[0].Cells["ProductID"].Value);
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

