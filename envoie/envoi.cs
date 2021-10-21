using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class envoi : Form
    {
        public envoi()
        {
            InitializeComponent();
        }
        MySqlConnection cnx;
        bool Connecté = false;
        Socket clientSocket;
       
        byte[] data = new byte[256];
        byte[] Sdepart = new byte[256];
        byte[] Sdestination = new byte[256];
        int valeurid;
        int i = 0;
        int dep = 0;
        int departverif = 0;
        int destinationverif = 0;
        public static Socket connex;
        private byte[] h = new byte[16];
        private int h1g;
        private int h2g;
        private int convh1g;
        private int m1g;
        private int m2g;
        private int convm1g;
        private int s1g;
        private int s2g;
        private int convs1g;
        private int hours;
        private int hours2;
        private int min;
        private int min2;
        private int sec;
        private int i4 = 0;
        private int i5 =0;
        private int totale;

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void envoie_Load(object sender, EventArgs e)
        {
            ACCUEIL.openenvoi = 1;
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
            catch(Exception)
            {
                MessageBox.Show("echec de la connexion");
            }
            cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");//connexion bd
            if (cnx.State == ConnectionState.Closed)//ouverture bd
            {
                cnx.Open();
            }
            comboBox1.Items.Clear();//effacer les Items de la comboBox
            MySqlCommand combocmd = new MySqlCommand("SELECT ID FROM trajets", cnx);//selection dans la table de la collone ID
            using (MySqlDataReader Liretab = combocmd.ExecuteReader())
            {
                while (Liretab.Read())
                {
                    string ID = Liretab["ID"].ToString();
                    comboBox1.Items.Add(ID);//ajout de des ID dans l'items de la combobox
                }
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Connecté)
            {      
                if (comboBox1.Text != "")
                {
                    string recup = comboBox1.Text;//recuperation de l'id selectionner
                    valeurid = Int32.Parse(recup);
                    MySqlCommand lirecmd = new MySqlCommand("SELECT * FROM trajets", cnx);//requete sql
                    using (MySqlDataReader Liretab = lirecmd.ExecuteReader())//lecture dans bd
                    {
                        while (Liretab.Read())//tant qu'on lit dans la bd
                        {
                            //transfere valeur de bd au programme
                            string ID = Liretab["ID"].ToString();
                            string heuredep = Liretab["heuredep"].ToString();
                            string départ = Liretab["depart"].ToString();
                            string destination = Liretab["destination"].ToString();
                            string charg = Liretab["charg"].ToString();
                            string dechar = Liretab["dechar"].ToString();
                            string vit = Liretab["vitesse"].ToString();
                            string dir = Liretab["direction"].ToString();
                            //
                            destinationverif = Int32.Parse(destination);
                            departverif = Int32.Parse(départ);
                            
                            h = Encoding.UTF8.GetBytes(charg);
                            h1g = h[0] - 48;
                            h2g = h[1] - 48;
                            if (h1g >= 1)
                            {
                                //tant que l'attribut convh1g n'est pas égale à h1g on rajoute 9 ce qui fait pour 22 2+(2+9*2) donc 22
                                while (convh1g != h1g)
                                {
                                    h2g += 9;
                                    convh1g++;
                                }
                            }
                            m1g = h[3] - 48;
                            m2g = h[4] - 48;
                            if (m1g >= 1)
                            {
                                while (convm1g != m1g)
                                {
                                    m2g += 9;
                                    convm1g++;
                                }
                            }
                            s1g = h[6] - 48;
                            s2g = h[7] - 48;
                            if (s1g >= 1)
                            {
                                while (convs1g != s1g)
                                {
                                    s2g += 9;
                                    convs1g++;
                                }
                            }
                            //
                            hours = h1g + h2g;// addition des deux nombres pour 15h (1 + 14)
                            hours2 = hours;
                            min = m1g + m2g;
                            min2 = min;
                            sec = s1g + s2g;
                            while (i4 != hours2)//passer les heures en secondes
                            {
                                hours += 3600;
                                i4++;
                            }
                            while (i5 != min2)//passer les minutes en secondes
                            {
                                min += 60;
                                i5++;
                            }
                            hours -= hours2;
                            min -= min2;
                            totale = hours + min + sec;
                            Thread.Sleep(totale*1000);
                            int ID2 = Int32.Parse(ID);//conversion ID en int
                            if (ID2 == valeurid)//la lecture est en boucle donc en faisant ca on attend que la boucle                                
                            {//arrive au bon ID dans la lecture
                            ;
                                if (i == 1 && dep == departverif)//on regarde si la destination et la meme que le prochain point de depart.
                                {
                                    dep = destinationverif;//dep = destination 
                                    int pointinterog = 1;
                                    string envo = "?a" + Convert.ToString(pointinterog);
                                    Sdepart = Encoding.UTF8.GetBytes(envo + "|" + ID + "|" + heuredep + "|" + départ + "|" + destination + "|" + charg + "|" + dechar + "|" + dir + vit + "|");
                                    clientSocket.Send(Sdepart, 0, Sdepart.Length, SocketFlags.None);//envoie du message                                                 
                                    MessageBox.Show("Envoyer.");
                                }
                                else
                                if (i == 0)
                                {
                                    dep = destinationverif;
                                    int pointinterog = 1;
                                    string envo = "?a" + Convert.ToString(pointinterog);
                                    Sdepart = Encoding.UTF8.GetBytes(envo + "|" + ID + "|" + heuredep + "|" + départ + "|" + destination + "|" + charg + "|" + dechar + "|" + dir + vit + "|");
                                    clientSocket.Send(Sdepart, 0, Sdepart.Length, SocketFlags.None);//envoie du message                                                 
                                    MessageBox.Show("Envoyer.");
                                    i++;
                                }
                                else
                                {
                                    MessageBox.Show("Le depart doit être le même que la destination precedente.");
                                }
                            }
  
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Selectionner un trajet");
                }

            }
            else 
            {
                MessageBox.Show("Vous n'etes pas connecté");
            }

        }
        private void label2_Click(object sender, EventArgs e)
        {



        }

        private void envoi_FormClosing(object sender, FormClosingEventArgs e)
        {
            ACCUEIL.openenvoi = 0;
            try
            {
                string pcoff = "?IHMOFF";
                data = Encoding.UTF8.GetBytes(pcoff);//conversion du string en Byte
                clientSocket.Send(data, 0, data.Length, SocketFlags.None);//envoie du message
                clientSocket.Close();//deconnexion socket
            }
            catch(Exception)
            {
                MessageBox.Show("echec de deconnexion");
            }
        }
    }
}
