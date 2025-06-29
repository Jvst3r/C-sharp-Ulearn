using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrisonEscapeGame.View.Forms
{
    public class AboutGameForm : Form
    {
        private Panel slidePanel;
        private Button prevButton;
        private Button nextButton;
        private Button closeButton;
        private Label titleLabel;
        private Label contentLabel;
        private PictureBox slideImage;

        private int currentSlide = 0;
        private readonly (string title, string content, string imagePath)[] slides =
        {
            ("Концепция игры",
             "Вы - заключенный, пытающийся сбежать из американской тюрьмы. Используйте различные методы для побега: прорыть тоннель, замаскироваться или перелезть через стену.",
             "Properties/Images/slide1.jpg"),
            ("Механики игры",
             "Ищите предметы, прячьтесь от охранников, собирайте информацию и выполняйте действия в нужные моменты времени.",
             "Properties/Images/slide2.jpg"),
            ("Управление",
             "WASD - движение\nE - взаимодействие с объектами\n1-5 - выбор предмета в инвентаре\nShift - бег\nCtrl - присесть",
             "Properties/Images/slide3.jpg")
        };

        public AboutGameForm()
        {
            InitializeComponents();
            SetupLayout();
            SetupEvents();
            ShowSlide(0);
        }

        private void InitializeComponents()
        {
            this.Text = "Об игре";
            this.ClientSize = new Size(600, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            slidePanel = new Panel
            {
                Size = new Size(560, 380),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            titleLabel = new Label
            {
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };

            contentLabel = new Label
            {
                Font = new Font("Arial", 10),
                ForeColor = Color.Black,
                MaximumSize = new Size(400, 200),
                AutoSize = true
            };

            slideImage = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(200, 200)
            };

            prevButton = new Button
            {
                Text = "< Назад",
                Size = new Size(80, 30),
                Location = new Point(150, 420)
            };

            nextButton = new Button
            {
                Text = "Вперед >",
                Size = new Size(80, 30),
                Location = new Point(350, 420)
            };

            closeButton = new Button
            {
                Text = "Закрыть",
                Size = new Size(100, 30),
                Location = new Point(250, 460)
            };
        }

        private void SetupLayout()
        {
            this.Controls.Add(slidePanel);
            this.Controls.Add(prevButton);
            this.Controls.Add(nextButton);
            this.Controls.Add(closeButton);

            slidePanel.Controls.Add(titleLabel);
            slidePanel.Controls.Add(contentLabel);
            slidePanel.Controls.Add(slideImage);

            titleLabel.Location = new Point(20, 20);
            contentLabel.Location = new Point(20, 60);
            slideImage.Location = new Point(420, 90);
        }

        private void SetupEvents()
        {
            prevButton.Click += (sender, e) => ShowSlide(currentSlide - 1);
            nextButton.Click += (sender, e) => ShowSlide(currentSlide + 1);
            closeButton.Click += (sender, e) => this.Close();
        }

        private void ShowSlide(int index)
        {
            if (index < 0 || index >= slides.Length)
                return;

            currentSlide = index;
            var slide = slides[index];

            titleLabel.Text = slide.title;
            contentLabel.Text = slide.content;

            if (!string.IsNullOrEmpty(slide.imagePath))
            {
                try
                {
                    slideImage.Image = Image.FromFile(slide.imagePath);
                }
                catch
                {
                    slideImage.Image = null;
                }
            }

            prevButton.Enabled = index > 0;
            nextButton.Enabled = index < slides.Length - 1;
        }
    }
}