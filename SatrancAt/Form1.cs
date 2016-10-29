using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SatrancAt
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Skorumuz
        /// </summary>
        private int skor = 0;

        /// <summary>
        /// Bir önceki seçim.
        /// </summary>
        private Label oncekiSecim = null;
        /// <summary>
        /// Bir önceki seçimin arkaplan rengi.
        /// </summary>
        private Color oncekiSecimArkaplanRengi;
        /// <summary>
        /// Bir önceki seçimin metin rengi.
        /// </summary>
        private Color oncekiSecimMetinRengi;

        /// <summary>
        /// Daha önce ziyaret edilmiş yerler.
        /// </summary>
        private List<Label> secilmisYerler;

        /// <summary>
        /// Anlık seçilebilecek yerler.
        /// </summary>
        private List<Label> secilebilirYerler;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Açılışta skoru yazdırıyoruz ve listelerizi oluşturuyoruz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            lblSkor.Text = "Skor: " + skor.ToString();
            secilmisYerler = new List<Label>();
            secilebilirYerler = new List<Label>();
        }

        /// <summary>
        /// Oluşturma tuşuna tıklandığında grid boyutuna karar verip ilgili metodu çağırıyoruz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int gridKenar = -1;

            #region Grid Boyut Seçimi
            if (radioButton1.Checked)
            {
                gridKenar = 5;
            }
            else if (radioButton2.Checked)
            {
                gridKenar = 6;
            }
            else if (radioButton3.Checked)
            {
                gridKenar = 7;
            }
            else if (radioButton4.Checked)
            {
                gridKenar = 8;
            }
            else if (radioButton5.Checked)
            {
                gridKenar = 9;
            }
            else
            {
                MessageBox.Show("Grid boyutunu seçmediniz!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            if (gridKenar != -1)
            {
                //MessageBox.Show(gridKenar + " x " + gridKenar + " boyutunda grid oluşturuluyor.",
                //                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                button1.Enabled = false;
                GridOlustur(gridKenar);
            }
        }

        /// <summary>
        /// Hücre sayısına göre gridi oluşturuyoruz.
        /// </summary>
        /// <param name="hucre">Oluşturulacak gridin hücre sayısı.</param>
        private void GridOlustur(int hucre)
        {
            for (int i = 0; i < hucre; i++)
            {
                for (int j = 0; j < hucre; j++)
                {
                    Label l = new Label();

                    #region Pozisyon İşlemleri
                    l.Width = 50;
                    l.Height = 50;

                    l.Left = 30 + (i * 50);
                    l.Top = 75 + (j * 50);

                    l.Text = (j + 1) + (i + 1).ToString();
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    #endregion

                    #region Görsel İşlemler
                    l.BorderStyle = BorderStyle.FixedSingle;

                    if ((i + j) % 2 == 0)
                    {
                        l.BackColor = Color.Beige;
                    }
                    else
                    {
                        l.BackColor = Color.Wheat;
                    }
                    #endregion

                    l.Click += HamleleriGoster;

                    this.Controls.Add(l);

                    // İlk hamle için her yeri seçilebilir yapıyoruz.
                    secilebilirYerler.Add(l);
                }
            }
        }

        /// <summary>
        /// Bir pozisyon seçildiğinde, seçilebilir yerleri gösteriyoruz.
        /// </summary>
        /// <param name="sender">Seçilen pozisyon.</param>
        /// <param name="e">Olay argümanları.</param>
        private void HamleleriGoster(object sender, EventArgs e)
        {
            // Skoru arttırıyoruz.
            skor++;
            lblSkor.Text = "Skor: " + skor.ToString();

            // Seçilebilir yerleri sıfırlıyoruz.
            secilebilirYerler = new List<Label>();

            // TODO: Seçilebilir mi kontrol et. Skor 1 üzerindeyse. (En az 1 seçim yaptıysa).

            if (oncekiSecim == ((Label)sender))
            {
                MessageBox.Show("Aynı elemanı tekrar seçemezsiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                #region Renk İşlemleri
                if (oncekiSecim != null)
                {
                    oncekiSecim.BackColor = oncekiSecimArkaplanRengi;
                    oncekiSecim.ForeColor = oncekiSecimMetinRengi;
                }

                oncekiSecim = ((Label)sender);
                oncekiSecimArkaplanRengi = ((Label)sender).BackColor;
                oncekiSecimMetinRengi = ((Label)sender).ForeColor;

                ((Label)sender).BackColor = Color.DarkBlue;
                ((Label)sender).ForeColor = Color.White;
                #endregion

                lblSecim.Text = "Aktif Seçim: " + ((Label)sender).Text;

                int secilenKod = Convert.ToInt16(((Label)sender).Text);

                // Seçilebilir kodları seçilen koda göre belirle.
                int[] secilebilirKodlar = new int[8];
                secilebilirKodlar[0] = secilenKod - 21;
                secilebilirKodlar[1] = secilenKod - 19;
                secilebilirKodlar[2] = secilenKod + 19;
                secilebilirKodlar[3] = secilenKod + 21;
                secilebilirKodlar[4] = secilenKod - 12;
                secilebilirKodlar[5] = secilenKod - 8;
                secilebilirKodlar[6] = secilenKod + 8;
                secilebilirKodlar[7] = secilenKod + 12;

                secilmisYerler.Add((Label)sender);
                secilebilirYerler.Remove((Label)sender);
                
                lblSecilebilir.Text = "Seçilebilir Yerler: \n";

                foreach (var item in Controls)
                {
                    if (item is Label)
                    {
                        foreach (var kod in secilebilirKodlar)
                        {
                            if (((Label)item).Text == kod.ToString())
                            {
                                lblSecilebilir.Text += ((Label)item).Text + "\n";

                                secilebilirYerler.Add((Label)item);

                                // TODO: KÖŞELER SEÇİLDİĞİNDE HATALI BOYUYOR DÜZELT!
                                ((Label)item).BackColor = Color.Red;
                                ((Label)item).ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
        }

    }
}
