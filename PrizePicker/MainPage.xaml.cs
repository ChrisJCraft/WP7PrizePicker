using System;
using System.Security.Cryptography;
using System.Windows;
using Microsoft.Phone.Controls;

namespace PrizePicker
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //disable DrawButton button initially 
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = false;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            this.Focus(); //Added to dismiss the SIP (Soft Input Panel) it is already visible
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true; //enable the DrawButton button
            if (listBoxWinners.Items.Count > 0)
            {
                var result = MessageBox.Show("Are you sure? This action will clear the Winners list.", "Alert", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            int start = Convert.ToInt32(textBoxStart.Text);
            int stop = Convert.ToInt32(textBoxStop.Text);

            if (start > stop)
            {
                MessageBox.Show("Are you sure? This action will clear the Winners list.", "Alert", MessageBoxButton.OKCancel);

                return;
            }

            listBoxHopefuls.Items.Clear();
            listBoxWinners.Items.Clear();

            for (int i = start; i <= stop; i++)
            {
                listBoxHopefuls.Items.Add(i);
            }
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            this.Focus(); //Added to dismiss the SIP (Soft Input Panel) it is already visible
            if (listBoxHopefuls.Items.Count == 0)
            {
                MessageBox.Show("Everybody has won something, go home.", "Alert", MessageBoxButton.OKCancel);
                return;
            }

            var randomBytes = new byte[4];

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            int pick = seed % listBoxHopefuls.Items.Count;

            string winner = listBoxHopefuls.Items[pick].ToString();
            listBoxHopefuls.Items.RemoveAt(pick);

            listBoxWinners.Items.Add(winner);
        }
    }
}