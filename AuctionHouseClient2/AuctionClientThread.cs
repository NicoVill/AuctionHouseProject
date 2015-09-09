using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AuctionHouseClient2
{
    class AuctionClientThread
    {
        public event BidReceived bidreceivedEvent;

        public void ReceiveBidInput(object streamReadObj)
        {
            string chat;
            StreamReader streamReader = (StreamReader)streamReadObj;

            while(true)
            {
                try
                {
                    chat = streamReader.ReadLine();

                }
                catch (IOException)
                {
                    
                    break;
                }

                bidreceivedEvent(chat);
            }
        }

    }
}
