using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormAdminC : Form
    {
        private int _adminID = 1; // ✔ Set to 1 since only one admin exists

        public FormAdminC()
        {
            InitializeComponent();
            loadAdmin();
            loadCustomerDetails();
        }

        private void loadAdmin()
        {
            string connectionString = @"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Name, Phone, Email FROM Admin WHERE AdminID = @adminID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@adminID", _adminID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["Name"].ToString();
                                txtPhone.Text = reader["Phone"].ToString();
                                txtEmail.Text = reader["Email"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("⚠️ Admin not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error loading admin data:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadCustomerDetails()
        {
            string connectionString = @"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT userName, Email, PhoneNumber, Address FROM Users WHERE Role  = 'Customer';";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load customer data:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FormAdmin admin = new FormAdmin();
            admin.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormAdmin admin = new FormAdmin();
            admin.Show();
            this.Hide();
        }
    }
}
