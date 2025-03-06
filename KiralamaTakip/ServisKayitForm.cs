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

namespace KiralamaTakip
{
    public partial class ServisKayitForm : Form
    {
        public ServisKayitForm(string secilenPlaka)
        {
            InitializeComponent();
            this.plaka = secilenPlaka;
            label2.Text = $"Plaka: {plaka}";
        }

        private void ServisKayitForm_Load(object sender, EventArgs e)
        {

        }

        private string connectionString = DatabaseConfig.GetConnectionString();
        private string plaka;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO servis (plaka, km, tarih, yapilan_islem) " +
                                         "VALUES (@Plaka, @KM, @Tarih, @Islem)";

                    using (var command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Plaka", plaka);
                        command.Parameters.AddWithValue("@KM", int.Parse(textBox1.Text));
                        command.Parameters.AddWithValue("@Tarih", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@Islem", textBox2.Text);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Servis kaydı başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // **Ana Formu (Form1) bul ve LoadServis() metodunu çağır**
                            Form1 anaForm = Application.OpenForms["Form1"] as Form1;
                            if (anaForm != null)
                            {
                                anaForm.LoadServis(); // Ana formda servis verilerini güncelle
                            }

                            this.Close(); // Formu kapat
                        }
                        else
                        {
                            MessageBox.Show("Servis kaydı eklenemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
