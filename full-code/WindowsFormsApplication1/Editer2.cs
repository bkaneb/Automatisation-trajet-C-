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

namespace WindowsFormsApplication1
{
    public partial class Editer2 : Form
    {
      

       
        public Editer2()
        {
            InitializeComponent();
        }
        MySqlConnection cnx;
        int valeurid;
        string heure = DateTime.Now.ToString("HH:mm:ss");
        //verif trajet deja prog
        bool verif = false;
        //pour boucle
        int i = 0;
        int i1 = 0;
        int i2 = 0;
        int i3 = 0;
        int i4 = 0;
        int i5 = 0;
        //
        //recup en sec de la vrai heure
        int h0b = 0;
        int h1b = 0;
        //
        //recup en sec heure de départ
        int h0c = 0;
        int h1c = 0;
        //
        string timeNow;//convert heure de départ pour envoie a bd
        byte[] h = new byte[254];//Recup des valeurs de l'heure dans la bd
        string hS, mS, sS;//recup comnbobox heure départ
        int Hs, Ms, Ss;//conversion int combobox heure départ
        //pour recup bd
        int hours, hours2, h1g, h2g, totale;//recup heure bd
        int recupVit, recupDir;

        private void Editer2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ACCUEIL.openediter2 = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        int min, min2, m1g, m2g;//recup min bd
        int sec, s1g, s2g;//recup sec bd
        int convm1g = 0, convh1g = 0, convs1g = 0;//coversion
        //
        int h0, h1, h2, h3, Th1, cb1, cb2;
        string recupcb1, recupcb2;
        private void Editer2_Load(object sender, EventArgs e)
        {
            ACCUEIL.openediter2 = 1;
            label8.Text = heure;
            cnx = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;DATABASE=projet;SslMode=none;UID=root;PWD=;");//connexion bd
            if (cnx.State == ConnectionState.Closed)//ouverture bd
            {
                cnx.Open();
            }
                valeurid = editer.Valid;//transfere de la valeur id de editer.cs
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
                    string vitesse = Liretab["vitesse"].ToString();
                    string direction = Liretab["direction"].ToString();
                    //
                    int ID2 = Int32.Parse(ID);//conversion ID en int
                    if (ID2 == valeurid)/*la lecture est en boucle donc en faisant ca on
                        attend que la boucle arrive au bon ID dans la lecture*/
                    {
                        //affiche les valeurs de la bd dans les champs
                        h = Encoding.UTF8.GetBytes(heuredep);
                        comboBox1.Text = départ;
                        comboBox2.Text = destination;
                        textBox2.Text = charg;
                        textBox1.Text = dechar;
                        if(vitesse == "1")
                        {
                            comboBox7.Text = "lente";
                        }
                        else
                        if (vitesse == "2")
                        {
                            comboBox7.Text = "moyenne";
                        }
                        else
                        if (vitesse == "3")
                        {
                            comboBox7.Text = "rapide";
                        }
                        if(direction == "1")
                        {
                            comboBox6.Text = "droite";
                        }
                        else
                        {
                            comboBox6.Text = "gauche";
                        }
                            //
                            convm1g = 0;
                        convh1g = 0;
                        convs1g = 0;
                        h1g = h[0] - 48;
                        h2g = h[1] - 48;
                        if (h1g >= 1)//si > à 1 alors pas besoin de 0 devant car >= 10
                        {
                            //cette condition est faite pour afficher corectement lors de l'ouverture
                            if (h2g != 0)
                            {
                                while (convh1g != h1g)//tant que l'attribut convh1g
                                {// n'est pas égale à h1g on rajoute 9 ce qui fait pour 15h 1+(5+9) donc 15
                                    h2g += 9;
                                    convh1g++;
                                }
                                hours = h1g + h2g;// additoin des deux nombres pour 15h (1 + 14)
                                comboBox3.Text = Convert.ToString(hours);
                            }
                            else//si h2g = 0 on l'ecrit en 0 et en string
                            {
                                hours = h1g;
                                comboBox3.Text = Convert.ToString(hours + "0");
                            }
                        }
                        else//rajout du 0 devant l'unité car < à 10
                        {
                            if (h2g != 0)
                            {
                                hours = h2g;
                                comboBox3.Text = Convert.ToString("0" + hours);
                            }
                            else//si h5g = 0 le résultat sera 00 donc on l'écrit en string
                            {
                                comboBox3.Text = Convert.ToString("00");
                            }
                        }
                        m1g = h[3] - 48;
                        m2g = h[4] - 48;
                        if (m1g >= 1)
                        {
                            if (m2g >= 1)
                            {
                                while (convm1g != m1g)
                                {
                                    m2g += 9;
                                    convm1g++;
                                }
                                min = m2g + m1g;
                                comboBox5.Text = Convert.ToString(min);
                            }
                            else
                            {
                                min = m1g;
                                comboBox5.Text = Convert.ToString(min + "0");
                            }
                        }
                        else
                        {
                            if (m2g != 0)
                            {
                                min = m2g;
                                comboBox5.Text = Convert.ToString("0" + min);
                            }
                            else
                            {
                                comboBox5.Text = Convert.ToString("00");
                            }
                        }
                        s1g = h[6] - 48;
                        s2g = h[7] - 48;
                        if (s1g >= 1)
                        {
                            if (s2g >= 1)
                            {
                                while (convs1g != s1g)
                                {
                                    s2g += 9;
                                    convs1g++;
                                }
                                sec = s2g + s1g;
                                comboBox4.Text = Convert.ToString(sec);
                            }
                            else
                            {
                                sec = s1g;
                                comboBox4.Text = Convert.ToString(sec + "0");
                            }
                        }
                        else
                        {
                            if (s2g != 0)
                            {
                                sec = s2g;
                                comboBox4.Text = Convert.ToString("0" + sec);
                            }
                            else
                            {
                                comboBox4.Text = Convert.ToString("00");
                            }
                        }

                    }
                }



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            valeurid = editer.Valid;
            verif = false;
            try
            {
                h0 = DateTime.Now.Hour;
                h1 = DateTime.Now.Minute;
                h2 = DateTime.Now.Second;

                hS = comboBox3.Text;
                mS = comboBox5.Text;
                sS = comboBox4.Text;
                Hs = int.Parse(hS);
                Ms = int.Parse(mS);
                Ss = int.Parse(sS);
                timeNow = hS + ":" + mS + ":" + sS;//création de l'heure.
                if (comboBox7.Text == "lente")
                {
                    recupVit = 1;
                }
                else
               if (comboBox7.Text == "moyenne")
                {
                    recupVit = 2;
                }
                else
               if (comboBox7.Text == "rapide")
                {
                    recupVit = 3;
                }
                if (comboBox6.Text == "gauche")
                {
                    recupDir = 2;
                }
                else
                if (comboBox6.Text == "droite")
                {
                    recupDir = 1;
                }

                while (i != h0)//passer les heures en secondes
                {
                    h0b += 3600;
                    i++;
                }
                while (i1 != h1)//passer les minutes en secondes
                {
                    h1b += 60;
                    i1++;
                }
                while (i2 != Hs)//passer les heures en secondes
                {
                    h0c += 3600;
                    i2++;
                }
                while (i3 != Ms)//passer les minutes en secondes
                {
                    h1c += 60;
                    i3++;
                }
                h3 = h0b + h1b + h2;//permet d'avoir l'heure en secondes
                Th1 = h0c + h1c + Ss;

                recupcb1 = comboBox1.Text;
                cb1 = int.Parse(recupcb1);

                recupcb2 = comboBox2.Text;
                cb2 = int.Parse(recupcb2);

                MySqlCommand viewcmd = new MySqlCommand("SELECT * FROM trajets", cnx);//selection de toute la base test
                using (MySqlDataReader Liretab = viewcmd.ExecuteReader())//lectcure dans table
                {
                    while (Liretab.Read())//tant qu'on lit dans la bd
                    {
                        //transfere valeur de bd au programme
                        string ID = Liretab["ID"].ToString();
                        string heuredep = Liretab["heuredep"].ToString();
                        h = Encoding.UTF8.GetBytes(heuredep);
                        int ID2 = Int32.Parse(ID);
                        //test || decomposition de l'heure
                        //renitialisation a chaque début de boucle
                        if (valeurid != ID2)//ne pas faire pour le trajet selectionné
                        {
                            i4 = 0;
                            i5 = 0;
                            convm1g = 0;
                            convh1g = 0;
                            convs1g = 0;
                            //
                            h1g = h[0] - 48;
                            h2g = h[1] - 48;
                            if (h1g >= 1)
                            {
                                while (convh1g != h1g)//tant que l'attribut convh1g n'est pas égale à h1g on rajoute 9 ce qui fait pour 22 2+(2+9*2) donc 22
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

                            if (Th1 != totale)
                            {
                                if (cb2 - cb1 == 1 || cb1 - cb2 == 5)//si on veut parcourir 1 station
                                {
                                    if (Th1 >= totale - 90 && Th1 <= totale + 90)
                                    {
                                        totale = hours + min + sec;
                                        break;

                                    }
                                }
                                else
                               if (cb2 - cb1 == 2 || cb1 - cb2 == 4)//si on veut parcourir 2 stations
                                {
                                    if (Th1 >= totale - 180 && Th1 <= totale + 180)
                                    {
                                        totale = hours + min + sec;
                                        break;
                                    }
                                }
                                else
                                if (cb2 - cb1 == 3 || cb1 - cb2 == 3)//si on veut parcourir 3 stations
                                {
                                    if (Th1 >= totale - 270 && Th1 <= totale + 270)
                                    {
                                        totale = hours + min + sec;
                                        break;
                                    }
                                }
                                else
                                if (cb2 - cb1 == 4 || cb1 - cb2 == 2)//si on veut parcourir 3 stations
                                {
                                    if (Th1 >= totale - 360 && Th1 <= totale + 360)
                                    {
                                        totale = hours + min + sec;
                                        break;
                                    }
                                }
                                else
                                if (cb2 - cb1 == 5 || cb1 - cb2 == 1)//si on veut parcourir 3 stations
                                {
                                    if (Th1 >= totale - 450 && Th1 <= totale + 450)
                                    {
                                        totale = hours + min + sec;
                                        break;
                                    }
                                }
                            }
                            else//Un trajet est déjà programmé pour cette heure là.
                            {
                                totale = hours + min + sec;
                                break;

                            }
                        }
                    }

                }
                if (cb1 != cb2)
                {
                    if (Th1 != totale)
                    {
                        if (cb2 - cb1 == 1 || cb1 - cb2 == 5)//si on veut parcourir 1 station
                        {
                            if (Th1 >= totale - 90 && Th1 <= totale + 90)
                            {
                                MessageBox.Show("Un trajet sera déjà commencé.");
                                verif = true;
                            }
                        }

                        if (cb2 - cb1 == 2 || cb1 - cb2 == 4)//si on veut parcourir 2 stations
                        {
                            if (Th1 >= totale - 180 && Th1 <= totale + 180)
                            {
                                MessageBox.Show("Un trajet sera déjà commencé.");
                                verif = true;
                            }
                        }

                        if (cb2 - cb1 == 3 || cb1 - cb2 == 3)//si on veut parcourir 3 stations
                        {
                            if (Th1 >= totale - 270 && Th1 <= totale + 270)
                            {
                                MessageBox.Show("Un trajet sera déjà commencé.");
                                verif = true;
                            }
                        }
                        if (cb2 - cb1 == 4 || cb1 - cb2 == 2)//si on veut parcourir 3 stations
                        {
                            if (Th1 >= totale - 360 && Th1 <= totale + 360)
                            {
                                MessageBox.Show("Un trajet sera déjà commencé.");
                                verif = true;
                            }
                        }
                        if (cb2 - cb1 == 5 || cb1 - cb2 == 1)//si on veut parcourir 3 stations
                        {
                            if (Th1 >= totale - 450 && Th1 <= totale + 450)
                            {
                                MessageBox.Show("Un trajet sera déjà commencé.");
                                verif = true;
                            }
                        }
                        if (verif != true)
                        {
                            if (comboBox3.Text == "")
                            {
                                MessageBox.Show("Entrez l'heure de départ");
                            }
                            else
                            if (comboBox5.Text == "")
                            {
                                MessageBox.Show("Entrez les minutes de départ");
                            }
                            else
                            if (comboBox4.Text == "")
                            {
                                MessageBox.Show("Entrez les secondes de départ");
                            }
                            else
                            if (comboBox1.Text == "")
                            {
                                MessageBox.Show("Entrez le point de départ");
                            }
                            else
                            if (comboBox2.Text == "")
                            {
                                MessageBox.Show("Entrez la destination");
                            }
                            else
                            if (comboBox3.Text == "00:00:00" || comboBox4.Text == "00:00:00" || comboBox5.Text == "00:00:00")
                            {
                                MessageBox.Show("Entrez l'heure de départ");
                            }
                            else
                            if (textBox2.Text == "" || textBox2.Text == "00:00:00")
                            {
                                MessageBox.Show("Entrez la durée du chargement");
                            }
                            else
                            if (textBox1.Text == "" || textBox1.Text == "00:00:00")
                            {
                                MessageBox.Show("Entrez la durée de déchargement");
                            }
                            else
                            {


                                MySqlCommand insertcmd = new MySqlCommand("UPDATE trajets  SET heuredep=@heuredep, charg=@charg, dechar=@dechar, depart=@depart, destination=@destination, vitesse=@vitesse, direction=@direction WHERE ID=@ID", cnx);
                                //transfere valeur de bd au programme
                                insertcmd.Parameters.AddWithValue("@charg", textBox2.Text);//donne la valeur de la textBox2 au paramètre @charg de insertcmd
                                insertcmd.Parameters.AddWithValue("@dechar", textBox1.Text);
                                insertcmd.Parameters.AddWithValue("@depart", int.Parse(comboBox1.Text));
                                insertcmd.Parameters.AddWithValue("@destination", int.Parse(comboBox2.Text));
                                insertcmd.Parameters.AddWithValue("@heuredep", timeNow);
                                insertcmd.Parameters.AddWithValue("@ID", valeurid);
                                insertcmd.Parameters.AddWithValue("@vitesse", recupVit);
                                insertcmd.Parameters.AddWithValue("@direction", recupDir);
                                //
                                insertcmd.ExecuteNonQuery();//execution requete sql
                                insertcmd.Parameters.Clear();//parametre cmd effacer
                                MessageBox.Show("Edité.");
                            }
                        }
                    }
                    else
               if (Th1 == totale)
                    {
                        MessageBox.Show("Un trajet est déjà programmé pour cette heure la");
                    }


                }
                else
                {
                    MessageBox.Show("Le départ et le trajet ne peut pas être identique");
                }
            }
            catch (Exception)
            {
                if (comboBox3.Text == "")
                {
                    MessageBox.Show("Entrez l'heure de départ");
                }
                else
                if (comboBox5.Text == "")
                {
                    MessageBox.Show("Entrez les minutes de départ");
                }
                else
                if (comboBox4.Text == "")
                {
                    MessageBox.Show("Entrez les secondes de départ");
                }
                else
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("Entrez le point de départ");
                }
                else
                   if (comboBox2.Text == "")
                {
                    MessageBox.Show("Entrez la destination");
                }
            }
        }
    

        private void Editer2_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
    