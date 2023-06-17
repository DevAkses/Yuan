using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ulasan
{
    public partial class HomepageUser : Form
    {
        private Login.User user;

        public HomepageUser(Login.User user)
        {
            InitializeComponent();
            this.user = user;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Ulasan ulasan = new Ulasan(user);
            ulasan.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tiket tiket = new Tiket(user);
            tiket.Show();
            this.Hide();
        }
    }
}
