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
    public partial class AracDuzenleForm : Form
    {
        private string connectionString = DatabaseConfig.GetConnectionString();
        private int aracId;
        public AracDuzenleForm(int aracID)
        {
            InitializeComponent();
            this.aracId = aracID;
            LoadAracBilgileri();
        }

        


        private void LoadAracBilgileri()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM araclar WHERE arac_id = @AracId";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AracId", aracId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader["plaka"].ToString();
                                textBox2.Text = reader["model"].ToString();
                                textBox3.Text = reader["marka"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Araç Bilgileri yüklenirken bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE araclar SET plaka = @Plaka, model = @Model, marka = @Marka WHERE arac_id = @AracId";
                    using (var command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Plaka", textBox1.Text);
                        command.Parameters.AddWithValue("@Model", textBox2.Text);
                        command.Parameters.AddWithValue("@Marka", textBox3.Text);
                        command.Parameters.AddWithValue("@AracId", aracId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0)
                        {
                            MessageBox.Show("Araç bilgileri başarı ile güncellendi!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Araç bilgileri güncellenemedi!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
