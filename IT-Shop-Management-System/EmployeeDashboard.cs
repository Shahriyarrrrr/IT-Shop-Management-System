using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class EmployeeDashboard : Form
    {
        private string connectionString = @"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

        public EmployeeDashboard()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns["ProductID"].DataPropertyName = "ProductID";
            dataGridView1.Columns["ProductName"].DataPropertyName = "ProductName";
            dataGridView1.Columns["Category"].DataPropertyName = "Category";
            dataGridView1.Columns["Description"].DataPropertyName = "Description";
            dataGridView1.Columns["Price"].DataPropertyName = "Price";
            dataGridView1.Columns["StockQuantity"].DataPropertyName = "StockQuantity";
            dataGridView1.Columns["EmployeeUsername"].DataPropertyName = "EmployeeUsername";
        }

        private void EmployeeDashboard_Load(object sender, EventArgs e)
        {
            LoadProducts(); // Refresh product list when form opens
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProductID, ProductName, Category, Description, Price, StockQuantity, EmployeeUsername FROM Products";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtProductID.Text = row.Cells["ProductID"].Value.ToString();
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtCategory.Text = row.Cells["Category"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                txtStockQuantity.Text = row.Cells["StockQuantity"].Value.ToString();
            }
          
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtStockQuantity.Text))
            {
                MessageBox.Show("Please fill in all fields before adding a product.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Products (ProductName, Category, Description, Price, StockQuantity, EmployeeUsername) 
                                 VALUES (@ProductName, @Category, @Description, @Price, @StockQuantity, @EmployeeUsername)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@Category", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@StockQuantity", Convert.ToInt32(txtStockQuantity.Text));
                    cmd.Parameters.AddWithValue("@EmployeeUsername", Environment.UserName);

                    try
                    {
                        con.Open();
                        int rowsInserted = cmd.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Add Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string productID = txtProductID.Text;

            if (string.IsNullOrWhiteSpace(productID))
            {
                MessageBox.Show("Please select a product to update.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Products 
                                 SET ProductName = @ProductName, 
                                     Category = @Category, 
                                     Description = @Description, 
                                     Price = @Price, 
                                     StockQuantity = @StockQuantity 
                                 WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@Category", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@StockQuantity", Convert.ToInt32(txtStockQuantity.Text));

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string productID = txtProductID.Text;

            if (string.IsNullOrWhiteSpace(productID))
            {
                MessageBox.Show("Please select a product to delete.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productID);

                        try
                        {
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Product deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadProducts();
                                ClearFields();
                            }
                            else
                            {
                                MessageBox.Show("Product not found or could not be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadProducts(); // Show all if no keyword
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ProductID, ProductName, Category, Description, Price, StockQuantity, EmployeeUsername 
                                     FROM Products 
                                     WHERE ProductName LIKE @keyword 
                                        OR Category LIKE @keyword 
                                        OR EmployeeUsername LIKE @keyword";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching products: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtCategory.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtStockQuantity.Text = "";
        }

        // Optional empty events
        private void button1_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 form = new Form1();   
            form.Show();
            this.Hide();

        }

        private void EmployeeDashboard_Load_1(object sender, EventArgs e)
        {

        }
    }
}
