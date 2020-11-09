using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaMemoria
{
    public partial class Form1 : Form
    {
        int movimentos, cliques, cartasEncontradas, tagIndex;

        Image[] img = new Image[6]; // vetor que vai guarda as imagens escolhidas em cada local

        List<string> lista = new List<string>();

        int[] tags = new int[2];

        public Form1()
        {
            InitializeComponent();
            inicio();
        }

        private void inicio()
        {
            foreach (PictureBox item in Controls.OfType<PictureBox>()) // cada item picturebox vai ser analisado por esse meio
            {
                tagIndex = int.Parse(String.Format("{0}", item.Tag)); // vai ler a imagem baseado no indice que ela se encontra
                img[tagIndex] = item.Image; // vai receber a imagem que estava antes
                item.Image = Properties.Resources._010_carnival_mask; // toda vez que iniciar o jogo as imagens ficaram padrao
                item.Enabled = true;

            }

            Posicoes();
        }

        private void Posicoes()
        {
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                Random rdn = new Random();
                int[] xP = { 92, 230, 368, 506, 644, 782 };
                int[] yP = { 214, 325 };

            Repete:
                var X = xP[rdn.Next(0, xP.Length)];
                var Y = yP[rdn.Next(0, yP.Length)];
                
                string verificacao = X.ToString() + Y.ToString();

                if (lista.Contains(verificacao))// caso tiver volta
                {

                    goto Repete;

                } else // caso nao tiver, adiciona

                {
                    item.Location = new Point(X, Y);
                    lista.Add(verificacao);
                }

            }
        }

        private void ImagensClick_Click(object sender, EventArgs e)
        {
            bool parEncontrado = false;

            PictureBox pic = (PictureBox)sender;
            cliques++;

            tagIndex = int.Parse(String.Format("{0}", pic.Tag));
            pic.Image = img[tagIndex];
            pic.Refresh();


            if (cliques == 1) {
                tags[0] = int.Parse(String.Format("{0}", pic.Tag));
            }
            else if (cliques == 2) {
                movimentos++;
                lblMovimentos.Text ="Movimentos: " + movimentos.ToString();
                tags[1] = int.Parse(String.Format("{0}", pic.Tag));
                parEncontrado = ChecagemPares();
                desvira(parEncontrado);
            }
        }


        private bool ChecagemPares()
        {
            cliques = 0;
            if (tags[0] == tags[1]){

                return true;


            } else
            {
                return false;

                
            }
            
        }

        private void desvira(bool check)
        {

            Thread.Sleep(500);
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                if(int.Parse(String.Format("{0}", item.Tag)) == tags[0] ||
                   int.Parse(String.Format("{0}", item.Tag)) == tags[1])
                {
                    if(check == true)
                    {
                        item.Enabled = false;
                        cartasEncontradas++;

                    }
                    else
                    {
                        item.Image = Properties.Resources._010_carnival_mask;
                        item.Refresh();
                    }
                }
            }

            finaljogo();
        }

        private void finaljogo()
        {
            if(cartasEncontradas == (img.Length * 2))
            {
                MessageBox.Show("Jogo terminado com " + movimentos.ToString() + " movimentos!", "Fim de Jogo");
                DialogResult msg = 
                MessageBox.Show("Deseja jogar novamente?", "Caixa de Pergunta", MessageBoxButtons.YesNo);

                if(msg == DialogResult.Yes)
                {

                    cliques = 0; movimentos = 0; cartasEncontradas = 0;
                    lista.Clear();
                    inicio();

                } else if (msg == DialogResult.No)

                {
                    MessageBox.Show("Obrigado por jogar!", "Agradecimento");
                    Application.Exit();

                }
            }
        }
    }
}
