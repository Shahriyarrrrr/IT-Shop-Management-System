using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;


namespace IT_Shop_Management_System
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string conStr = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string gender = rdoMale.Checked ? "Male" : "Female";
                string role = "Customer"; 


                string query = "INSERT INTO Users (FullName, Username, Email, PhoneNumber, Gender, Address, Password, Role) " +
               "VALUES (@FullName, @Username, @Email, @PhoneNumber, @Gender, @Address, @Password, @Role)";





                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Role", role);



                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Account created successfully!");
                }
            }

        }

        private void txtName_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide(); // Hide the CreateAccount form
            Form1 loginForm = new Form1(); // 'login' should match the CLASS NAME of your login form
            loginForm.Show();

        }
    }
}
