using System;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : Form
{
    private TextBox timeTextBox;
    private Button startButton;
    private Label instructionLabel;
    private System.Windows.Forms.Timer timer;
    private DateTime targetTime;
    private Random random;

    public Form1()
    {
        InitializeComponents();
        random = new Random();
    }

    private void InitializeComponents()
    {
        this.Text = "Time Color Changer";
        this.ClientSize = new Size(800, 400);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        instructionLabel = new Label();
        instructionLabel.Text = "Enter target time (HH:MM:SS):";
        instructionLabel.Location = new Point(20, 30);
        instructionLabel.AutoSize = true;
        this.Controls.Add(instructionLabel);

        timeTextBox = new TextBox();
        timeTextBox.Location = new Point(20, 60);
        timeTextBox.Size = new Size(200, 20);
        this.Controls.Add(timeTextBox);

        startButton = new Button();
        startButton.Text = "Start Alarm";
        startButton.Location = new Point(20, 90);
        startButton.Click += StartButton_Click;
        this.Controls.Add(startButton);

        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000; // 1 second
        timer.Tick += Timer_Tick;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        if (DateTime.TryParseExact(timeTextBox.Text, "HH:mm:ss", 
            System.Globalization.CultureInfo.InvariantCulture, 
            System.Globalization.DateTimeStyles.None, out targetTime))
        {
            // Set the alarm time to today's date with the specified time
            targetTime = DateTime.Today.Add(targetTime.TimeOfDay);

            // If the alarm time has already passed today, set it for tomorrow
            if (targetTime < DateTime.Now)
            {
                targetTime = targetTime.AddDays(1);
            }

            startButton.Enabled = false;
            timeTextBox.Enabled = false;
            timer.Start();
        }
        else
        {
            MessageBox.Show("Invalid time format. Please use HH:MM:SS format.",
                          "Invalid Input",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
        }
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        this.BackColor = GetRandomColor();
        if (DateTime.Now >= targetTime)
        {
            timer.Stop();
            this.BackColor = SystemColors.Control;
            MessageBox.Show("Target time has been reached!",
                          "Time Reached",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
            startButton.Enabled = true;
            timeTextBox.Enabled = true;
        }
    }

    private Color GetRandomColor()
    {
        return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }
}
