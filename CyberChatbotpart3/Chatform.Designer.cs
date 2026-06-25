using System;
using System.Collections.Generic;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Cyberbot_Part3
{
    partial class Chatform
    {
        private System.ComponentModel.IContainer components = null;

        // Control declarations
        private RichTextBox rtxtDisplay;
        private TextBox txtInput;
        private Button btnSend;
        private Button btnClear;
        private Button btnSpeak;
        private Button btnQuiz;
        private Button btnTasks;
        private Button btnLog;
        private Label lblStatus;
        private Label lblTitle;
        private Label lblSub;

        // Chatbot Engine
        private Random random;
        private Dictionary<string, List<string>> responses;
        private Dictionary<string, string> userMemory;
        private string currentTopic;

        // Task Manager (In-Memory)
        private List<TaskItem> tasks;
        private int taskCounter;

        // Quiz Manager
        private List<QuizQuestion> quizQuestions;
        private int currentQuestionIndex;
        private int quizScore;
        private bool quizActive;

        // Activity Log
        private List<string> activityLog;

        // Voice
        private SpeechSynthesizer speechSynthesizer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (speechSynthesizer != null)
            {
                speechSynthesizer.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ============ FORM PROPERTIES ============
            this.Text = "Cyberbot - Cybersecurity Assistant";
            this.Size = new Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 25, 45);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // ============ ASCII ART TITLE ============
            this.lblTitle = new Label();
            this.lblTitle.Text = @"
   ██████╗██╗   ██╗██████╗ ███████╗██████╗  ██████╗ ████████╗
  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝
  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██║   ██║   ██║   
  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██║   ██║   ██║   
  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║╚██████╔╝   ██║   
   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝ ╚═════╝    ╚═╝   ";
            this.lblTitle.ForeColor = Color.FromArgb(0, 255, 255);
            this.lblTitle.Font = new Font("Consolas", 9, FontStyle.Bold);
            this.lblTitle.Location = new Point(150, 10);
            this.lblTitle.Size = new Size(700, 140);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // ============ SUBTITLE ============
            this.lblSub = new Label();
            this.lblSub.Text = "Your Personal Cybersecurity Assistant";
            this.lblSub.ForeColor = Color.LightGray;
            this.lblSub.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblSub.Location = new Point(300, 150);
            this.lblSub.Size = new Size(400, 25);
            this.lblSub.TextAlign = ContentAlignment.MiddleCenter;

            // ============ CHAT DISPLAY ============
            this.rtxtDisplay = new RichTextBox();
            this.rtxtDisplay.Location = new Point(20, 190);
            this.rtxtDisplay.Size = new Size(730, 380);
            this.rtxtDisplay.BackColor = Color.FromArgb(20, 25, 40);
            this.rtxtDisplay.ForeColor = Color.White;
            this.rtxtDisplay.Font = new Font("Segoe UI", 10);
            this.rtxtDisplay.ReadOnly = true;
            this.rtxtDisplay.BorderStyle = BorderStyle.FixedSingle;

            // ============ SIDE BUTTONS ============
            // Quiz Button
            this.btnQuiz = new Button();
            this.btnQuiz.Text = "QUIZ";
            this.btnQuiz.Location = new Point(770, 190);
            this.btnQuiz.Size = new Size(110, 40);
            this.btnQuiz.BackColor = Color.FromArgb(255, 140, 0);
            this.btnQuiz.ForeColor = Color.White;
            this.btnQuiz.FlatStyle = FlatStyle.Flat;
            this.btnQuiz.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnQuiz.Click += new EventHandler(btnQuiz_Click);

            // Tasks Button
            this.btnTasks = new Button();
            this.btnTasks.Text = "TASKS";
            this.btnTasks.Location = new Point(770, 240);
            this.btnTasks.Size = new Size(110, 40);
            this.btnTasks.BackColor = Color.FromArgb(0, 150, 100);
            this.btnTasks.ForeColor = Color.White;
            this.btnTasks.FlatStyle = FlatStyle.Flat;
            this.btnTasks.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnTasks.Click += new EventHandler(btnTasks_Click);

            // Activity Log Button
            this.btnLog = new Button();
            this.btnLog.Text = "ACTIVITY";
            this.btnLog.Location = new Point(770, 290);
            this.btnLog.Size = new Size(110, 40);
            this.btnLog.BackColor = Color.FromArgb(100, 100, 200);
            this.btnLog.ForeColor = Color.White;
            this.btnLog.FlatStyle = FlatStyle.Flat;
            this.btnLog.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnLog.Click += new EventHandler(btnLog_Click);

            // Speak Button
            this.btnSpeak = new Button();
            this.btnSpeak.Text = "SPEAK";
            this.btnSpeak.Location = new Point(770, 340);
            this.btnSpeak.Size = new Size(110, 40);
            this.btnSpeak.BackColor = Color.FromArgb(80, 140, 80);
            this.btnSpeak.ForeColor = Color.White;
            this.btnSpeak.FlatStyle = FlatStyle.Flat;
            this.btnSpeak.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnSpeak.Click += new EventHandler(btnSpeak_Click);

            // ============ USER INPUT ============
            this.txtInput = new TextBox();
            this.txtInput.Location = new Point(20, 585);
            this.txtInput.Size = new Size(620, 27);
            this.txtInput.BackColor = Color.FromArgb(40, 45, 60);
            this.txtInput.ForeColor = Color.White;
            this.txtInput.BorderStyle = BorderStyle.FixedSingle;
            this.txtInput.Font = new Font("Segoe UI", 10);
            this.txtInput.KeyPress += new KeyPressEventHandler(txtInput_KeyPress);

            // ============ SEND BUTTON ============
            this.btnSend = new Button();
            this.btnSend.Text = "SEND";
            this.btnSend.Location = new Point(650, 583);
            this.btnSend.Size = new Size(100, 32);
            this.btnSend.BackColor = Color.FromArgb(0, 120, 215);
            this.btnSend.ForeColor = Color.White;
            this.btnSend.FlatStyle = FlatStyle.Flat;
            this.btnSend.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnSend.Click += new EventHandler(btnSend_Click);

            // ============ CLEAR BUTTON ============
            this.btnClear = new Button();
            this.btnClear.Text = "CLEAR";
            this.btnClear.Location = new Point(760, 583);
            this.btnClear.Size = new Size(110, 32);
            this.btnClear.BackColor = Color.FromArgb(200, 80, 80);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnClear.Click += new EventHandler(btnClear_Click);

            // ============ STATUS LABEL ============
            this.lblStatus = new Label();
            this.lblStatus.Text = "Ready - Ask me about passwords, scams, privacy, or phishing";
            this.lblStatus.ForeColor = Color.LightGreen;
            this.lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.lblStatus.Location = new Point(20, 625);
            this.lblStatus.Size = new Size(850, 25);

            // ============ ADD CONTROLS ============
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSub);
            this.Controls.Add(this.rtxtDisplay);
            this.Controls.Add(this.btnQuiz);
            this.Controls.Add(this.btnTasks);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnSpeak);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblStatus);

            // ============ INITIALIZE CHATBOT ============
            InitializeChatbot();
            AddWelcomeMessage();
            LogActivity("Application started", "Cyberbot initialized");
        }

        // ============ INITIALIZATION ============
        private void InitializeChatbot()
        {
            random = new Random();
            currentTopic = "";
            userMemory = new Dictionary<string, string>();
            tasks = new List<TaskItem>();
            activityLog = new List<string>();
            taskCounter = 0;
            quizActive = false;
            currentQuestionIndex = 0;
            quizScore = 0;

            // Initialize Voice
            try
            {
                speechSynthesizer = new SpeechSynthesizer();
            }
            catch { }

            InitializeResponses();
            InitializeQuiz();
        }

        private void InitializeResponses()
        {
            responses = new Dictionary<string, List<string>>()
            {
                ["password"] = new List<string>
                {
                    "[PASSWORD SECURITY] Use strong passwords with at least 12 characters including numbers, symbols, and capital letters.",
                    "[PASSWORD SECURITY] Never reuse passwords across different accounts. Use a password manager.",
                    "[PASSWORD SECURITY] Enable Two-Factor Authentication (2FA) whenever possible."
                },
                ["scam"] = new List<string>
                {
                    "[SCAM ALERT] Never share personal information with unknown callers or emails.",
                    "[SCAM ALERT] If something sounds too good to be true, it probably is.",
                    "[SCAM ALERT] Always verify the sender before clicking on links."
                },
                ["privacy"] = new List<string>
                {
                    "[PRIVACY PROTECTION] Review your privacy settings on social media regularly.",
                    "[PRIVACY PROTECTION] Be careful what personal information you share online.",
                    "[PRIVACY PROTECTION] Use a VPN when on public Wi-Fi networks."
                },
                ["phishing"] = new List<string>
                {
                    "[PHISHING WARNING] Check the sender's email address carefully.",
                    "[PHISHING WARNING] Don't click on suspicious links.",
                    "[PHISHING WARNING] Look for spelling mistakes and generic greetings."
                }
            };
        }

        private void InitializeQuiz()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    CorrectAnswer = "C",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    Explanation = "Reporting phishing emails helps prevent scams."
                },
                new QuizQuestion
                {
                    QuestionText = "A strong password should be at least how many characters long?",
                    CorrectAnswer = "B",
                    Options = new List<string> { "6 characters", "12 characters", "8 characters", "4 characters" },
                    Explanation = "Security experts recommend passwords of at least 12 characters."
                },
                new QuizQuestion
                {
                    QuestionText = "Two-Factor Authentication (2FA) adds an extra layer of security.",
                    CorrectAnswer = "A",
                    Options = new List<string> { "True", "False" },
                    Explanation = "2FA requires a second verification step."
                },
                new QuizQuestion
                {
                    QuestionText = "What is phishing?",
                    CorrectAnswer = "C",
                    Options = new List<string> { "A computer virus", "A fishing technique", "A scam to steal information", "A security software" },
                    Explanation = "Phishing is when scammers pretend to be legitimate companies."
                },
                new QuizQuestion
                {
                    QuestionText = "It is safe to use the same password for multiple accounts.",
                    CorrectAnswer = "B",
                    Options = new List<string> { "True", "False" },
                    Explanation = "Never reuse passwords across accounts."
                },
                new QuizQuestion
                {
                    QuestionText = "What does VPN stand for?",
                    CorrectAnswer = "B",
                    Options = new List<string> { "Virtual Public Network", "Virtual Private Network", "Verified Private Network", "Virtual Protected Network" },
                    Explanation = "A VPN encrypts your internet traffic."
                },
                new QuizQuestion
                {
                    QuestionText = "Public Wi-Fi is safe for online banking.",
                    CorrectAnswer = "B",
                    Options = new List<string> { "True", "False" },
                    Explanation = "Public Wi-Fi is often unencrypted."
                },
                new QuizQuestion
                {
                    QuestionText = "What is social engineering?",
                    CorrectAnswer = "A",
                    Options = new List<string> { "Manipulating people for information", "Building social media", "Engineering platforms", "Computer hardware" },
                    Explanation = "Social engineering tricks people into revealing information."
                },
                new QuizQuestion
                {
                    QuestionText = "You should only download software from official sources.",
                    CorrectAnswer = "A",
                    Options = new List<string> { "True", "False" },
                    Explanation = "Untrusted sources can contain malware."
                },
                new QuizQuestion
                {
                    QuestionText = "What does HTTPS indicate on a website?",
                    CorrectAnswer = "A",
                    Options = new List<string> { "Secure encrypted connection", "High traffic server", "Hacker protected", "Hosted private server" },
                    Explanation = "HTTPS means the connection is encrypted."
                },
                new QuizQuestion
                {
                    QuestionText = "Pop-up ads claiming you won a prize are safe to click.",
                    CorrectAnswer = "B",
                    Options = new List<string> { "True", "False" },
                    Explanation = "These are usually scams."
                },
                new QuizQuestion
                {
                    QuestionText = "What is the best way to protect against ransomware?",
                    CorrectAnswer = "D",
                    Options = new List<string> { "Pay the ransom", "Ignore updates", "Click pop-ups", "Regular backups and updates" },
                    Explanation = "Regular backups protect your data."
                }
            };
        }

        // ============ LOGGING ============
        private void LogActivity(string action, string details)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss}] {action}: {details}";
            activityLog.Insert(0, entry);
            if (activityLog.Count > 50)
                activityLog.RemoveAt(activityLog.Count - 1);
        }

        // ============ WELCOME MESSAGE ============
        private void AddWelcomeMessage()
        {
            rtxtDisplay.SelectionColor = Color.FromArgb(100, 255, 100);
            rtxtDisplay.AppendText("============================================================================\n");
            rtxtDisplay.AppendText("                         WELCOME TO CYBERBOT                                   \n");
            rtxtDisplay.AppendText("                    Your Cybersecurity Assistant                               \n");
            rtxtDisplay.AppendText("============================================================================\n\n");
            rtxtDisplay.SelectionColor = Color.White;
            rtxtDisplay.AppendText("I can help you with:\n");
            rtxtDisplay.AppendText("  - Password Security\n");
            rtxtDisplay.AppendText("  - Scam Detection\n");
            rtxtDisplay.AppendText("  - Privacy Protection\n");
            rtxtDisplay.AppendText("  - Phishing Awareness\n\n");
            rtxtDisplay.AppendText("Try asking:\n");
            rtxtDisplay.AppendText("  - 'Tell me about passwords'\n");
            rtxtDisplay.AppendText("  - 'How to spot a scam?'\n");
            rtxtDisplay.AppendText("  - 'Privacy tips please'\n");
            rtxtDisplay.AppendText("  - 'Give me a phishing tip'\n\n");
            rtxtDisplay.SelectionColor = Color.FromArgb(100, 255, 100);
            rtxtDisplay.AppendText("============================================================================\n");
            rtxtDisplay.AppendText("  How can I help you stay safe online today?\n");
            rtxtDisplay.AppendText("============================================================================\n\n");
            rtxtDisplay.ScrollToCaret();
        }

        private void AppendToChat(string sender, string message, Color color)
        {
            rtxtDisplay.SelectionStart = rtxtDisplay.TextLength;
            rtxtDisplay.SelectionLength = 0;
            rtxtDisplay.SelectionColor = color;
            rtxtDisplay.AppendText($"[{sender}] ");
            rtxtDisplay.SelectionColor = Color.White;
            rtxtDisplay.AppendText(message + "\n\n");
            rtxtDisplay.ScrollToCaret();
        }

        // ============ SPEAK METHOD ============
        private void SpeakText(string text)
        {
            try
            {
                if (speechSynthesizer != null)
                {
                    speechSynthesizer.SpeakAsync(text);
                }
            }
            catch (Exception)
            {
                // Voice not available - silently ignore
            }
        }

        // ============ EVENT HANDLERS ============
        private void btnSend_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AppendToChat("YOU", input, Color.Yellow);
            txtInput.Clear();

            string response = ProcessIntent(input);
            AppendToChat("CYBERBOT", response, Color.Cyan);

            // Speak the response
            SpeakText(response);

            LogActivity("Question asked", input.Length > 50 ? input.Substring(0, 50) + "..." : input);
            lblStatus.Text = "Ready - Ask me anything about cybersecurity!";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtxtDisplay.Clear();
            AddWelcomeMessage();
            LogActivity("Chat cleared", "User cleared the chat history");
        }

        private void btnSpeak_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                SpeakText(input);
            }
        }

        private void btnQuiz_Click(object sender, EventArgs e)
        {
            StartQuiz();
        }

        private void btnTasks_Click(object sender, EventArgs e)
        {
            ShowTasks();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            ShowActivityLog();
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSend_Click(sender, e);
                e.Handled = true;
            }
        }

        // ============ QUIZ METHODS ============
        private void StartQuiz()
        {
            // Shuffle questions
            for (int i = 0; i < quizQuestions.Count - 1; i++)
            {
                int j = random.Next(i, quizQuestions.Count);
                var temp = quizQuestions[i];
                quizQuestions[i] = quizQuestions[j];
                quizQuestions[j] = temp;
            }

            currentQuestionIndex = 0;
            quizScore = 0;
            quizActive = true;
            LogActivity("Quiz started", "User began the cybersecurity quiz");
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex < quizQuestions.Count)
            {
                var q = quizQuestions[currentQuestionIndex];
                AppendToChat("QUIZ", q.QuestionText, Color.Orange);
                AppendToChat("QUIZ", q.GetOptionsText(), Color.LightGray);
                AppendToChat("QUIZ", "Type your answer (A, B, C, D, or True/False)", Color.LightGray);
            }
            else
            {
                quizActive = false;
                string feedback = quizScore >= 7 ? "Great job! You're a cybersecurity pro!" : "Keep learning to stay safe online!";
                AppendToChat("QUIZ", $"Quiz completed! Score: {quizScore}/{quizQuestions.Count}. {feedback}", Color.Green);
                LogActivity("Quiz completed", $"Score: {quizScore}/{quizQuestions.Count}");
            }
        }

        private bool CheckQuizAnswer(string input)
        {
            if (!quizActive || currentQuestionIndex >= quizQuestions.Count)
                return false;

            string normalized = input.Trim().ToUpper();
            if (normalized == "TRUE") normalized = "A";
            if (normalized == "FALSE") normalized = "B";

            bool correct = normalized == quizQuestions[currentQuestionIndex].CorrectAnswer;
            if (correct) quizScore++;

            LogActivity("Quiz answer", correct ? "Correct" : "Incorrect");
            currentQuestionIndex++;
            return correct;
        }

        // ============ TASK METHODS ============
        private void AddTask(string title)
        {
            taskCounter++;
            var task = new TaskItem
            {
                Id = taskCounter,
                Title = title,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
            tasks.Add(task);
            LogActivity("Task added", title);
        }

        private void ShowTasks()
        {
            if (tasks.Count == 0)
            {
                AppendToChat("TASKS", "You have no tasks. Add one: 'Add task - [description]'", Color.LightGray);
                return;
            }

            AppendToChat("TASKS", "========== YOUR TASKS ==========", Color.Green);
            for (int i = 0; i < tasks.Count; i++)
            {
                string status = tasks[i].IsCompleted ? "[COMPLETED]" : "[PENDING]";
                AppendToChat("TASKS", $"{i + 1}. {status} - {tasks[i].Title}", Color.LightGray);
            }
            AppendToChat("TASKS", "Type 'Complete task [number]' to mark a task as done.", Color.LightGray);
        }

        private void CompleteTask(int index)
        {
            if (index > 0 && index <= tasks.Count)
            {
                tasks[index - 1].IsCompleted = true;
                LogActivity("Task completed", tasks[index - 1].Title);
                AppendToChat("TASKS", $"Task '{tasks[index - 1].Title}' marked as completed!", Color.Green);
            }
            else
            {
                AppendToChat("TASKS", "Invalid task number. Use 'Show tasks' to see your tasks.", Color.Red);
            }
        }

        // ============ ACTIVITY LOG ============
        private void ShowActivityLog()
        {
            int count = Math.Min(10, activityLog.Count);
            AppendToChat("ACTIVITY", "========== RECENT ACTIVITIES ==========", Color.Green);
            for (int i = 0; i < count; i++)
            {
                AppendToChat("LOG", $"{i + 1}. {activityLog[i]}", Color.LightGray);
            }
        }

        // ============ INTENT PROCESSING ============
        private string ProcessIntent(string input)
        {
            string lower = input.ToLower();

            // ===== QUIZ HANDLING =====
            if (lower.Contains("quiz") || lower.Contains("start quiz") || lower.Contains("take quiz"))
            {
                StartQuiz();
                return "Starting the cybersecurity quiz! Let's test your knowledge.";
            }

            if (quizActive)
            {
                bool correct = CheckQuizAnswer(input);
                var q = quizQuestions[currentQuestionIndex - 1];
                string result = correct ? "Correct! " : "Incorrect. ";
                ShowQuestion();
                return result + q.Explanation;
            }

            // ===== TASK HANDLING =====
            if (lower.Contains("add task") || lower.Contains("add -"))
            {
                string desc = ExtractTaskDescription(input);
                if (!string.IsNullOrEmpty(desc))
                {
                    AddTask(desc);
                    return $"Task added: '{desc}'. Type 'Show tasks' to see your tasks.";
                }
                return "Please specify a task. Example: 'Add task - Review privacy settings'";
            }

            if (lower.Contains("show tasks") || lower.Contains("my tasks") || lower.Contains("list tasks"))
            {
                ShowTasks();
                return "Tasks displayed above.";
            }

            if (lower.Contains("complete task") || lower.Contains("complete"))
            {
                int taskNum = ExtractTaskNumber(input);
                CompleteTask(taskNum);
                return "Task updated.";
            }

            // ===== ACTIVITY LOG =====
            if (lower.Contains("activity log") || lower.Contains("what have you done") || lower.Contains("show log"))
            {
                ShowActivityLog();
                return "Activity log displayed above.";
            }

            // ===== MEMORY: STORE NAME =====
            if (lower.Contains("my name is") || lower.Contains("call me"))
            {
                try
                {
                    string name = "";
                    if (lower.Contains("my name is"))
                    {
                        int idx = lower.IndexOf("my name is") + 10;
                        name = input.Substring(idx).Trim().Split(' ')[0];
                    }
                    else
                    {
                        int idx = lower.IndexOf("call me") + 7;
                        name = input.Substring(idx).Trim().Split(' ')[0];
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        userMemory["name"] = name;
                        LogActivity("Name stored", name);
                        return $"Nice to meet you, {name}! I'll remember your name.";
                    }
                }
                catch { }
            }

            // ===== MEMORY: STORE INTEREST =====
            if (lower.Contains("interested in") || lower.Contains("like to learn about"))
            {
                try
                {
                    int idx = lower.Contains("interested in") ? lower.IndexOf("interested in") + 13 : lower.IndexOf("like to learn about") + 18;
                    string topic = input.Substring(idx).Trim();
                    if (!string.IsNullOrEmpty(topic))
                    {
                        userMemory["interest"] = topic;
                        LogActivity("Interest stored", topic);
                        return $"Great! I'll remember you're interested in {topic}.";
                    }
                }
                catch { }
            }

            // ===== FOLLOW-UP QUESTIONS =====
            if (IsFollowUp(lower))
            {
                if (!string.IsNullOrEmpty(currentTopic) && responses.ContainsKey(currentTopic))
                {
                    return "Here's another tip: " + GetRandomResponse(responses[currentTopic]);
                }
                return "What topic would you like to know more about?";
            }

            // ===== SENTIMENT DETECTION =====
            string sentiment = "neutral";
            if (lower.Contains("worried") || lower.Contains("concerned") || lower.Contains("nervous") || lower.Contains("scared"))
                sentiment = "worried";
            if (lower.Contains("frustrated") || lower.Contains("confused") || lower.Contains("annoyed") || lower.Contains("angry"))
                sentiment = "frustrated";
            if (lower.Contains("curious") || lower.Contains("interested") || lower.Contains("wondering"))
                sentiment = "curious";

            // ===== KEYWORD RECOGNITION =====
            foreach (var kvp in responses)
            {
                if (lower.Contains(kvp.Key))
                {
                    currentTopic = kvp.Key;
                    string response = GetRandomResponse(kvp.Value);

                    if (userMemory.ContainsKey("name"))
                        response = userMemory["name"] + ", " + response.ToLower();

                    if (sentiment == "worried")
                        return "I understand your concern. " + response;
                    if (sentiment == "frustrated")
                        return "I know it can be frustrating. " + response;
                    if (sentiment == "curious")
                        return "That's a great question! " + response;

                    return response;
                }
            }

            // ===== GREETINGS =====
            if (lower.Contains("hello") || lower.Contains("hi") || lower.Contains("hey"))
            {
                string name = userMemory.ContainsKey("name") ? userMemory["name"] : "";
                return string.IsNullOrEmpty(name) ?
                    "Hello! I'm Cyberbot. Ask me about passwords, scams, privacy, or phishing." :
                    $"Hello, {name}! How can I help you today?";
            }

            // ===== THANKS =====
            if (lower.Contains("thank") || lower.Contains("thanks"))
            {
                return "You're welcome! Stay safe online!";
            }

            // ===== HELP =====
            if (lower.Contains("help") || lower.Contains("what can you do"))
            {
                return @"Here's what I can help you with:

Cybersecurity Tips:
  - 'Tell me about passwords'
  - 'How to spot a scam?'
  - 'Privacy tips please'
  - 'Give me a phishing tip'

Tasks:
  - 'Add task - Review privacy settings'
  - 'Show tasks'
  - 'Complete task 1'

Quiz:
  - 'Start quiz'

Activity Log:
  - 'Show activity log'";
            }

            // ===== DEFAULT =====
            return GetDefaultResponse();
        }

        // ============ HELPER METHODS ============
        private bool IsFollowUp(string input)
        {
            string[] phrases = { "tell me more", "another tip", "more about", "explain more", "continue", "go on", "what else" };
            foreach (string p in phrases)
                if (input.Contains(p)) return true;
            return false;
        }

        private string GetRandomResponse(List<string> list)
        {
            return list[random.Next(list.Count)];
        }

        private string ExtractTaskDescription(string input)
        {
            int idx = input.ToLower().IndexOf("add task");
            if (idx >= 0)
            {
                string remaining = input.Substring(idx + 8).Trim();
                if (remaining.StartsWith("-")) remaining = remaining.Substring(1).Trim();
                if (remaining.StartsWith("to")) remaining = remaining.Substring(2).Trim();
                return remaining;
            }
            return "";
        }

        private int ExtractTaskNumber(string input)
        {
            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                if (int.TryParse(word, out int num))
                    return num;
            }
            return 0;
        }

        private string GetDefaultResponse()
        {
            string[] defaults = {
                "I'm not sure I understand. Try asking about passwords, scams, privacy, or phishing.",
                "Could you rephrase that? I can help with cybersecurity topics.",
                "Try asking: 'Tell me about password safety' or 'How to spot a scam?'"
            };
            return defaults[random.Next(defaults.Length)];
        }
    }

    // ============ TASK CLASS ============
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ============ QUIZ CLASS ============
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Options { get; set; }
        public string Explanation { get; set; }

        public string GetOptionsText()
        {
            if (Options.Count == 2 && Options[0] == "True" && Options[1] == "False")
                return "A) True\nB) False";

            string result = "";
            for (int i = 0; i < Options.Count; i++)
            {
                char letter = (char)('A' + i);
                result += $"{letter}) {Options[i]}\n";
            }
            return result;
        }
    }
}