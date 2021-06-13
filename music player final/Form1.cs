using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace music_player_final
{
    public partial class Form1 : Form
    {
        int stratindex = 0;
        string[] filename, filepath;
        Boolean playnext = false;
        int playlist_count = 0;
        int mouse_click_count;
        bool _playing = false;
        int soundclick = 0;
        bool mediaend = false;
        int repeat_click = 0;
        int media_change = 0;
        public Form1()
        {
            InitializeComponent();
        }

         OpenFileDialog opnfliedio = new OpenFileDialog();
         public void open_files()
         { // open files in dioalog box
             stratindex = 0;
             playnext = false;
             opnfliedio.Multiselect = true;
             opnfliedio.Filter = "(mp3,wav,mp4,mov,wmv,mpg,avi,3gp,flv,srt)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg;*.avi;*.3pg;*.flv;*.srt|all files|*.*";
             var myplaylist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myplaylist");

             listBox1.Items.Clear();
             if (opnfliedio.ShowDialog() == System.Windows.Forms.DialogResult.OK)
             {
                 filepath = opnfliedio.FileNames;
                 filename = opnfliedio.SafeFileNames;
                 for (int i = 0; i < filepath.Length; i++)
                 {
                     var mediaitem = axWindowsMediaPlayer1.newMedia(filepath[i]);
                     myplaylist.appendItem(mediaitem);
                     listBox1.Items.Add(filename[i]);
                 }
                 axWindowsMediaPlayer1.currentPlaylist = myplaylist;
                 axWindowsMediaPlayer1.Ctlcontrols.play();
             }
         }
         public void stopplyer() // stop playing
         {
             axWindowsMediaPlayer1.Ctlcontrols.stop();
         }
                    
        private void bunifuImageButton13_Click(object sender, EventArgs e)
        {

        }

      

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void maximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
                 
        }

        private void minimaize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

        private void menuact_Click(object sender, EventArgs e)
        {
            if (panel3.Width <= 85)
            {
                panel3.Width = 180;
                menuact.Image = menuback.Image;
            }
            else
            {
                menuact.Image = menu.Image;
                panel3.Width = 63;
            }
        }

        private void btnpuse_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void btnplay_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void btnstope_Click(object sender, EventArgs e)
        {
            stopplyer();
            isplaying = true;
        }

        private void btnopenfile_Click(object sender, EventArgs e)
        {
            this.open_files();
            
        }

        private void btnhome_Click(object sender, EventArgs e)
        {

            btnhome.IsTab = true;
            btnhome.TabStop = false;
            shortcut.TabStop = true;
            setting.TabStop = true;
            setting_panl.Visible = false;
            axWindowsMediaPlayer1.BringToFront();
            playlist_count = 0;
        }

        private void btnplayilist_Click(object sender, EventArgs e)
        {
            shortcut1.Visible = false;
          setting_panl.Visible = false;
            listBox1.BringToFront();
            playlist_count++;
            if (listBox1.Visible == true)
            {
                listBox1.Visible = false;
                btnhome.selected = true;
                btnhome.IsTab = true;
                shortcut.selected = false;
                setting.selected = false;
            
            }
            else
            {
                listBox1.Visible = true;
            }

        }

        private void soundline_ValueChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = soundline.Value;
            soundlable.Text = soundline.Value.ToString();
        }

        public bool isplaying // check play or puse 
        {
            get
            {
                return _playing;
            }
            set
            {
                _playing = value;
                if (_playing)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    btnaction.Image = btnpuse.Image;
                }
                else
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    btnaction.Image = btnplay.Image;

                }
            }

        }

        private void ptnnext_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.next();
            isplaying = false;
        }
        public EventHandler onAction = null;

        private void btnaction_Click(object sender, EventArgs e)
        {
            isplaying = !isplaying;
            if (onAction != null)
            {
                onAction.Invoke(this, e);
            }
        }

        private void ptnplayback_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.previous();
            // axWindowsMediaPlayer1.Ctlcontrols.play();
            isplaying = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            startlable.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;  // print current position of media file in the star_lable
            endlable.Text = axWindowsMediaPlayer1.Ctlcontrols.currentItem.durationString.ToString(); // print total duration of current media file 
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                playline.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition; // set value of current media file position  into the progras bar
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                playline.MaximumValue = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration; //change maxvalue of duration into the progressbar
                timer1.Start();
                songname.Text = axWindowsMediaPlayer1.currentMedia.name;// print name of current flie name in the song lable
                isplaying = false;
              
               
                // change btn_action image;


               /* if (axWindowsMediaPlayer1.Ctlcontrols.currentItem.sourceURL.EndsWith(".mp3", false, null)) // chech file type is mp3 or not
                {
                    mp3panal1.Visible = true;
                }
                else
                {
                    mp3panal1.Visible = false;
                }*/



            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                timer1.Stop();
                isplaying = true;
                mediaend = false; // if pause  the media  media end is false  
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                //when media is stope mean stop button is press then timer is off and beging media in 0
                timer1.Stop();
                playline.Value = 0;

            }

            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                stratindex = axWindowsMediaPlayer1.Ctlcontrols.currentItem.attributeCount;
                // when media player is end and repeat_click value is odd the media is repeat
                media_change++;
                if (repeat_click % 2 == 1)
                {
                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                   
                }
                else
                {
                    axWindowsMediaPlayer1.settings.setMode("loop", false);
                    

                }
               
               
                if (axWindowsMediaPlayer1.Ctlcontrols.currentItem.attributeCount <= axWindowsMediaPlayer1.currentPlaylist.count -1 && repeat_click % 2 == 0)
                {
                   
                 axWindowsMediaPlayer1.Ctlcontrols.next();
                // axWindowsMediaPlayer1.settings.autoStart = false;       
                }
                else
                {
                   
                    isplaying = true;
                    
                  
                    // change btn_action button image
                    // media change to next one when media is end

                }

            }
         
        }

        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            //when click mouse left button on window media player is play or not  
            if (e.nButton == 1)
            {
                mouse_click_count++;

                if (mouse_click_count % 2 == 1)
                {
                    isplaying = true;
                }
                else
                {
                    isplaying = false;
                }

            }
        }

        private void btnrepeat_Click(object sender, EventArgs e)
        {
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
            isplaying = false;
        }

        private void axWindowsMediaPlayer1_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                WMPLib.IWMPMedia2 errsource = e.pMediaObject as WMPLib.IWMPMedia2;
                WMPLib.IWMPErrorItem erroritem = errsource.Error;
                MessageBox.Show("Error" + erroritem.errorCode.ToString("X") + "in" + errsource.sourceURL);

            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Error.");
            }
        }

       

        private void repeateaction_Click_1(object sender, EventArgs e)
        {
            // count reapeat press 
            repeat_click++;
            if (repeat_click % 2 == 1)
            {
                repeateaction.Image = btnrepeatgreen.Image;
            }
            else
            {
                repeateaction.Image = btnrepeat.Image;
            }
        }

        private void playline_ValueChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition =  playline.Value ;
        }

        private void soundaction_Click(object sender, EventArgs e)
        {
            soundclick++;
            if (soundclick %2 == 1)
            {
                axWindowsMediaPlayer1.settings.mute = true;
                soundaction.Image = btnmute.Image;
            }
            else
            {
                axWindowsMediaPlayer1.settings.mute = false;
                soundaction.Image = btnsound.Image;
            }
        }

        private void maximize_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(maximize, "Maximize");
        }

        private void minimaize_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(minimaize, "Minimize");
        }

        private void repeateaction_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(repeateaction, "Repeat");
        }

        private void btnstope_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(btnstope, "Stop");
        }

        private void ptnplayback_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(ptnplayback, "Previous");
        }

        private void btnaction_MouseMove(object sender, MouseEventArgs e)
        {
            if(isplaying == false)
            {
                toolTip1.SetToolTip(btnaction, "Pause");
            }
            else
            {
                toolTip1.SetToolTip(btnaction, "Play");
            }
        }

        private void btnclose_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(btnclose, "Close");
        }

        private void ptnnext_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(ptnnext, "Next");
        }

        private void btnopenfile_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(btnopenfile, "Open File");
        }

        private void btnplayilist_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(btnplayilist, "Playlist");
        }

        private void soundaction_MouseMove(object sender, MouseEventArgs e)
        {
            if (soundaction.Image == btnsound.Image)
            {
                toolTip1.SetToolTip(soundaction, "Mute");
            }
            else
            {
                toolTip1.SetToolTip(soundaction, "Sound");
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // when press space button media is pause or play
            if (e.KeyChar == (char)Keys.Space)
            {
                mouse_click_count++;

                if (mouse_click_count % 2 == 1)
                {
                    isplaying = true;
                }
                else
                {
                    isplaying = false;
                }
            }

            char[] c = Keys.C.ToString().ToLower().ToCharArray();
            // when press "c" button appliction is exit
            if (e.KeyChar == (char)c[0])
            {
                Application.Exit();
            }

            char[] o = Keys.O.ToString().ToLower().ToCharArray();
            // when press "o" button new file dioalog is open to choose media file
            if (e.KeyChar == o[0])
            {
                open_files();
            }
            // if press "b" button appliction is minimize
            char[] b = Keys.B.ToString().ToLower().ToCharArray();
            if (e.KeyChar == b[0] && WindowState != FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                isplaying = true;
            }

           
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // when press n button move to 30  time in media;
            int y = playline.MaximumValue / 30;
            if (axWindowsMediaPlayer1.Ctlcontrols.currentPosition < playline.MaximumValue - y) // go forword some distant in media
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        playline.Value = (int)(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
                        axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (axWindowsMediaPlayer1.Ctlcontrols.currentPosition + y);
                    }
                }
            }

            if (axWindowsMediaPlayer1.Ctlcontrols.currentPosition > y)  // go backword some distant in media
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        playline.Value = (int)(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
                        axWindowsMediaPlayer1.Ctlcontrols.currentPosition = (axWindowsMediaPlayer1.Ctlcontrols.currentPosition - y);
                    }
                }
            }
        }

        private void shortcut_Click(object sender, EventArgs e)
        {
            btnhome.TabStop = true;
            setting.TabStop = true;
            shortcut.IsTab = true;
            shortcut.TabStop = false;
            shortcut1.Visible = true;
            listBox1.Visible = false;
            shortcut1.BringToFront();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                shortcut1.lable1.ForeColor = Color.LimeGreen;
                shortcut1.lable2.ForeColor = Color.LimeGreen;
                shortcut1.lable3.ForeColor = Color.LimeGreen;
                shortcut1.lable4.ForeColor = Color.LimeGreen;
                shortcut1.lable5.ForeColor = Color.LimeGreen;
                shortcut1.lable6.ForeColor = Color.LimeGreen;
                playline.IndicatorColor = Color.LimeGreen;
                soundline.IndicatorColor = Color.LimeGreen;
            }
            else if ( comboBox1.SelectedIndex == 1)
            {
                playline.IndicatorColor = Color.Red;
                soundline.IndicatorColor = Color.Red;
                shortcut1.lable1.ForeColor = Color.Red;
                shortcut1.lable2.ForeColor = Color.Red;
                shortcut1.lable3.ForeColor = Color.Red;
                shortcut1.lable4.ForeColor = Color.Red;
                shortcut1.lable5.ForeColor = Color.Red;
                shortcut1.lable6.ForeColor = Color.Red;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                playline.IndicatorColor = Color.DarkOrange;
                soundline.IndicatorColor = Color.DarkOrange;
                shortcut1.lable1.ForeColor = Color.DarkOrange;
                shortcut1.lable2.ForeColor = Color.DarkOrange;
                shortcut1.lable3.ForeColor = Color.DarkOrange;
                shortcut1.lable4.ForeColor = Color.DarkOrange;
                shortcut1.lable5.ForeColor = Color.DarkOrange;
                shortcut1.lable6.ForeColor = Color.DarkOrange;
            }
            else if ( comboBox1.SelectedIndex == 3)
            {
                playline.IndicatorColor = Color.DodgerBlue;
                soundline.IndicatorColor = Color.DodgerBlue;
                shortcut1.lable1.ForeColor = Color.DodgerBlue;
                shortcut1.lable2.ForeColor = Color.DodgerBlue;
                shortcut1.lable3.ForeColor = Color.DodgerBlue;
                shortcut1.lable4.ForeColor = Color.DodgerBlue;
                shortcut1.lable5.ForeColor = Color.DodgerBlue;
                shortcut1.lable6.ForeColor = Color.DodgerBlue;
            }
            
        }

        private void setting_Click(object sender, EventArgs e)
        {
           setting_panl.Visible = true;
           setting_panl.BringToFront();
           btnhome.TabStop = true;
            setting.IsTab = true;
            setting.TabStop = false;
        }


    }
}
