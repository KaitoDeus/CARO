using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    public class Player
    {
        private string name;
        public string Name 
        { 
            get => name; 
            set => name = value; 
        }

        private Image mark;
        public Image Mark 
        { 
            get => mark;
            set => mark = value; 
        }

        // Avatar riêng biệt với Mark - hiển thị ở panel thông tin người chơi
        private Image avatar;
        public Image Avatar
        {
            get => avatar;
            set => avatar = value;
        }

        public Player(string name, Image mark)
        {
            this.Name = name;
            this.Mark = mark;
            this.Avatar = mark; // Mặc định avatar = mark
        }
    }
}
