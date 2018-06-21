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

namespace AtomikhErgasia2Monadwn
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void change2_Click(object sender, EventArgs e)
        {
            if (red2.Visible == true)
            {
                red2.Visible = false;
                green2.Visible = true;
            }
            else if (green2.Visible == true)
            {
                green2.Visible = false;
                yellow2.Visible = true;
            }
            else if (yellow2.Visible == true)
            {
                yellow2.Visible = false;
                red2.Visible = true;
            }

            //AdvancedLevel
            if (green2.Visible == true)
            {//If the second traffic light becomes manually green, make the other one red
                red1.Visible = true;
                green1.Visible = false;
                yellow1.Visible = false;
            }
        }

        private void change1_Click(object sender, EventArgs e)
        {
            if (red1.Visible == true)
            {
                red1.Visible = false;
                green1.Visible = true;
            }
            else if (green1.Visible == true)
            {
                green1.Visible = false;
                yellow1.Visible = true;
            }
            else if (yellow1.Visible == true)
            {
                yellow1.Visible = false;
                red1.Visible = true;
            }

            //AdvancedLevel
            if (green1.Visible == true)
            {//If the second traffic light becomes manually green, make the other one red
                red2.Visible = true;
                green2.Visible = false;
                yellow2.Visible = false;
            }
        }
        int trafficlight1Pos = 550;
        int trafficlight2Pos = 200;
        int speed = 4;
        //we declare a picturebox array that will contain the two cars of the line no1
        //so that we don't write the same code twice
        PictureBox[] carofline1 = new PictureBox[2]; 
        private void timer1_Tick(object sender, EventArgs e)//checking the car movement
        {
            carofline1[0] = car1;
            carofline1[1] = car3;
            //car line no1
                for (int i = 0; i < 2; i++)
                {//if the car is not close to the traffic lights position and the red light is not visible
                    if (!(carofline1[i].Left < trafficlight1Pos + 10 && carofline1[i].Left >= trafficlight1Pos  && green1.Visible == false))
                    {//if the car is near the traffic light and the state of it is yellow, slow down
                        if ((carofline1[i].Left < trafficlight1Pos + 70 && carofline1[i].Left >= trafficlight1Pos) && yellow1.Visible == true)
                        {
                            carofline1[i].Left -= (speed - 2);
                        }
                        else//move normally
                        {
                            carofline1[i].Left -= speed;
                        }                       
                    }
                 }                               
            //and if the cars reach the end of the form, they go back to their initial position
            if (car1.Left <= 0)
            {
                car1.Left = 915;
            }
            //we put this car more back than the width of the form to simulate time delay and distance between the 2 cars 
            if (car3.Left <= 0)
            {
                car3.Left = 1100;
            }


            //car line no2
            if (!(car2.Top <= trafficlight2Pos && car2.Top > trafficlight2Pos - 10 && green2.Visible == false))
            {
                //if the car is near the traffic light and the state of it is yellow, slow down
                if ((car2.Top > trafficlight2Pos - 70 && car2.Top <= trafficlight2Pos) && yellow2.Visible == true)
                 {
                        car2.Top += (speed - 2);
                 }
                 else//move normally
                 {
                      car2.Top += speed;
                 }
                
            }
            
            //if the car reaches the end of the form, go back to its initial position
            if (car2.Top >= 772)
            {
                car2.Top = 10;
            }
            
        }
        int auto_count1 = 0, auto_count2 = 0;
        /*we declare the variables that will store the values of the user-given times of each traffic light here,
        because if we passed them directly from the textbox they would change real-time*/
        int green1time, yellow1time, red1time, green2time, yellow2time, red2time;
        private void timer3_Tick(object sender, EventArgs e)//the timer that changes automatically the traffic lights
        {        
            //because we started counting from 0,in the conditions we put < and not <=
            if (auto_count1 < green1time)
            {
                //first we change all the traffic light conditions that might have been applied before to false 
                red1.Visible = false;
                yellow1.Visible = false;

                //and now we start making the automatic changes
                green1.Visible = true;                  
            }
            else if (auto_count1 < green1time + yellow1time)
            {
                green1.Visible = false;
                yellow1.Visible = true;
            }
            else if (auto_count1 < green1time + yellow1time + red1time - 1)
            {
                yellow1.Visible = false;
                red1.Visible = true;            
            }
            else//when auto_count1 is equal with the sum of all the given times of the traffic light
            {//start the loop from the beggining

                auto_count1 = -1;
            }
            
            if (auto_count2 < red2time)
            {
                //we follow the same procedure for the traffic light no2
                
                yellow2.Visible = false;
                green2.Visible = false; 
                //but this time we start from the red condition (Advanced Level)              
                red2.Visible = true;

            }
            else if (auto_count2 < green2time + red2time)
            {
                red2.Visible = false;
                green2.Visible = true;
            }
            else if (auto_count2 < green2time + red2time + yellow2time - 1)
            {
                yellow2.Visible = true;
                green2.Visible = false;
            }
            else
            {
                auto_count2 = -1;
            }
            //and in every tick increase the time of the counters
            auto_count1++;
            auto_count2++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            //I added sound so it can look more real :)
            string soundfile = string.Format("{0}//trafficjam.wav", Application.StartupPath);
            System.Media.SoundPlayer trafficsound = new System.Media.SoundPlayer(soundfile);
            trafficsound.PlayLooping();
        }

        private void auto_Click(object sender, EventArgs e)
        {
            //everytime the user clicks the button, if it was running before we stop it to check all the conditions and then we start it again
            timer3.Stop();
            bool b = true;
            //1st input control - fill all the requested times
            if (red1secs.Text == "" || yellow1secs.Text == "" || green1secs.Text == "" || red2secs.Text == "" || yellow2secs.Text == "" || green2secs.Text == "")
            {
                MessageBox.Show("Please fill all the text boxes!");
                b = false;
            }

            //2nd input control - give only int numbers
            int r1, y1, g1, r2, y2, g2;
            if (b == true)
            {
                if (!int.TryParse(red1secs.Text, out r1) || !int.TryParse(yellow1secs.Text, out y1) || !int.TryParse(green1secs.Text, out g1) || !int.TryParse(red2secs.Text, out r2) || !int.TryParse(yellow2secs.Text, out y2) || !int.TryParse(green2secs.Text, out g2))
                {
                    MessageBox.Show("You must give ONLY int numbers as input!");
                    b = false;
                }
            }
            //3rd input control - give only positive numbers
            if (b == true)
            {
                if ((int.Parse(red2secs.Text) <= 0  || int.Parse(green2secs.Text) <= 0 || int.Parse(yellow2secs.Text) <= 0 || int.Parse(red1secs.Text) <= 0 || int.Parse(green1secs.Text) <= 0 || int.Parse(yellow1secs.Text) <= 0))
                {
                    MessageBox.Show("You must give ONLY positive int numbers as input!");
                    b = false;
                }
            }
            //AdvancedLevel
            //4th input control - if everything else is ok, check if the sum of the green and the yellow lights is equal with the opposite red light
            if (b == true)
            {
                if (int.Parse(red2secs.Text) != int.Parse(green1secs.Text) + int.Parse(yellow1secs.Text))
                {
                    MessageBox.Show("Problem with time input: The sum of green + yello light no1 time must be the same with red light no2 !");
                    b = false;
                }
                if (int.Parse(red1secs.Text) != int.Parse(green2secs.Text) + int.Parse(yellow2secs.Text))
                {
                    MessageBox.Show("Problem with time input: The sum of green + yello light no2 time must be the same with red light no1!");
                    b = false;
                }
            }  

            //if all the tests have been passed, start the timer
            if (b == true)
            {
                timer3.Start();
                //pass the values of the traffic light times only when the user clicks the button
                green1time = int.Parse(green1secs.Text);
                yellow1time = int.Parse(yellow1secs.Text);
                red1time = int.Parse(red1secs.Text);
                green2time = int.Parse(green2secs.Text);
                yellow2time = int.Parse(yellow2secs.Text);
                red2time = int.Parse(red2secs.Text);
            }

        }

    }
}
