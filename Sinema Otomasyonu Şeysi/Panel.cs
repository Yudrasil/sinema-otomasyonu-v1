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
    public partial class Panel : Form
    {
        public Panel()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider = Microsoft.JET.OLEDB.4.0; Data Source=kullanicisifre.mdb");
        //OleDbCommand komut1 = new OleDbCommand();
        OleDbDataAdapter adaptor;
        DataTable dt = new DataTable();

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dia = new DialogResult();
            dia = MessageBox.Show("Çıkış Yapmak İstediğinize Emin Misiniz?", "Emin Misiniz?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dia == DialogResult.Yes)
            {
                baslangic frm1 = new baslangic();
                frm1.Show();
                this.Hide();
            }
        }
        public void baglan()
        {
            if (baglanti.State == ConnectionState.Open)
            {
                baglanti.Close();
            }
            baglanti.Open();
        }
        private void Panel_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut1 = new OleDbCommand("select  filmler.film_adi,filmler.film_turu,seans1, seans1_sa from filmler ,seanslar where seanslar.film_adi=filmler.film_adi", baglanti);
            adaptor = new OleDbDataAdapter(komut1);
            DataTable tablo = new DataTable();
            adaptor.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].HeaderText = "Film Adı";
            dataGridView1.Columns[1].HeaderText = "Film Türü";
            dataGridView1.Columns[2].HeaderText = "Seans";
            dataGridView1.Columns[3].HeaderText = "Salon No";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bilet bilt = new Bilet();
            bilt.Show();
            this.Hide();
        }

        private void Panel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Film fi = new Film();
            fi.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Seans sea = new Seans();
            sea.Show();
            this.Hide();
        }
    }
}
