﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace P2PNetworking
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class P2PService : IP2PService
    {
        #region Everything we need to receive messages

        DisplayMessageDelegate _displayMessageDelegate = null;

        /// <summary>
        /// The default constructor is only here for testing purposes.
        /// </summary>
        private P2PService()
        {
        }

        /// <summary>
        /// ChatBackend constructor should be called with a delegate that is capable of displaying messages.
        /// </summary>
        /// <param name="dmd">DisplayMessageDelegate</param>
        public P2PService(DisplayMessageDelegate dmd)
        {
            _displayMessageDelegate = dmd;
            StartService();
        }

        /// <summary>
        /// This method gets called by our friends when they want to display a message on our screen.
        /// We're really only returning a string for demonstration purposes ... it might be cleaner
        /// to return void and also make this a one-way communication channel.
        /// </summary>
        /// <param name="composite"></param>
        public void DisplayMessage(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (_displayMessageDelegate != null)
            {
                _displayMessageDelegate(composite);
            }
        }

        #endregion // Everything we need to receive messages

        #region Everything we need for bi-directional communication

        private string _myUserName = "Anonymous";
        private ServiceHost host = null;
        private ChannelFactory<IP2PService> channelFactory = null;
        private IP2PService _channel;

        /// <summary>
        /// The front-end calls the SendMessage method in order to broadcast a message to our friends
        /// </summary>
        /// <param name="text"></param>
        public void SendMessage(string text)
        {
            if (text.StartsWith("setname:", StringComparison.OrdinalIgnoreCase))
            {
                _myUserName = text.Substring("setname:".Length).Trim();
                _displayMessageDelegate(new CompositeType("Event", "Setting your name to " + _myUserName));
            }
            else
            {
                // In order to send a message, we call our friends' DisplayMessage method
                _channel.DisplayMessage(new CompositeType(_myUserName, text));
            }
        }

        private void StartService()
        {
            try
            {
                host = new ServiceHost(this);
                host.Open();
                channelFactory = new ChannelFactory<IP2PService>("ChatEndpoint");
                _channel = channelFactory.CreateChannel();

                // Information to send to the channel
                _channel.DisplayMessage(new CompositeType("Event", _myUserName + " has entered the conversation."));

                // Information to display locally
                _displayMessageDelegate(new CompositeType("Info", "To change your name, type setname: NEW_NAME"));
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }

        private void StopService()
        {
            if (host != null)
            {
                _channel.DisplayMessage(new CompositeType("Event", _myUserName + " is leaving the conversation."));
                if (host.State != CommunicationState.Closed)
                {
                    channelFactory.Close();
                    host.Close();
                }
            }
        }


        #endregion // Everything we need for bi-directional communication
    }
}
