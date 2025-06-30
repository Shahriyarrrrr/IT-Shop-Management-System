using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
           
        }

 

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 🔐 Admin check
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("✅ Admin login successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormAdmin adminForm = new FormAdmin();
                adminForm.Show();
                this.Hide();
            }

            else if (username == "employee" && password == "emp123")
            {
                MessageBox.Show("✅ Employee login successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EmployeeDashboard adminDashboard = new EmployeeDashboard();
                adminDashboard.Show();
                this.Hide();
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "SELECT UserID FROM Users WHERE Username = @username AND Password = @password";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["UserID"]);
                            LoggedInUser.UserID = userId;

                            MessageBox.Show("✅ Login successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CustomerDashboard dashboard = new CustomerDashboard(userId);
                            dashboard.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("❌ Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ ERROR: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            CreateAccount f = new CreateAccount();
            f.Show();
            this.Hide();
        }

        private void linkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            ForgotPassword forgotForm = new ForgotPassword();
            forgotForm.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void label1_Click_2(object sender, EventArgs e) { }
    }
}
