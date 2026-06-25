using System;
using System.Collections.Generic;

namespace CyberbotPart3
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Options { get; set; }
        public string Explanation { get; set; }
        public string QuestionType { get; set; }

        public string GetOptionsText()
        {
            if (QuestionType == "TrueFalse") return "A) True\nB) False";
            string result = "";
            for (int i = 0; i < Options.Count; i++)
                result += string.Format("{0}) {1}\n", (char)('A' + i), Options[i]);
            return result;
        }
    }

    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentIndex;
        private int score;
        private bool quizActive;
        private Random random;

        public QuizManager()
        {
            random = new Random();
            LoadQuestions();
            currentIndex = 0;
            score = 0;
            quizActive = false;
        }

        private void LoadQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    CorrectAnswer = "C",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    Explanation = "Reporting phishing emails helps prevent scams and protects others.",
                    QuestionType = "MCQ"
                },
                new QuizQuestion
                {
                    QuestionText = "A strong password should be at least how many characters long?",
                    CorrectAnswer = "B",
                    Options = new List<string> { "6 characters", "12 characters", "8 characters", "4 characters" },
                    Explanation = "Security experts recommend passwords of at least 12 characters.",
                    QuestionType = "MCQ"
                },
                new QuizQuestion
                {
                    QuestionText = "Two-Factor Authentication (2FA) adds an extra layer of security.",
                    CorrectAnswer = "A",
                    Options = new List<string> { "True", "False" },
                    Explanation = "2FA requires a second verification step, making accounts harder to hack.",
                    QuestionType = "TrueFalse"
                },
                new QuizQuestion
                {
                    QuestionText = "What is phishing?",
                    CorrectAnswer = "C",
                    Options = new List<string> { "A computer virus", "A fishing technique", "A scam to steal information", "A security software" },
                    Explanation = "Phishing is when scammers pretend to be legitimate companies to steal info.",
                    QuestionType = "MCQ"
                },
                new QuizQuestion
                {
                    QuestionText = "It is safe to use the same password for multiple accounts.",
                    CorrectAnswer = "B",
                    Options = new List<string> { "True", "False" },
                    Explanation = "Never reuse passwords across accounts. If one is compromised, all are at risk.",
                    QuestionType = "TrueFalse"
                },
                new QuizQuestion
                {
                    QuestionText = "What does VPN stand for?",
                    CorrectAnswer = "B",
                    Options = new List<string> { "Virtual Public Network", "Virtual Private Network", "Verified Private Network", "Virtual Protected Network" },
                    Explanation = "A VPN encrypts your internet traffic and hides your IP address.",
                    QuestionType = "MCQ"
                },
                new QuizQuestion
                {
                    QuestionText = "Public Wi-Fi is safe for online banking.",
                    CorrectAnswer = "B",
                    Options = new List<string> { "True", "False" },
                    Explanation = "Public Wi-Fi is often unencrypted and can be intercepted by hackers.",
                    QuestionType = "TrueFalse"
                },
                new QuizQuestion
                {
                    QuestionText = "What is social engineering?",
                    CorrectAnswer = "A",
                    Options = new List<string> { "Manipulating people for information", "Building social media", "Engineering platforms", "Computer hardware" },
                    Explanation = "Social engineering tricks people into revealing confidential information.",
                    QuestionType = "MCQ"
                },
                new QuizQuestion
                {
                    QuestionText = "You should only download software from official sources.",
                    CorrectAnswer = "A",
                    Options = new List<string> { "True", "False" },
                    Explanation = "Untrusted sources can contain malware and viruses.",
                    QuestionType = "TrueFalse"
                },
                new QuizQuestion
                {
                    QuestionText = "What does HTTPS indicate on a website?",
                    CorrectAnswer = "A",
                    Options = new List<string> { "Secure encrypted connection", "High traffic server", "Hacker protected", "Hosted private server" },
                    Explanation = "HTTPS means the connection between your browser and the website is encrypted.",
                    QuestionType = "MCQ"
                },
                new QuizQuestion
                {
                    QuestionText = "Pop-up ads claiming you won a prize are safe to click.",
                    CorrectAnswer = "B",
                    Options = new List<string> { "True", "False" },
                    Explanation = "These are usually scams designed to steal your information or install malware.",
                    QuestionType = "TrueFalse"
                },
                new QuizQuestion
                {
                    QuestionText = "What is the best way to protect against ransomware?",
                    CorrectAnswer = "D",
                    Options = new List<string> { "Pay the ransom", "Ignore updates", "Click pop-ups", "Regular backups and updates" },
                    Explanation = "Regular backups protect your data, and updates fix security vulnerabilities.",
                    QuestionType = "MCQ"
                }
            };
        }

        public void StartQuiz()
        {
            currentIndex = 0;
            score = 0;
            quizActive = true;
            // Shuffle
            for (int i = 0; i < questions.Count - 1; i++)
            {
                int j = random.Next(i, questions.Count);
                var temp = questions[i];
                questions[i] = questions[j];
                questions[j] = temp;
            }
        }

        public bool IsQuizActive() => quizActive && currentIndex < questions.Count;

        public QuizQuestion GetCurrentQuestion() => currentIndex < questions.Count ? questions[currentIndex] : null;

        public bool CheckAnswer(string userAnswer)
        {
            if (!quizActive || currentIndex >= questions.Count) return false;

            string normalized = userAnswer.Trim().ToUpper();
            if (normalized == "TRUE") normalized = "A";
            if (normalized == "FALSE") normalized = "B";

            bool correct = normalized == questions[currentIndex].CorrectAnswer;
            if (correct) score++;
            currentIndex++;

            if (currentIndex >= questions.Count) quizActive = false;

            return correct;
        }

        public int GetScore() => score;
        public int GetTotalQuestions() => questions.Count;
    }
}
