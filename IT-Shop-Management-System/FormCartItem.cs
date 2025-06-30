using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace IT_Shop_Management_System
{
    public partial class FormCartItem : Form
    {
        string connectionString = "Data Source=DESKTOP-BIIDSM8\\SQLEXPRESS;Initial Catalog=ITShopDB;Integrated Security=True";
        private int userID;

        public FormCartItem()
        {
            InitializeComponent();
            dgvCartItems.CellEndEdit += dgvCartItems_CellEndEdit;
        }

        private void LoadCartItems()
        {
            int cartId = LoggedInUser.CartID;

            if (cartId == 0)
            {
                dgvCartItems.DataSource = null;
                lblTotal.Text = "Total Price: $0.00";
                MessageBox.Show("🛒 You have no active cart. Please add items first.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT ci.CartItemID, p.ProductName, p.Price, ci.Quantity, (p.Price * ci.Quantity) AS Total
                                 FROM CartItem ci
                                 INNER JOIN Products p ON ci.ProductID = p.ProductID
                                 WHERE ci.CartID = @CartID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@CartID", cartId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    dgvCartItems.DataSource = null;
                    lblTotal.Text = "Total Price: $0.00";
                    MessageBox.Show("🛒 No items found in your cart.");
                    return;
                }

                dgvCartItems.DataSource = dt;

                // Auto column formatting
                if (dgvCartItems.Columns.Contains("Total"))
                {
                    dgvCartItems.Columns["Total"].DefaultCellStyle.Format = "C2";
                }

                // Total price calculation
                decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Total"));
                lblTotal.Text = "Total Price: $" + total.ToString("0.00");
            }
        }

        private void FormCartItem_Load(object sender, EventArgs e)
        {
            LoadCartItems();
        }

        private void dgvCartItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvCartItems.Columns[e.ColumnIndex].Name == "Quantity")
                {
                    DataGridViewRow row = dgvCartItems.Rows[e.RowIndex];

                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    decimal total = price * quantity;

                    row.Cells["Total"].Value = total;

                    // Recalculate grand total
                    decimal grandTotal = 0;
                    foreach (DataGridViewRow r in dgvCartItems.Rows)
                    {
                        if (r.Cells["Total"].Value != null && r.Cells["Total"].Value.ToString() != "")
                        {
                            grandTotal += Convert.ToDecimal(r.Cells["Total"].Value);
                        }
                    }

                    lblTotal.Text = "Total Price: $" + grandTotal.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            FormOrderItem F = new FormOrderItem(LoggedInUser.UserID);
            F.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CustomerDashboard f = new CustomerDashboard(userID);
            f.Show();
            this.Hide();
        }
    }
}
