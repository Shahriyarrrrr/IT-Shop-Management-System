using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {
            // Optional: Add any initialization logic here
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Optional: You can remove this if label2 doesn't need a click handler
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();

            if (username == "")
            {
                MessageBox.Show("Please enter your username.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

            string query = "SELECT Password FROM Users WHERE Username = @Username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    object result = cmd.ExecuteScalar();

                    if(result != null)
{
                        lblPassword.Text = "Your password is: " + result.ToString();
                        
                    }


                    else
                    {
                        lblPassword.Text = "";
                        MessageBox.Show("Username not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 F = new Form1();
            F.Show();
            this.Hide();
        }
    }
}

