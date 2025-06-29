using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prison_Escape_Game.Model.GameObjects
{
    public class Player
    {
        private int health {  get; set; }
        private string name { get; set; }

        private Item[] inventory;
    }
}
