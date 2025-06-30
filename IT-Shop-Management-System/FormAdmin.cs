using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormAdmin : Form
    {
        // Ensure your connection string is correct for your SQL Server instance
        private string connectionString = @"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

        public FormAdmin()
        {
            InitializeComponent();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employee";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            // Ensure these textboxes exist on your form designer
            // (e.g., txtName, txtEmail, txtPhone, txtPosition, txtSalary)
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtPosition.Text = "";
            txtSalary.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || txtPosition.Text == "" || txtSalary.Text == "")
            {
                MessageBox.Show("⚠ Please fill all fields before adding an employee.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Employee (Name, Email, Phone, Position, Salary, UserID) 
                                 VALUES (@Name, @Email, @Phone, @Position, @Salary, @UserID)";
                SqlCommand cmdAdd = new SqlCommand(query, con);
                cmdAdd.Parameters.AddWithValue("@Name", txtName.Text);
                cmdAdd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmdAdd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmdAdd.Parameters.AddWithValue("@Position", txtPosition.Text);
  
                //cmdAdd.Parameters.AddWithValue("@Role", cmbRole.SelectedItem?.ToString()); // if using a dropdown


                // Add error handling for salary conversion
                decimal salary;
                if (!decimal.TryParse(txtSalary.Text, out salary))
                {
                    MessageBox.Show("⚠ Please enter a valid number for Salary.");
                    return;
                }
                cmdAdd.Parameters.AddWithValue("@Salary", salary);
                cmdAdd.Parameters.AddWithValue("@UserID", 2); // Dummy UserID, consider making this dynamic

                try
                {
                    con.Open();
                    cmdAdd.ExecuteNonQuery();
                    MessageBox.Show("✅ Employee added successfully.");
                    LoadEmployees();
                    ClearFields();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An SQL error occurred: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠ Please select an employee to update.");
                return;
            }

            int employeeID;
            if (!int.TryParse(dataGridView1.SelectedRows[0].Cells["EmployeeID"].Value.ToString(), out employeeID))
            {
                MessageBox.Show("Error: Could not retrieve EmployeeID from selected row.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Employee 
                         SET Name = @Name, Email = @Email, Phone = @Phone, 
                             Position = @Position, Salary = @Salary 
                         WHERE EmployeeID = @EmployeeID";

                SqlCommand cmdUpdate = new SqlCommand(query, con);
                cmdUpdate.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmdUpdate.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmdUpdate.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                cmdUpdate.Parameters.AddWithValue("@Position", txtPosition.Text.Trim());

                decimal salary;
                if (!decimal.TryParse(txtSalary.Text, out salary))
                {
                    MessageBox.Show("⚠ Please enter a valid number for Salary.");
                    return;
                }

                cmdUpdate.Parameters.AddWithValue("@Salary", salary);
                cmdUpdate.Parameters.AddWithValue("@EmployeeID", employeeID); // ✅ FIXED: Added this!

                try
                {
                    con.Open();
                    cmdUpdate.ExecuteNonQuery();
                    MessageBox.Show("✅ Employee updated successfully.");
                    LoadEmployees();
                    ClearFields();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An SQL error occurred: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message);
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠ Please select an employee to delete.");
                return;
            }

            // Ensure "EmployeeID" column exists and contains valid integer values
            int employeeID;
            if (!int.TryParse(dataGridView1.SelectedRows[0].Cells["EmployeeID"].Value.ToString(), out employeeID))
            {
                MessageBox.Show("Error: Could not retrieve EmployeeID from selected row.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this employee?",
                                                     "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                    SqlCommand cmdDelete = new SqlCommand(query, con);
                    cmdDelete.Parameters.AddWithValue("@EmployeeID", employeeID);

                    try
                    {
                        con.Open();
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("🗑️ Employee deleted.");
                        LoadEmployees();
                        ClearFields();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An SQL error occurred: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unexpected error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Employee 
                         WHERE Name LIKE @Name OR Email LIKE @Name 
                         OR Phone LIKE @Name OR Position LIKE @Name";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@Name", "%" + keyword + "%");

                DataTable dt = new DataTable();

                try
                {
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An SQL error occurred during search: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred during search: " + ex.Message);
                }
            }
        }


        // Renamed from private_dataGridView1_CellClick to dataGridView1_CellClick
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                // Ensure column names match your database schema and DataGridView column names
                // It's safer to check for null values from cells
                txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
                txtPosition.Text = row.Cells["Position"].Value?.ToString() ?? "";
                txtSalary.Text = row.Cells["Salary"].Value?.ToString() ?? "";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

    

        private void button2_Click(object sender, EventArgs e)
        {
            FormAdminC adminC = new FormAdminC();
            adminC.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 a = new Form1();
            a.Show();
            this.Hide();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}