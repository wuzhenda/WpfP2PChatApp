﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private P2PNetworking.P2PService _backend;
        public MainWindow()
        {
            InitializeComponent();
            _backend = new P2PNetworking.P2PService(this.DisplayMessage);
        }

        public void DisplayMessage(P2PNetworking.CompositeType composite)
        {
            string username = composite.Username == null ? string.Empty : composite.Username;
            string message = composite.Message == null ? string.Empty  : composite.Message;
            textBoxChatPane.Text += (username + ": " + message + Environment.NewLine);
        }

        private void textBoxEntryField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                _backend.SendMessage(textBoxEntryField.Text);
                textBoxEntryField.Clear();
            }
        }
    }
}
