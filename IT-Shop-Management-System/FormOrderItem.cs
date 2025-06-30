using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormOrderItem : Form
    {
        private int customerId;

        public FormOrderItem()
        {
            InitializeComponent();

            cmbPaymentMethod.Items.Add("Cash On Delivery");
            cmbPaymentMethod.Items.Add("Card");
            cmbPaymentMethod.SelectedIndex = 0;
        }

      
        public FormOrderItem(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;

            cmbPaymentMethod.Items.Add("Cash On Delivery");
            cmbPaymentMethod.Items.Add("Card");
            cmbPaymentMethod.SelectedIndex = 0;

            loadProducts();
        }

        private void loadProducts()
        {
            string connectionString = @"Data Source=DESKTOP-BIIDSM8\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                   
                    SqlCommand cmd = new SqlCommand("SELECT Name, Phone, Address FROM Customer WHERE CustomerID = @CustomerID", con);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                       
                       
                    }
                    dr.Close();

                 
                    SqlCommand cartCmd = new SqlCommand("SELECT CartID FROM Cart WHERE CustomerID = @CustomerID", con);
                    cartCmd.Parameters.AddWithValue("@CustomerID", customerId);
                    object cartIdObj = cartCmd.ExecuteScalar();

                    if (cartIdObj != null)
                    {
                        int cartId = Convert.ToInt32(cartIdObj);

                    
                        SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT 
                        p.ProductName, 
                        ci.Quantity, 
                        p.Price AS UnitPrice,
                        (ci.Quantity * p.Price) AS TotalPrice
                    FROM CartItem ci
                    INNER JOIN Products p ON ci.ProductID = p.ProductID
                    WHERE ci.CartID = @CartID", con);

                        da.SelectCommand.Parameters.AddWithValue("@CartID", cartId);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvOrderItems.DataSource = dt;

                    
                        decimal total = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row["TotalPrice"]));
                        lblTotal.Text = "Total: $" + total.ToString("0.00");
                    }
                    else
                    {
                        dgvOrderItems.DataSource = null;
                        lblTotal.Text = "Total: $0.00";
                        MessageBox.Show("🛒 No cart found for this customer.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error loading cart data:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            string selectedMethod = cmbPaymentMethod.SelectedItem.ToString().Trim();

            if (selectedMethod == "Cash On Delivery")
            {
                MessageBox.Show("🎉 Congratulations! Your order is confirmed.", "Order Placed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (selectedMethod == "Card")
            {
                FormCardPayment cardPaymentForm = new FormCardPayment();
                cardPaymentForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("❌ Please select a valid payment method.");
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormOrderItem_Load(object sender, EventArgs e)
        {
            
        }

        private void dgvOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
