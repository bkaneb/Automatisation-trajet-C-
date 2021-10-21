using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class Position : Form
    {
        public Position()
        {
            InitializeComponent();
        }
        int i = 0;
        int recuppos = 0;
        int x, y;
        Thread pnt;
        Image myImage;
        Graphics go;
        Brush br;
        Rectangle re;
        Socket clientSocket;
        byte[] data = new byte[256];
        byte[] posbyte = new byte[4];
        int pos = 0;
        int depart;
        private static ManualResetEvent pntMre = new ManualResetEvent(false);
        private void ThreadPaint()
        {
            while(true)
            {
                //mets l'image en fond
                myImage = panel1.BackgroundImage;
                pntMre.WaitOne();
                //reception des données
                try
                {

                    clientSocket.Receive(posbyte, 0, clientSocket.Available, SocketFlags.None);
                }
                catch(Exception)
                {
                    //receptionner sans bloquer le programme
                }
                recuppos = posbyte[0] - 48;        
                if (i == 0)
                {
                    if (depart == 1)
                    {
                        //création d'un graphics sur le panel
                        go = panel1.CreateGraphics();
                        //definition position station 1
                        re = new Rectangle(369, 169, 25, 25);
                        //definir la couleur
                        br = new SolidBrush(Color.Red);
                        //création du cercle
                        go.FillEllipse(br, re);
                        i++;
                    }
                    if (recuppos == 2)
                    {
                        go = panel1.CreateGraphics();
                        re = new Rectangle(518, 96, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        i++;
                        x = 518;
                    }
                    if (depart == 3)
                    {
                        go = panel1.CreateGraphics();
                        re = new Rectangle(636, 96, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        i++;
                    }
                    if (depart == 4)
                    {
                        go = panel1.CreateGraphics();
                        re = new Rectangle(550, 247, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        i++;
                    }
                    if (depart == 5)
                    {
                        go = panel1.CreateGraphics();
                        re = new Rectangle(142, 249, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        i++;
                    }
                    if (depart == 6)
                    {

                        go = panel1.CreateGraphics();
                        re = new Rectangle(183, 96, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        i++;
                    }
                    /*myImage = panel1.BackgroundImage;
                    go = panel1.CreateGraphics();
                    re = new Rectangle(x, y, 25, 25);
                    //go.Clear(panel1.BackgroundImage);
                    br = new SolidBrush(Color.Red);
                    go.FillEllipse(br, re);
                    i++;
                    i++;*/
                }
                if (i == 1)
                {
                    if (recuppos == 1 && pos != 1)
                    {
                        //effacement de tout le dessin
                        go.Clear(Color.White);
                        //redefinision de l'image en fond
                        go.DrawImage(myImage, new Point(0, 0));
                        //création d'un graphics sur le panel
                        go = panel1.CreateGraphics();
                        //definition position station 1
                        re = new Rectangle(369, 169, 25, 25);
                        //definir la couleur
                        br = new SolidBrush(Color.Red);
                        //création du cercle
                        go.FillEllipse(br, re);
                        pos = 1;
                    }
                    if (recuppos == 2 && pos != 2)
                    {
                        go.Clear(Color.White);
                        go.DrawImage(myImage, new Point(0, 0));
                        go = panel1.CreateGraphics();
                        re = new Rectangle(518, 96, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        pos = 2;
                        x = 518;                        
                    }
                    if (recuppos == 3 && pos != 3)
                    {
                        go.Clear(Color.White);
                        go.DrawImage(myImage, new Point(0, 0));
                        go = panel1.CreateGraphics();
                        re = new Rectangle(636, 96, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        pos = 3;
                    }
                    if (recuppos == 4 && pos != 4)
                    {
                        go.Clear(Color.White);
                        go.DrawImage(myImage, new Point(0, 0));
                        go = panel1.CreateGraphics();
                        re = new Rectangle(550, 247, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        pos = 4;
                    }
                    if (recuppos == 5 && pos != 5)
                    {
                        go.Clear(Color.White);
                        go.DrawImage(myImage, new Point(0, 0));
                        go = panel1.CreateGraphics();
                        re = new Rectangle(142, 249, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        pos = 5;

                    }
                    if (recuppos == 6 && pos != 6)
                    {
                        go.Clear(Color.White);
                        go.DrawImage(myImage, new Point(0, 0));
                        go = panel1.CreateGraphics();
                        re = new Rectangle(183, 96, 25, 25);
                        //go.Clear(panel1.BackgroundImage);
                        br = new SolidBrush(Color.Red);
                        go.FillEllipse(br, re);
                        pos = 6;
                    }
                    /*go.Clear(Color.White);
                    go.DrawImage(myImage, new Point(0, 0));
                    go = panel1.CreateGraphics();
                    re = new Rectangle(x, y, 25, 25);
                    //go.Clear(panel1.BackgroundImage);
                    br = new SolidBrush(Color.Red);
                    go.FillEllipse(br, re);
                    i++;*/
                }

            }
        }
        private void Position_Load(object sender, EventArgs e)
        {
            ACCUEIL.openposition = 1;//parametre pour ne pas avoir deux fenetres ouvertes
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//ajout du protocole TCP
                clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);//option du soclet, 
                //definition du temps pour émettre
                IPEndPoint serveurEP = new IPEndPoint(IPAddress.Parse("192.168.1.61"), 1500);//Mettre en parametre l'adresse IP 
                //et le port pour la connexion
                clientSocket.Connect(serveurEP);//connexion au socket
                string pc = "?IHM";
                data = Encoding.UTF8.GetBytes(pc);//conversion du string en Byte
                clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnt = new Thread(new ThreadStart(this.ThreadPaint));     
            pnt.Start();
            pntMre.Set();

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void fermeture_FormClosing(object sender, FormClosingEventArgs e)
        {//deconnexion
            ACCUEIL.openposition = 0;//dis que c'est deconnecté
            pntMre.Reset();
            string pcoff = "?IHMOFF";
            data = Encoding.UTF8.GetBytes(pcoff);//conversion du string en Byte
            clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
            clientSocket.Close();//deconnexion socket
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            y -= 10;
            i--;
        }



//
    }
}
