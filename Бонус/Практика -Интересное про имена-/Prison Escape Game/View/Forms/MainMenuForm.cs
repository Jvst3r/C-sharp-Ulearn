using Prison_Escape_Game.View.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace PrisonEscapeGame.View.Forms
{
    public class MainMenuForm : Form
    {
        private Button playButton;
        private Button aboutGameButton;
        private Button exitButton;
        private PictureBox backgroundImage;
        private Label gameName;
        public MainMenuForm()
        {
            InitializeComponents();
            SetupLayout();
            SetupEvents();
        }

        //инициализация компонентов формы (размера,фона, кнопок и названия)
        private void InitializeComponents()
        {
            // Настройка формы
            this.Text = "Prison Escape";
            this.ClientSize = new Size(800, 600); //задаем размер формы
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false; //отключаем развертывание окна, эта игруха на такое явно не бует рассчитана))
            this.StartPosition = FormStartPosition.CenterScreen; //окно "спавнится" по середине экрана
            
            //this.Icon = new Icon()
            // Фоновое изображение
            backgroundImage = new PictureBox();
            try
            {
                //в общем на данном этапе у меня какая то проблема с загрузкой фона в главное меню,
                //поэтому пока изпользую try-catch чтобы хотя бы запустить посмотреть что я своими кривыми
                //ручками накалякал :(
                backgroundImage.Image = Image.FromFile("/Properties/Resources/Images/MainMenuBackground.jpg");
           }
           catch
            {
               this.BackColor = Color.Thistle;
            }

            // Кнопка "Играть"
            playButton = new MenuButton
            {
                Text = "Играть",
                Size = new Size(200, 50),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            // Кнопка "Об игре"
            aboutGameButton = new MenuButton
            {
                Text = "Об игре",
                Size = new Size(200, 50),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            gameName = new Label
            {
                Text = "PRISON ESCAPE",
                Size = new Size(500, 80),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Popup,
                Font = new Font("Arial", 40, FontStyle.Bold)
            };

            // Кнопка "Выход"
            exitButton = new MenuButton
            {
                Text = "Выход",
                Size = new Size(200, 50),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
        }


        //размещает кнопки на форме и добавляем кнопки в контроллеры
        private void SetupLayout()
        {
            // Центрирование кнопок
            int centerX = (this.ClientSize.Width - playButton.Width) / 2;
            playButton.Location = new Point(centerX, 250);
            aboutGameButton.Location = new Point(centerX, 320);
            exitButton.Location = new Point(centerX, 390);
            gameName.Location = new Point(centerX/2+40, 100);
            this.Controls.Add(playButton);
            this.Controls.Add(aboutGameButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(gameName);
        }

        //подписываем на события
        private void SetupEvents()
        {
            playButton.Click += (sender, e) =>
            {
                var gameForm = new GameForm();
                gameForm.Show();
                this.Hide();
                gameForm.FormClosed += (s, args) => this.Show(); //когда выходим из игры - показываем главное меню
            };

            aboutGameButton.Click += (sender, e) =>
            {
                var aboutForm = new AboutGameForm();
                aboutForm.ShowDialog();
            };

            exitButton.Click += (sender, e) =>
            {
                var result = MessageBox.Show(
                    "Вы действительно хотите выйти из игры?",
                    "Подтверждение выхода",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainMenuForm
            // 
            this.ClientSize = new System.Drawing.Size(1101, 535);
            this.Name = "MainMenuForm";
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            this.ResumeLayout(false);

        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {

        }
    }

    // Класс для кнопок меню с общими свойствами для всех 
    public class MenuButton : Button
    {
        public MenuButton()
        {
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
            this.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 50);
        }
    }
}