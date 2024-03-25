using System;
using System.Threading;
using System.Linq;

namespace Dice_Roll
{
    public class Dice
    {
        //Store number of rolls of each number
        long[] rolls = { 0, 0, 0, 0, 0, 0 };
        //Store the last rolled number
        int lastRoll = 0;
        //Store the previously rolled number
        int previousRoll = 0;
        //Use random for rolling
        private static readonly Random roller = new Random();

        //Perform a dice roll
        public int RollNumber()
        {
            //Save previous number
            previousRoll = lastRoll;
            //Roll the dice (die!)
            lastRoll = roller.Next(0, 6) + 1;
            //Update all rolls
            rolls[lastRoll - 1]++;
            return lastRoll;
        }
        //Get last rolled number
        public int LastRoll => lastRoll;
        //Get previous rolled number
        public int PreviousRoll => previousRoll;
        //Totals of each number rolled
        public long[] Rolls => rolls;
        //Total of all rolls performed
        public long TotalRolls => rolls.Sum();
        //Total value of all rolled numbers
        public long[] TotalsOfRolledNumbers => rolls.Select((number, index) => number * (index + 1)).ToArray();
        //Total of all values rolled
        public long TotalValues => TotalsOfRolledNumbers.Sum();
        //Mean number rolled (double cast required to prevent integer only division) 
        public double MeanValueRolled => TotalValues / (double)TotalRolls;
    }
}
