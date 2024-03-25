using System;
using System.Drawing;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dice_Roll
{
    public partial class FrmDiceRoll : Form
    {
        //Dice faces
        Bitmap[] faces = new Bitmap[7]; //includes an image for rolling
        SoundPlayer play = new SoundPlayer();   //For the dice roll sound
        //Supports two dice
        Dice dice1 = new Dice();
        Dice dice2 = new Dice();
        static long doubles = 0;    //Count doubles when two dice

        public FrmDiceRoll()
        {
            InitializeComponent();
            //Load images for the dice
            Assembly prog = Assembly.GetExecutingAssembly();
            faces[0] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.dice3d160.png"));
            faces[1] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.one.png"));
            faces[2] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.two.png"));
            faces[3] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.three.png"));
            faces[4] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.four.png"));
            faces[5] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.five.png"));
            faces[6] = new Bitmap(prog.GetManifestResourceStream("Dice_Roll.six.png"));
            //Load sound file
            play.Stream = prog.GetManifestResourceStream("Dice_Roll.shake_dice.wav"); 
        }

        private void FrmDiceRoll_Load(object sender, EventArgs e)
        {
            //Put an image in the picture box to start
            PBDice1.Image = faces[0];
            PBDice2.Image = faces[0];
        }

        //Roll the dice
        private async void ButRoll_Click(object sender, EventArgs e)
        {
            //Only one roll at a time
            ButRoll.Enabled = false;
            //Don't interaction during roll
            ButReset.Enabled = false;
            ChkOneOrTwo.Enabled = false;
            PBDice1.Enabled = false;
            PBDice2.Enabled = false;
            //Clear previous roll image
            PBDice1.Image = faces[0];
            if (ChkOneOrTwo.Checked)
                //Roll dice two as well
                PBDice2.Image = faces[0];
            //Dice sound
            play.Play();
            //Roll the dice and update display
            PBDice1.Image = faces[await Roll()];
            if(ChkOneOrTwo.Checked)
                //Roll dice two as well
                PBDice2.Image = faces[dice2.RollNumber()];
            UpdateStats();
            //Can roll again
            PBDice2.Enabled = true;
            PBDice1.Enabled = true;
            ButRoll.Enabled = true;
            ButReset.Enabled = true;
            ChkOneOrTwo.Enabled = true;
        }

        private void ButReset_Click(object sender, EventArgs e)
        {
            dice1 = new Dice();
            dice2 = new Dice();
            doubles=0;
            //Reset starting image
            PBDice1.Image = faces[0];
            PBDice2.Image = faces[0];
            LblTotalDoubles.Text = (doubles).ToString();
            UpdateStats();
        }

        //Background roll, with pause, to allow a UI change
        private async Task<int> Roll()
        {
            
            //Brief pause
            await Task.Delay(500);
            //Roll dice
            return dice1.RollNumber();
        }

        //Update the various stats available
        void UpdateStats()
        {
            LblTotalNumRolls1.Text = dice1.TotalRolls.ToString();
            LblOnes1.Text = dice1.Rolls[0].ToString();
            LblTwos1.Text = dice1.Rolls[1].ToString();
            LblThrees1.Text = dice1.Rolls[2].ToString();
            LblFours1.Text = dice1.Rolls[3].ToString();
            LblFives1.Text = dice1.Rolls[4].ToString();
            LbLSixes1.Text = dice1.Rolls[5].ToString();
            LblSumRolls1.Text = dice1.TotalValues.ToString();
            LblMeanValue1.Text = dice1.MeanValueRolled.ToString();
            LblPreviousRoll1.Text = dice1.PreviousRoll.ToString();
            //See if two dice are being rolled
            if (ChkOneOrTwo.Checked)
            {
                //Update Dice Two Stats
                LblTotalNumRolls2.Text = dice2.TotalRolls.ToString();
                LblOnes2.Text = dice2.Rolls[0].ToString();
                LblTwos2.Text = dice2.Rolls[1].ToString();
                LblThrees2.Text = dice2.Rolls[2].ToString();
                LblFours2.Text = dice2.Rolls[3].ToString();
                LblFives2.Text = dice2.Rolls[4].ToString();
                LblSixes2.Text = dice2.Rolls[5].ToString();
                LblSumRolls2.Text = dice2.TotalValues.ToString();
                LblMeanValue2.Text = dice2.MeanValueRolled.ToString();
                LblPreviousRoll2.Text = dice2.PreviousRoll.ToString();
                //Update doubles count if not reset
                if (dice1.LastRoll > 0 && (dice1.LastRoll == dice2.LastRoll))
                    LblTotalDoubles.Text= (++doubles).ToString();
            }
        }

        //Show second dice?
        private void ChkOneOrTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkOneOrTwo.Checked)
                GrpDice2.Visible = true;
            else
                GrpDice2.Visible = false;
        }

        
    }
}
