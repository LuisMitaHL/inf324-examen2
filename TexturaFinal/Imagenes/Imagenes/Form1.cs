using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging; // importar esa libreria
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Imagenes
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        int x, y;
        int nrocambios = 99;
        int[] oR = new int[99];
        int[] oG = new int[99];
        int[] oB = new int[99];
        int[] dR = new int[99];
        int[] dG = new int[99];
        int[] dB = new int[99];
        string[] dD = new string[99];
        int[] conteo = new int[99];

        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            OdbcConnection con = new OdbcConnection();
            OdbcDataAdapter ada = new OdbcDataAdapter();
            con.ConnectionString = "dsn=mysqlcolores";
            ada.SelectCommand = new OdbcCommand();
            ada.SelectCommand.Connection = con;
            ada.SelectCommand.CommandText = "select * from texturas";
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            nrocambios = ds.Tables[0].Rows.Count;


            for (int i = 0; i < nrocambios; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                oR[i] = Convert.ToInt32(row["oR"]);
                oG[i] = Convert.ToInt32(row["oG"]);
                oB[i] = Convert.ToInt32(row["oB"]);
                dR[i] = Convert.ToInt32(row["dR"]);
                dG[i] = Convert.ToInt32(row["dG"]);
                dB[i] = Convert.ToInt32(row["dB"]);
                dD[i] = Convert.ToString(row["descripcion"]);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Subir Imagen
            openFileDialog1.Filter = "Todos|*.*|Archivos JPEG|*.jpg|Archivos GIF|*.";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            int colorescambiados = 0;
            String colorescambiadosdetalle = "";
            bool[] coloresmodificados = new bool[99];

            for (int i = 0; i < bmp.Width-10; i=i+10)
            {
                for (int j = 0; j < bmp.Height-10; j=j+10)
                {
                    int sR = 0; int sG = 0; int sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                    {
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            Color pixelColor = bmp.GetPixel(ip, jp);
                            sR += pixelColor.R;
                            sG += pixelColor.G;
                            sB += pixelColor.B;
                        }
                    }

                    sR /= 100; sG /= 100; sB /= 100;

                    for (int k = 0; k < nrocambios; k++)
                    {
                        if (((oR[k] - 20 <= sR) && (sR <= oR[k] + 20)) &&
                            ((oG[k] - 20 <= sG) && (sG <= oG[k] + 20)) &&
                            ((oB[k] - 20 <= sB) && (sB <= oB[k] + 20)))
                        {
                            for (int ip = i; ip < i + 10 && ip < bmp.Width; ip++)
                            {
                                for (int jp = j; jp < j + 10 && jp < bmp.Height; jp++)
                                {
                                    bmp.SetPixel(ip, jp, Color.FromArgb(dR[k], dG[k], dB[k]));
                                    conteo[k]++;
                                }
                            }
                            // anotamos en el array de booleanos que si usamos el color
                            coloresmodificados[k] = true;
                        }
                    }           
                }
            }
            // leemos el array de booleanos, los que son true son cambios que usamos
            for (int k = 0; k < nrocambios; k++)
            {
                if (coloresmodificados[k])
                {
                    colorescambiados++;
                    colorescambiadosdetalle = colorescambiadosdetalle + "- (" + conteo[k] + " px)" + dD[k] + "\r\n";
                }
            }
            pictureBox1.Image = bmp;
            if (colorescambiados > 0)
            {
                textBox6.Text = "Se cambiaron " + Convert.ToString(colorescambiados) + " correspondencias de colores:\r\n" + colorescambiadosdetalle;
            }
            else {
                textBox6.Text = "No se detectaron colores compatibles en la imagen";
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            textBox4.Text = x.ToString();
            textBox5.Text = y.ToString();
            Color c = new Color();
            c = bmp.GetPixel(x, y);
            textBox1.Text = c.R.ToString();
            textBox2.Text = c.G.ToString();
            textBox3.Text = c.B.ToString();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

     /*   private void button5_Click(object sender, EventArgs e)
        {
            int mRn = 0, mGn = 0, mBn = 0;
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            for (int i = 0; i < bmp.Width-10; i=i+10)
                for (int j = 0; j < bmp.Height-10; j=j+10)
                {
                    c = bmp.GetPixel(i, j);

                }
            pictureBox1.Image = bmp2;
        }
        */
    }
}
