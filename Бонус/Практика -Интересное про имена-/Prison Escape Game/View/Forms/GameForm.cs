using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prison_Escape_Game.View.Forms
{
    internal class GameForm : Form
    {
        private ProgressBar healthBar;
        private Label healthLabel;
        private const int InventorySlots = 5;
        private PictureBox[] inventorySlots;
        private Button exitButton;

        public GameForm()
        {
            InitializeComponents();
            SetupLayout();
            SetupEvents();
            this.DoubleBuffered = true; // Для плавной отрисовки
        }

        private void InitializeComponents()
        {
            // Настройка формы
            this.ClientSize = new Size(800, 600);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Инициализация Health Bar
            healthBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Value = 100,
                Height = 25,
                ForeColor = Color.Red,
                BackColor = Color.DarkGray
            };

            healthLabel = new Label
            {
                Text = "100%",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            // Инициализация инвентаря
            inventorySlots = new PictureBox[InventorySlots];
            for (int i = 0; i < InventorySlots; i++)
            {
                inventorySlots[i] = new InventorySlot();
            }

            // Кнопка выхода
            exitButton = new Button
            {
                Text = "Выход",
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void SetupLayout()
        {
            
        }

        private void SetupEvents()
        {
            exitButton.Click += (sender, e) =>
            {
                var result = MessageBox.Show(
                    "Выйти в главное меню?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            };
        }

        public void UpdateHealth(int health)
        {
            healthBar.Value = (health < 0) ? 0 : (health > 100) ? 100 : health;
            healthLabel.Text = $"{healthBar.Value}%";
            healthBar.ForeColor = healthBar.Value < 30 ? Color.DarkRed : Color.Red;
        }

        public void UpdateInventory(Image[] items)
        {
            for (int i = 0; i < Math.Min(items.Length, InventorySlots); i++)
            {
                inventorySlots[i].Image = items[i];
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetupLayout(); // Пересчитываем расположение при изменении размера
        }

        public class InventorySlot : PictureBox
        {
            public InventorySlot()
            {
                Size = new Size(50, 50);
                BackColor = Color.FromArgb(70, 70, 70);
                BorderStyle = BorderStyle.FixedSingle;
                SizeMode = PictureBoxSizeMode.StretchImage;

                // Эффекты при наведении
                this.MouseEnter += (s, e) => BackColor = Color.FromArgb(100, 100, 100);
                this.MouseLeave += (s, e) => BackColor = Color.FromArgb(70, 70, 70);
            }
        }
    }
}