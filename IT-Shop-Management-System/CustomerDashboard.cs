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
    public partial class CustomerDashboard : Form
    {
        private int id; 
        public CustomerDashboard(int userID)
        {
            InitializeComponent();
            loadProducts();


            id = userID;
            
        }

        private void loadProducts()
        {
            int userId = LoggedInUser.UserID;  // Get logged-in user ID

            string connectionString = @"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Username, Gender, Address, PhoneNumber, Email FROM Users WHERE UserID = @userID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userID", userId);

                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtUserID.Text = userId.ToString(); // You can also hide this field if needed
                        txtCustomerName.Text = dr["Username"].ToString();
                        txtGender.Text = dr["Gender"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        txtPhone.Text = dr["PhoneNumber"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("User not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error: " + ex.Message);
                }
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkGPU_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormGPU c = new FormGPU();
            c.Show();
            this.Hide();
        }

        private void linkCPU_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCPU c = new FormCPU();
            c.Show();
            this.Hide();
        }

        private void linkRAM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRAM c = new FormRAM();
            c.Show();
            this.Hide();
        }

        private void linkMonitor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormMonitor c = new FormMonitor();
            c.Show();
            this.Hide();
        }

        private void linkKeyboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormKeyboard c = new FormKeyboard();
            c.Show();
            this.Hide();
        }

        private void linkGoToCart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCartItem f = new FormCartItem();
            f.Show();
            this.Hide();    
        }

        private void linkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 F = new Form1();
            F.Show();
            this.Hide();
        }
    }
}
