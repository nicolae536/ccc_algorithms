using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace action_bidder
{

    class Auction
    {
        private int currentPrice;
        private string winnderName = "";
        private int maximumPrice;
        private string history = "";
        private int buyNow;

        public Auction(string biddersIn)
        {
            string[] bids = biddersIn.Split(',');
            currentPrice = Int32.Parse(bids[0]);
            history += "-," + currentPrice;
            buyNow = Int32.Parse(bids[1]);
            StartAuction(bids);
            LogResult();
        }

        private void StartAuction(string[] bids)
        {
            for (int i = 2; i < bids.Length - 1; i += 2)
            {
                int bidValue = Int32.Parse(bids[i + 1]);
                string bidderName = bids[i];

                if (winnderName == "" && bidValue >= currentPrice)
                {
                    winnderName = bidderName;
                    maximumPrice = bidValue;
                    history += "," + bidderName + "," + currentPrice;
                }
                else if (bidValue > currentPrice && bidValue > maximumPrice)
                {
                    if (winnderName != bidderName)
                    {
                        currentPrice = maximumPrice + 1;
                        history += "," + bidderName + "," + currentPrice;
                    }

                    winnderName = bidderName;
                    maximumPrice = bidValue;

                    //if (buyNow != 0 && buyNow <= maximumPrice)
                    //{
                    //    history += "," + bidderName + "," + buyNow;
                    //    return;
                    //}
                }
                else if (bidValue >= currentPrice && winnderName != bidderName)
                {

                    if (maximumPrice == bidValue)
                    {
                        currentPrice = bidValue;
                    }
                    else
                    {
                        currentPrice = bidValue + 1;
                    }


                    if (buyNow != 0 && buyNow <= currentPrice)
                    {
                        history += "," + winnderName + "," + buyNow;
                        return;
                    }
                    history += "," + winnderName + "," + currentPrice;
                }
            }
        }

        private void LogResult()
        {
            Console.WriteLine("Winner : " + winnderName + "," + currentPrice);
            Console.WriteLine("History : " + history);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs =
            {
                "1,15,A,5,B,10,A,8,A,17,B,17",
                "100,0,C,100,C,115,C,119,C,121,C,144,C,154,C,157,G,158,C,171,C,179,C,194,C,206,C,214,C,227,C,229,C,231,C,298",
                "100,325,C,100,C,115,C,119,C,121,C,144,C,154,C,157,G,158,C,171,C,179,C,194,C,206,C,214,C,227,C,229,C,231,C,298",
                "100,160,C,100,C,115,C,119,C,121,C,144,C,154,C,157,G,158,C,171,C,179,C,194,C,206,C,214,C,227,C,229,C,231,C,298",
                "1,0,nepper,15,hamster,24,philipp,30,mmautne,31,hamster,49,hamster,55,thebenil,57,fliegimandi,59,ev,61,philipp,64,philipp,65,ev,74,philipp,69,philipp,71,fliegimandi,78,hamster,78,mio,95,hamster,103,macquereauxpl,135",
                "1,120,6a,17,kl,5,kl,10,kl,15,cs,28,kl,20,kl,25,hr,35,hr,40,hr,41,hl,42,hr,43,hr,44,hl,44,hl,49,hr,47",
                "1,47,6a,17,kl,5,kl,10,kl,15,cs,28,kl,20,kl,25,hr,35,hr,40,hr,41,hl,42,hr,43,hr,44,hl,44,hl,49,hr,47"
                //"100,160,C,100,C,115,C,119,C,121,C,144,C,154,C,157,G,158,C,171,C,179,C,194,C,206,C,214,C,227,C,229,C,231,C,298"
            };

            foreach (string item in inputs)
            {
                new Auction(item);
            }
            Console.ReadLine();
        }
    }
}
