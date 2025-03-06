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
    public partial class MusteriDuzenleForm : Form
    {

        private string connectionString = DatabaseConfig.GetConnectionString();
        private string musteriKimlikNo; // Güncellenecek müşterinin Kimlik No'sunu tutar
        public MusteriDuzenleForm(string kimlikNo, string ad, string soyad, string dogumYeri, DateTime dogumTarihi, string ehliyetNo, string telefonNo)
        {
            InitializeComponent();
            musteriKimlikNo = kimlikNo; // Müşterinin Kimlik No'sunu kaydet

            // TextBox ve diğer bileşenlere bilgileri ata
            textBox1.Text = kimlikNo;
            textBox2.Text = ad;
            textBox3.Text = soyad;
            textBox4.Text = dogumYeri;
            dateTimePicker1.Value = dogumTarihi;
            textBox5.Text = ehliyetNo;
            textBox6.Text = telefonNo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE musteriler SET ad=@Ad, soyad=@Soyad, dogum_yeri=@DogumYeri, dogum_tarihi=@DogumTarihi, ehliyet_numarasi=@EhliyetNo, telefon_numarasi=@TelefonNo WHERE kimlik_numarasi=@KimlikNo";

                    using (var command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@KimlikNo", musteriKimlikNo);
                        command.Parameters.AddWithValue("@Ad", textBox2.Text);
                        command.Parameters.AddWithValue("@Soyad", textBox3.Text);
                        command.Parameters.AddWithValue("@DogumYeri", textBox4.Text);
                        command.Parameters.AddWithValue("@DogumTarihi", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@EhliyetNo", textBox5.Text);
                        command.Parameters.AddWithValue("@TelefonNo", textBox6.Text);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Müşteri bilgileri başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Güncelleme başarılıysa formu kapat
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme başarısız oldu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Güncelleme sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}