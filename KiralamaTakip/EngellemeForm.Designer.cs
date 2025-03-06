namespace KiralamaTakip
{
    partial class EngellemeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtKimlikNumarasi = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEngellemeNedeni = new System.Windows.Forms.TextBox();
            this.btnEngelle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(38, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(498, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Engellemek istediğiniz müşterinin kimlik numarasını girin:";
            // 
            // txtKimlikNumarasi
            // 
            this.txtKimlikNumarasi.Location = new System.Drawing.Point(579, 93);
            this.txtKimlikNumarasi.Name = "txtKimlikNumarasi";
            this.txtKimlikNumarasi.Size = new System.Drawing.Size(141, 22);
            this.txtKimlikNumarasi.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(43, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Engelleme Nedeni:";
            // 
            // txtEngellemeNedeni
            // 
            this.txtEngellemeNedeni.Location = new System.Drawing.Point(239, 152);
            this.txtEngellemeNedeni.Multiline = true;
            this.txtEngellemeNedeni.Name = "txtEngellemeNedeni";
            this.txtEngellemeNedeni.Size = new System.Drawing.Size(193, 105);
            this.txtEngellemeNedeni.TabIndex = 3;
            // 
            // btnEngelle
            // 
            this.btnEngelle.Location = new System.Drawing.Point(239, 293);
            this.btnEngelle.Name = "btnEngelle";
            this.btnEngelle.Size = new System.Drawing.Size(105, 41);
            this.btnEngelle.TabIndex = 4;
            this.btnEngelle.Text = "Engelle";
            this.btnEngelle.UseVisualStyleBackColor = true;
            this.btnEngelle.Click += new System.EventHandler(this.btnEngelle_Click);
            // 
            // EngellemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEngelle);
            this.Controls.Add(this.txtEngellemeNedeni);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKimlikNumarasi);
            this.Controls.Add(this.label1);
            this.Name = "EngellemeForm";
            this.Text = "EngellemeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKimlikNumarasi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEngellemeNedeni;
        private System.Windows.Forms.Button btnEngelle;
    }
}