using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IT_Shop_Management_System
{
    public partial class FormCardPayment : Form
    {
        public FormCardPayment()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cardNumber = textBox1.Text.Trim();
            string cvc = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvc))
            {
                MessageBox.Show("⚠ Please enter both your Card Number and CVC.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("✅ Congratulation Your Payment is Successful! Thank you.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();    
            }
        }
    }
}

