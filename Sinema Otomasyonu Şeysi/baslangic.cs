using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Sinema_Otomasyonu_Şeysi
{
    public partial class baslangic : Form
    {
        OleDbConnection baglanti = new OleDbConnection("Provider = Microsoft.JET.OLEDB.4.0; Data Source=kullanicisifre.mdb");
        //OleDbDataAdapter adaptor;
        OleDbCommand komut1;
        //DataSet ds;
        OleDbDataReader okuyucu;

        public baslangic()
        {
            InitializeComponent();
        }

        void baglan()
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            baglanti.Open();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            komut1 = new OleDbCommand();
            baglanti.Open();
            komut1.Connection = baglanti;
            komut1.CommandText = "select * from sifreler where k_adi = @ad and sifre=@sifre";
            komut1.Parameters.AddWithValue("@ad", textBox1.Text);
            komut1.Parameters.AddWithValue("@sifre", textBox2.Text);
            okuyucu = komut1.ExecuteReader();
            if (okuyucu.Read())
            {
                textBox1.Text = "";
                textBox2.Text = "";
                Panel pane = new Panel();
                pane.Show();
                this.Hide();
                baglanti.Close();
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show("Kullanıcı adınız veya şifreniz yanlıştır!", "Başarısız Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void baslangic_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
            textBox1.Text = "admin";
            textBox2.Text = "1234";
        }
        void dogru_k_ad()
        {
            baglan();
            OleDbCommand komut = new OleDbCommand("select k_adi from sifreler where k_adi='" + textBox1.Text + "' ", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();
            if (oku.Read())
            {
                textBox1.ForeColor = Color.Green;
            }
            else
            {
                textBox1.ForeColor = Color.Red;
            }
            baglanti.Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dogru_k_ad();
        }
        void dogru_sifre()
        {
            baglan();
            OleDbCommand komt = new OleDbCommand("select sifre from sifreler where sifre='" + textBox2.Text + "' ", baglanti);
            OleDbDataReader oku = komt.ExecuteReader();
            if (oku.Read())
            {
                textBox2.ForeColor = Color.Green;
            }
            else
            {
                textBox2.ForeColor = Color.Red;
            }
            baglanti.Close();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            dogru_sifre();
        }
    }
}
