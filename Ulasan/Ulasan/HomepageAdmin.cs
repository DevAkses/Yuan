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
    public partial class HomepageAdmin : Form
    {
        private Login.User user;
        public HomepageAdmin(Login.User user)
        {
            InitializeComponent();
            this.user = user;
        }
    }
}
