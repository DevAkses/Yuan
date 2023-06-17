using Npgsql;
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
    public partial class Tiket : Form
    {
        private Login.User user;
        public Tiket(Login.User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void ChecklistboxLoad()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=ternyatabeda;Database=token"))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT pesanan.id_kamar, pesanan.checkin, pesanan.checkout FROM pesanan LEFT JOIN riwayat_pemesanan ON pesanan.id_kamar = riwayat_pemesanan.id_kamar";

                NpgsqlDataReader reader = command.ExecuteReader();

                List<int> nomorKamarList = new List<int>();

                while (reader.Read())
                {

                    int nomorKamar = reader.GetInt32(reader.GetOrdinal("id_kamar"));
                    DateTime tanggalCheckin = DateTime.Parse(reader["checkin"].ToString());
                    DateTime tanggalCheckout = DateTime.Parse(reader["checkout"].ToString());

                    // Cek apakah tanggal saat ini berada di antara tanggal check-in dan check-out
                    if (DateTime.Now < tanggalCheckin || DateTime.Now > tanggalCheckout)
                    {
                        nomorKamarList.Add(nomorKamar);
                    }
                    MessageBox.Show("Nomor Kamar telah dipesan oleh user lain pada waktu yang sama");
                }

                foreach (int nomorKamar in nomorKamarList)
                {
                    checkedListBox1.Items.Add(nomorKamar);
                }

                reader.Close();
                connection.Close();
            }
        }

        private void Tiket_Load(object sender, EventArgs e)
        {
            try
            {
                ChecklistboxLoad();
                using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=ternyatabeda;Database=token"))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select id_kamar from kamar where status_kamar = true";
                    NpgsqlDataReader reader = command.ExecuteReader();
                    List<string> nomorKamarList = new List<string>();

                    while (reader.Read())
                    {
                        //bool statusKamar = reader.GetBoolean(reader.GetOrdinal("status_kamar"));
                        int nomorKamar = reader.GetInt32(reader.GetOrdinal("id_kamar"));

                        //if (statusKamar)
                        //{
                        nomorKamarList.Add(nomorKamar.ToString());
                        //}
                    }

                    foreach (string nomorKamar in nomorKamarList)
                    {
                        checkedListBox1.Items.Add(nomorKamar);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error : " + ex.Message);
            }

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
            labelMalam.Visible = true;
            labelTotal.Visible = true;
        }

        private int GetSelisihHari()
        {

            DateTime checkInDate = dateTimePicker1.Value;
            DateTime checkOutDate = dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1);
            TimeSpan selisih = checkOutDate - checkInDate;
            return selisih.Days;
        }

        public void UpdateResult()
        {
            int selisihHari = GetSelisihHari();
            int jumlahPilihan = checkedListBox1.CheckedItems.Count;
            int nilaiLabel = Convert.ToInt32(label3.Text);
            int hasilPerkalian = selisihHari * jumlahPilihan * nilaiLabel;

            labelTotal.Text = hasilPerkalian.ToString();
            labelMalam.Text = selisihHari.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ChecklistboxLoad();
                using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=ternyatabeda;Database=token"))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "insert into pesanan (id_kamar, checkin, checkout, harga, username) values (@id_kamar, @checkin, @checkout, @harga, @username)";
                    foreach (var selectedItem in checkedListBox1.CheckedItems)
                    {
                        int nomorKamar = int.Parse(selectedItem.ToString()); // Convert the selected item to int

                        command.Parameters.AddWithValue("id_kamar", nomorKamar);
                        command.Parameters.AddWithValue("checkin", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("checkout", dateTimePicker2.Value);
                        command.Parameters.AddWithValue("harga", Convert.ToInt32(labelTotal.Text));
                        command.Parameters.AddWithValue("username", user.username);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear(); // Clear parameters for the next iteration
                    }
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Pesanan Berhasil!!");
                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

