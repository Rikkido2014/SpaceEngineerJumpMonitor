
        public void Main(string argument)
        {
            List<IMyJumpDrive> drives = new List<IMyJumpDrive>();
            GridTerminalSystem.GetBlocksOfType(drives);

            IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName("JumpLcd") as IMyTextPanel;
            //pipe = 15 Space = 22

            float cPower = 0f;
            float mPower = 0f;

            foreach (var drive in drives)
            {
                cPower += drive.CurrentStoredPower;
                mPower += drive.MaxStoredPower;
            }

            int progress = Convert.ToInt32(Math.Round((cPower / mPower) * 100));


            Prefix prefix = GetPrefix(cPower);
            switch (prefix)
            {
                case Prefix.N:
                    cPower *= (10 ^ 6);
                    mPower *= (10 ^ 6);
                    break;

                case Prefix.k:
                    cPower *= (10 ^ 3);
                    mPower *= (10 ^ 3);
                    break;

                case Prefix.M: break;
                default: break;
            }

            int barCount = progress / 2;
            Echo(barCount.ToString());
            int SpaCount = 50 - barCount;
            Echo(SpaCount.ToString());
            string progressBar = new string('|', barCount) + new string('ˉ', SpaCount);
            string chargingText = $" Charging {progress}%\n" +
                                  $" [{progressBar}]";
            string availableText = $"Ready to JUMP";

            string writeText = $" Current available Drives\n" +
                               $" {drives.Count}\n\n" +
                               $" Current Stored Power\n" +
                               $" {cPower}/{mPower}[{(prefix == Prefix.N ? "" : prefix.ToString())}W]\n\n" +
                               $" Drive Status\n"+
                               $" {(progress == 100 ? availableText:chargingText)}";
                               

            lcd.WritePublicText(writeText);
        }
        public Prefix GetPrefix(float num)
        {
            num *= (10 ^ 6);
            return num >= (10 ^ 6) ? Prefix.M : num >= (10 ^ 3) ? Prefix.k : Prefix.N;
        }
        public enum Prefix : int { k = 1000, M = 1000000, N = 0 }
    }

