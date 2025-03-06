using System;
using System.Windows.Forms;
using Npgsql;

namespace KiralamaTakip
{
    public partial class EngelKaldirForm : Form
    {
        private Label label1;
        private TextBox txtKimlikNumarasi;
        private Button button1;
        private string connectionString = DatabaseConfig.GetConnectionString();
        public event Action EngelKaldirildi; // Form1'e haber vermek için event

        public EngelKaldirForm()
        {
            InitializeComponent();
            button1.Click += btnEngelKaldir_Click;
        }

        private void btnEngelKaldir_Click(object sender, EventArgs e)
        {
            string kimlikNumarasi = txtKimlikNumarasi.Text.Trim();

            if (string.IsNullOrWhiteSpace(kimlikNumarasi))
            {
                MessageBox.Show("Lütfen bir kimlik numarası girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Önce müşteri gerçekten engellenmiş mi kontrol edelim
                    string checkQuery = "SELECT COUNT(*) FROM engellenen_musteriler WHERE kimlik_numarasi = @KimlikNumarasi";

                    using (var checkCmd = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@KimlikNumarasi", kimlikNumarasi);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("Bu kimlik numarasına sahip engellenmiş müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Müşteriyi engellenenler listesinden silelim
                    string deleteQuery = "DELETE FROM engellenen_musteriler WHERE kimlik_numarasi = @KimlikNumarasi";

                    using (var deleteCmd = new NpgsqlCommand(deleteQuery, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@KimlikNumarasi", kimlikNumarasi);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Müşteri engeli başarıyla kaldırıldı!", "Kiralama Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Form1'e haber veriyoruz
                            EngelKaldirildi?.Invoke();

                            this.Close(); // İşlem tamamlanınca formu kapat
                        }
                        else
                        {
                            MessageBox.Show("Engel kaldırma işlemi başarısız oldu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtKimlikNumarasi = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(584, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Engelini Kaldırmak İstediğiniz Müşterinin Kimlik Numarasını Giriniz!";
            // 
            // txtKimlikNumarasi
            // 
            this.txtKimlikNumarasi.Location = new System.Drawing.Point(153, 93);
            this.txtKimlikNumarasi.Name = "txtKimlikNumarasi";
            this.txtKimlikNumarasi.Size = new System.Drawing.Size(120, 22);
            this.txtKimlikNumarasi.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.Location = new System.Drawing.Point(157, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 53);
            this.button1.TabIndex = 2;
            this.button1.Text = "Engeli Kaldır";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // EngelKaldirForm
            // 
            this.ClientSize = new System.Drawing.Size(546, 337);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtKimlikNumarasi);
            this.Controls.Add(this.label1);
            this.Name = "EngelKaldirForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      
    }
}
