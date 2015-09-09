using System;
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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AuctionHouseClient2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PORT = 12345;

        private NetworkStream networkstream;
        private StreamReader streamreader;
        private StreamWriter streamwriter;
        private Socket socketToServer;
        private IPEndPoint serverAddress;
        private AuctionClientThread ClientThreadMethod;
        private Thread receiveChatThread;

        private delegate void bidReceivetGUIDelegate(string chat);
        public MainWindow()
        {
            InitializeComponent();

            this.serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT);
            this.ClientThreadMethod = new AuctionClientThread();

            this.ClientThreadMethod.bidreceivedEvent += new BidReceived(BidReceivedHandler);


        }

        public void BidReceivedHandler(string chat)
        {
            txtb_Chat.Dispatcher.BeginInvoke(new bidReceivetGUIDelegate(this.bidReceivedHandlerInvoke), chat);
        }

        private void bidReceivedHandlerInvoke(string chat)
        {
            txtb_Chat.Text += chat + "\r\n";	// Write to chat output
        }

        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            this.socketToServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                this.socketToServer.Connect(this.serverAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not connect to server");
                Console.WriteLine(ex.ToString());
                throw;
            }

            this.networkstream = new NetworkStream(this.socketToServer);
            this.streamreader = new StreamReader(this.networkstream);
            this.streamwriter = new StreamWriter(this.networkstream);

            this.receiveChatThread = new Thread(new ParameterizedThreadStart(ClientThreadMethod.ReceiveBidInput));
            this.receiveChatThread.Start(this.streamreader);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        
    }
}
