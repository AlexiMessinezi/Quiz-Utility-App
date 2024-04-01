using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace testWPF
{
    /// <summary>
    /// Interaction logic for FindingCallNumbers.xaml
    /// </summary>

    public partial class FindingCallNumbers : Window
    {
        // Variable to use for randomizer
        Random rnd = new Random();
        // Nodes to store specific data
        Node nodeLevel0;
        Node nodeLevel1;
        Node nodeLevel2;
        Node nodeLevel3;
        // Variable for fixed array size
        static int questionOutputSize = 4;
        int questionLevel = 0;
        int score = 0;
        public FindingCallNumbers()
        {
            InitializeComponent();
            CenterTextOnComponents();
            LoadDeweyDataFromFile();
            CreateQuestion();

        }

        public void LoadDeweyDataFromFile()
        {
            // Read the Dewey Data from file
            string dataInput = File.ReadAllText("DeweyDecimal.txt");
            // Extract the parent node from the string
            string key0Node = dataInput.Substring(0, dataInput.IndexOf("\n"));
            key0Node = key0Node.Replace("[LEVEL0]", "");
            nodeLevel0 = NewNode(key0Node);
            // Remove the parent node from the string
            dataInput = dataInput.Remove(0, dataInput.IndexOf("\n") + 1);
            // Find the level 1 nodes
            string[] level1nodes = dataInput.Split(new[] { "[LEVEL1]" }, StringSplitOptions.None);
            // Remove any empty cells from the array
            level1nodes = level1nodes.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < level1nodes.Length; i++)
            {
                // Extract the level 1 node from the string
                string key1Node = level1nodes[i].Substring(0, level1nodes[i].IndexOf("\n"));
                key1Node = key1Node.Replace("[LEVEL1]", "");
                nodeLevel0.child.Add(NewNode(key1Node));
                // Remove the level 1 node from the string
                level1nodes[i] = level1nodes[i].Remove(0, level1nodes[i].IndexOf("\n") + 1);

                // Find the level 2 nodes
                string[] level2nodes = level1nodes[i].Split(new[] { "[LEVEL2]" }, StringSplitOptions.None);
                level2nodes = level2nodes.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                for (int j = 0; j < level2nodes.Length; j++)
                {
                    // Extract the level 2 node from the string
                    string key2Node = level2nodes[j].Substring(0, level2nodes[j].IndexOf("\n"));
                    key2Node = key2Node.Replace("[LEVEL2]", "");
                    nodeLevel0.child[i].child.Add(NewNode(key2Node));
                    // Remove the level 2 node from the string
                    level2nodes[j] = level2nodes[j].Remove(0, level2nodes[j].IndexOf("\n") + 1);
                    // Find the level 3 nodes
                    string[] level3nodes = level2nodes[j].Split(new[] { "[LEVEL3]" }, StringSplitOptions.None);
                    level3nodes = level3nodes.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int k = 0; k < level3nodes.Length; k++)
                    {
                        // Extract the level 3 node from the string
                        string key3Node = level3nodes[k].Substring(0, level3nodes[k].IndexOf("\n"));
                        key3Node = key3Node.Replace("[LEVEL3]", "");
                        nodeLevel0.child[i].child[j].child.Add(NewNode(key3Node));
                    }
                }
            }
        }

        public void CreateQuestion()
        {
            questionLevel = 0;
            SelectRandomCallNumber();
            ShowButtonOutputs();
            
        }

        public void SelectRandomCallNumber()
        {
            // Randomly select a level 1 category
            int deweyLevel1Categories = nodeLevel0.child.Count;
            int randomSelect = rnd.Next(deweyLevel1Categories);
            nodeLevel1 = nodeLevel0.child[randomSelect];

            // Randomly select a level 2 category
            int deweyLevel2Categories = nodeLevel1.child.Count;
            randomSelect = rnd.Next(deweyLevel2Categories);
            nodeLevel2 = nodeLevel1.child[randomSelect];

            // Randomly select a level 3 category
            int deweyLevel3Categories = nodeLevel2.child.Count;
            randomSelect = rnd.Next(deweyLevel3Categories);
            nodeLevel3 = nodeLevel2.child[randomSelect];

            string tempCallNum = nodeLevel3.key.ToString();
            tempCallNum = tempCallNum.Remove(0, 6);
            lblHint.Content = tempCallNum.ToUpper();
        }

        public void ShowButtonOutputs()
        {
            string[] outputs = new string[questionOutputSize];

            switch (questionLevel)
            {
                case 0:
                    outputs = SelectAnswers(nodeLevel1, nodeLevel0);
                    break;
                case 1:
                    outputs = SelectAnswers(nodeLevel2, nodeLevel1);
                    break;
                case 2:
                    outputs = SelectAnswers(nodeLevel3, nodeLevel2);
                    break;
                case 3:
                default:
                    // If we get here we need to generate a new question
                    break;
            }

            
            if (outputs[0] == null || outputs[1] == null || outputs[2] == null || outputs[3] == null) return;
            BubbleSort(outputs);
            btnOutput1.Content = outputs[0].ToUpper();
            btnOutput2.Content = outputs[1].ToUpper();
            btnOutput3.Content = outputs[2].ToUpper();
            btnOutput4.Content = outputs[3].ToUpper();
            
        }

        public string[] SelectAnswers(Node selectedNode, Node parentNode)
        {
            // Retrieve all parents nodes children keys
            string[] temp = new string[questionOutputSize];
            int numberChildren = parentNode.child.Count;
            string[] allChildren = new string[numberChildren];
            for (int i = 0; i < numberChildren; i++)
            {
                allChildren[i] = parentNode.child[i].key.ToString();
            }
            // Randomly shuffle array to ensure random answers
            allChildren = allChildren.OrderBy(x => rnd.Next()).ToArray();

            temp[0] = allChildren[0];
            temp[1] = allChildren[1];
            temp[2] = allChildren[2];
            temp[3] = allChildren[3];

            // If selected node isnt in the array add it to array
            if (!Array.Exists(temp, element => element == selectedNode.key.ToString())) temp[0] = selectedNode.key.ToString();

            return temp;
        }

        public int CheckAnswer(string answerText)
        {
            int result = 0;
            switch (questionLevel)
            {
                case 0:
                    if (answerText.ToUpper() == nodeLevel1.key.ToUpper().ToString())
                    {
                        score += 1;
                        UpdateScore();
                        result = 1;
                        questionLevel = 1;
                    }
                    break;
                case 1:
                    if (answerText.ToUpper() == nodeLevel2.key.ToUpper().ToString())
                    {
                        score += 5;
                        UpdateScore();
                        result = 1;
                        questionLevel = 2;
                    }
                    break;
                case 2:
                    if (answerText.ToUpper() == nodeLevel3.key.ToUpper().ToString())
                    {
                        score += 10;
                        UpdateScore();
                        result = 100;
                        questionLevel = 3;
                    }
                    break;
            }
            return result;
        }

        public void CenterTextOnComponents()
        {
            lblHintHeading.HorizontalContentAlignment = HorizontalAlignment.Center;
            lblHint.HorizontalContentAlignment = HorizontalAlignment.Center;
            btnOutput1.HorizontalContentAlignment = HorizontalAlignment.Center;
            btnOutput2.HorizontalContentAlignment = HorizontalAlignment.Center;
            btnOutput3.HorizontalContentAlignment = HorizontalAlignment.Center;
            btnOutput4.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        public class Node
        {
            public string key;
            public List<Node> child = new List<Node>();
        };

        // Utility function to create a new tree node
        static Node NewNode(string key)
        {
            Node temp = new Node();
            temp.key = key;
            return temp;
        }

        private async void BtnOutput1_Click(object sender, RoutedEventArgs e)
        {
            int result = CheckAnswer(btnOutput1.Content.ToString());
            if (result == 0)
            {
                btnOutput1.Background = Brushes.Red;
                DisableButtons();
                return;
            };
            btnOutput1.Background = Brushes.LightGreen;
            await Task.Delay(2000);
            ResetButtonColours();
            ShowButtonOutputs();
            if (result == 100) CreateQuestion();
        }

        private async void BtnOutput2_Click(object sender, RoutedEventArgs e)
        {
            int result = CheckAnswer(btnOutput2.Content.ToString());
            if (result == 0)
            {
                btnOutput2.Background = Brushes.Red;
                DisableButtons();
                return;
            };
            btnOutput2.Background = Brushes.LightGreen;
            await Task.Delay(2000);
            ResetButtonColours();
            ShowButtonOutputs();
            if (result == 100) CreateQuestion();
        }

        private async void BtnOutput3_Click(object sender, RoutedEventArgs e)
        {
            int result = CheckAnswer(btnOutput3.Content.ToString());
            if (result == 0)
            {
                btnOutput3.Background = Brushes.Red;
                DisableButtons();
                return;
            };
            btnOutput3.Background = Brushes.LightGreen;
            await Task.Delay(2000);
            ResetButtonColours();
            ShowButtonOutputs();
            if (result == 100) CreateQuestion();
        }

        private async void BtnOutput4_Click(object sender, RoutedEventArgs e)
        {
            int result = CheckAnswer(btnOutput4.Content.ToString());
            if (result == 0)
            {
                btnOutput4.Background = Brushes.Red;
                DisableButtons();
                return;
            };
            btnOutput4.Background = Brushes.LightGreen;
            await Task.Delay(2000);
            ResetButtonColours();
            ShowButtonOutputs();
            if (result == 100) CreateQuestion();
        }

        public void ResetButtonColours()
        {
            btnOutput1.Background = Brushes.LightGray;
            btnOutput2.Background = Brushes.LightGray;
            btnOutput3.Background = Brushes.LightGray;
            btnOutput4.Background = Brushes.LightGray;
            btnOutput1.IsHitTestVisible = true;
            btnOutput2.IsHitTestVisible = true;
            btnOutput3.IsHitTestVisible = true;
            btnOutput4.IsHitTestVisible = true;
        }

        public void DisableButtons()
        {
            btnOutput1.IsHitTestVisible = false;
            btnOutput2.IsHitTestVisible = false;
            btnOutput3.IsHitTestVisible = false;
            btnOutput4.IsHitTestVisible = false;
        }

        private void BtnReset1_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            UpdateScore();
            ResetButtonColours();
            CreateQuestion();
        }

        public void UpdateScore()
        {
            lblScore.Content = "Current Score: " + score.ToString();
        }
        public static void BubbleSort(String[] arr)
        {
            int length = arr.Length;
            String temp;

            for (int j = 0; j < length - 1; j++)
            {
                for (int i = j + 1; i < length; i++)
                {
                    if (arr[j].CompareTo(arr[i]) > 0)
                    {
                        temp = arr[j];
                        arr[j] = arr[i];
                        arr[i] = temp;
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            mw.Show();  
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            String message = "When reading through the options please refer to the dewey Decimal system.\n" +
                "The application will provide you with a level 3 hint. The possible answers will produce " +
                "3 random buttons as well as the correct button from level 1. If you answer correctly the " +
                "button will be highlighted green and you will then be moved onto the next level. The same " +
                "thing will occur only now you will be on level 2 and so on. If you select the inccorect " +
                "answer the button will be highlighted in red and you will have to click reset. " +
                "If you would like to go back to the menu you will be able to with the back button and if " +
                "you would like to exit the application you may by clicking exit.";

            MessageBox.Show(message, "Help Screen", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}






