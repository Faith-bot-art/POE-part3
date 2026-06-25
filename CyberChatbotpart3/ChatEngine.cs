using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberbotPart3
{
    public class ChatEngine
    {
        private Dictionary<string, List<string>> responses;
        private Dictionary<string, string> memory;
        private string currentTopic;
        private Random random;

        public ChatEngine()
        {
            random = new Random();
            currentTopic = "";
            memory = new Dictionary<string, string>();

            responses = new Dictionary<string, List<string>>()
            {
                ["password"] = new List<string>
                {
                    "Use strong passwords with 12+ characters including numbers, symbols, and uppercase letters.",
                    "Never reuse passwords across accounts. Use a password manager.",
                    "Enable Two-Factor Authentication (2FA) for extra security.",
                    "Avoid using personal info like birthdays or pet names in passwords."
                },
                ["scam"] = new List<string>
                {
                    "Never share personal information with unknown callers or emails.",
                    "If something sounds too good to be true, it probably is.",
                    "Always verify the sender before clicking on links.",
                    "Scammers create urgency - take a moment to think before acting."
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings on social media regularly.",
                    "Be careful what personal information you share online.",
                    "Use a VPN when on public Wi-Fi networks.",
                    "Check app permissions and revoke unnecessary access."
                },
                ["phishing"] = new List<string>
                {
                    "Check the sender's email address carefully.",
                    "Don't click on suspicious links.",
                    "Look for spelling mistakes and generic greetings.",
                    "Verify urgent requests through official channels."
                }
            };
        }

        public string GetResponse(string input)
        {
            string lower = input.ToLower();

            // Store name
            if (lower.Contains("my name is"))
            {
                try
                {
                    int idx = lower.IndexOf("my name is") + 10;
                    string name = input.Substring(idx).Trim().Split(' ')[0];
                    memory["name"] = name;
                    return "Hello " + name + "! How can I help you today?";
                }
                catch { }
            }

            // Store interest
            if (lower.Contains("interested in"))
            {
                try
                {
                    int idx = lower.IndexOf("interested in") + 13;
                    string topic = input.Substring(idx).Trim();
                    memory["interest"] = topic;
                    return "Great! I'll remember you're interested in " + topic + ".";
                }
                catch { }
            }

            // Sentiment detection
            string sentiment = "neutral";
            if (lower.Contains("worried") || lower.Contains("concerned") || lower.Contains("nervous"))
                sentiment = "worried";
            if (lower.Contains("frustrated") || lower.Contains("confused") || lower.Contains("annoyed"))
                sentiment = "frustrated";

            // Follow-up
            if (IsFollowUp(lower))
            {
                if (!string.IsNullOrEmpty(currentTopic) && responses.ContainsKey(currentTopic))
                {
                    return "Here's another tip: " + GetRandom(responses[currentTopic]);
                }
                return "Ask me about passwords, scams, privacy, or phishing.";
            }

            // Check keywords
            foreach (var kvp in responses)
            {
                if (lower.Contains(kvp.Key))
                {
                    currentTopic = kvp.Key;
                    string response = GetRandom(kvp.Value);

                    // Personalize
                    if (memory.ContainsKey("name"))
                        response = memory["name"] + ", " + response.ToLower();

                    // Sentiment prefix
                    if (sentiment == "worried")
                        return "I understand your concern. " + response;
                    if (sentiment == "frustrated")
                        return "I know it can be frustrating. " + response;

                    return response;
                }
            }

            // Greeting
            if (lower.Contains("hello") || lower.Contains("hi") || lower.Contains("hey"))
            {
                return memory.ContainsKey("name") ?
                    "Hello " + memory["name"] + "! Ask me about passwords, scams, or privacy." :
                    "Hello! Ask me about passwords, scams, or privacy.";
            }

            // Thanks
            if (lower.Contains("thank") || lower.Contains("thanks"))
            {
                return "You're welcome! Stay safe online!";
            }

            // Default
            return "I'm not sure I understand. Try asking about passwords, scams, privacy, or phishing.";
        }

        private bool IsFollowUp(string input)
        {
            string[] phrases = { "tell me more", "another tip", "more about", "explain more", "continue" };
            return phrases.Any(p => input.Contains(p));
        }

        private string GetRandom(List<string> list)
        {
            return list[random.Next(list.Count)];
        }

        public void ResetConversation()
        {
            currentTopic = "";
        }
    }
}
