using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp5
{
    internal class Gracz
    {

        private char activePlayer;
        public Gracz()
        {

            activePlayer = 'O';

        }

        public string GetP()
        {
            return activePlayer.ToString();
        }

        public void SwitchP()
        {
            activePlayer = (activePlayer == 'O') ? 'X' : 'O';
        }
        public void Reset()
        {
            activePlayer = 'O';

        }
    }
}
