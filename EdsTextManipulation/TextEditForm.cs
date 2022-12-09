using System.ComponentModel;
using System.Text;
using System.Linq;

namespace EdsTextManipulation
{
    public partial class TextEditForm : Form
    {
        BindingList<TextTask> taskList = new BindingList<TextTask>();
        bool calcStatFlag = true;
        private string filePath = "";

        private InputText input;
        public const int MAX_TEXT_LENGTH = 5000000;
        public const int MAX_FILE_PATH_LENGTH = 260;

        public TextEditForm()
        {
            InitializeComponent();
            
            taskListDataGridView.AutoGenerateColumns = false;
            taskListDataGridView.Columns.Clear();
            taskListDataGridView.ColumnCount = 2;
            taskListDataGridView.Columns[0].HeaderText = "Task#";
            taskListDataGridView.Columns[0].Width = 40;
            taskListDataGridView.Columns[1].HeaderText = "Task Queue";
            taskListDataGridView.Columns[1].DataPropertyName = "TaskInfo";
            taskListDataGridView.DataSource = taskList;

            taskList.ListChanged += RefreshTaskListIDs;
            taskList.ListChanged += listChanged_AdjustDataRowGridHeight;

            //will need to move this to appropriate spot...
            inputMessageLabel.Text = "";

            RefreshInputText();
            //+1 is to enforce message prompt and inform user if they attempt to provide a value greater than MAX LENGTH
            inputTextBox.MaxLength = MAX_TEXT_LENGTH+1;
            previewTextBox.MaxLength = MAX_TEXT_LENGTH+1;
        }

        private void RefreshTaskListIDs(object? sender, ListChangedEventArgs e)
        {
            for(int i = 0; i < taskList.Count; i++)
            {
                taskListDataGridView.Rows[i].Cells[0].Value = i+1;
            }
        }

        private void listChanged_AdjustDataRowGridHeight(object? sender, ListChangedEventArgs e)
        {
            AdjustDataRowGridHeight();
        }
        
        private void AdjustDataRowGridHeight()
        {
            int maxQueueBodyHeight = queueMainPanel.Height - queueFirstPanel.Height - queueLastPanel.Height;
            int totalRowHeight = taskListDataGridView.ColumnHeadersHeight + taskListDataGridView.RowTemplate.Height;

            foreach(DataGridViewRow row in taskListDataGridView.Rows)
            {
                totalRowHeight += row.Height;
            }

            queueBodyPanel.Height = Math.Min(totalRowHeight, maxQueueBodyHeight);
        }

        private void TextSourceToggle(object sender, EventArgs e)
        {
            inputTextBox.Text = "";

            if (sender == singleTextRadioButton)
            {
                singleLineTextFieldPrep();
                FileChoiceToggleFields(false);
            }
            else if (sender == multiLineRadioButton)
            {
                multiLineTextFieldPrep();
                FileChoiceToggleFields(false);
            }
            else
            {
                multiLineTextFieldPrep();
                FileChoiceToggleFields(true);
            }
        }

        private void FileChoiceToggleFields(bool bEnable)
        {
            uploadFilePanel.Enabled = bEnable;
            uploadFilePanel.Visible = bEnable;

            //inputSaveToSourceButton.Visible = bEnable;
            inputSyncFromSourceButton.Visible = bEnable;
            previewSaveAsButton.Visible = bEnable;

            if (bEnable == false)
            {
                ResetFileOptionFields();
            }
        }

        private void singleLineTextFieldPrep()
        {
            inputTextBox.Multiline = false;
            inputTextBox.Dock = DockStyle.Top;
            previewTextBox.Multiline = false;
            previewTextBox.Dock = DockStyle.Top;
        }

        private void multiLineTextFieldPrep()
        {
            inputTextBox.Multiline = true;
            inputTextBox.WordWrap = false;
            inputTextBox.Dock = DockStyle.Fill;

            previewTextBox.Multiline = true;
            previewTextBox.WordWrap = false;
            previewTextBox.Dock = DockStyle.Fill;
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            RefreshInputText();
        }

        private void toUpperCaseButton_Click(object sender, EventArgs e)
        {
            TextTaskUppercase uppercaseTask = new TextTaskUppercase();
            ExecuteOrQueue(uppercaseTask);
        }

        private void toLowerCaseButton_Click(object sender, EventArgs e)
        {
            TextTaskLowercase lowercaseTask = new TextTaskLowercase();
            ExecuteOrQueue(lowercaseTask);
        }

        private void casingPascalButton_Click(object sender, EventArgs e)
        {
            TextTaskPascalcase pascalCaseTask = new TextTaskPascalcase();
            ExecuteOrQueue(pascalCaseTask);
        }

        private void insertPrefixSuffixButton_Click(object sender, EventArgs e)
        {
            if(IsInsertTextValid())
            {
                TextTask insertPreSufTask;
                errorProvider1.SetError(insertPreSufTextBox, String.Empty);

                if (prefixRadioButton.Checked)
                {
                    insertPreSufTask = new TextTaskPrefix(insertPreSufTextBox.Text);
                }
                else if (suffixRadioButton.Checked)
                {
                    insertPreSufTask = new TextTaskSuffix(insertPreSufTextBox.Text);
                }
                else
                {
                    insertPreSufTask = new TextTaskPrefixSuffix(insertPreSufTextBox.Text);
                }

                ExecuteOrQueue(insertPreSufTask);
            }
            else
            {
                errorProvider1.SetError(insertPreSufTextBox, "Please provide non-empty value!");
            }
        }

        private void lineOrderingButton_Click(object sender, EventArgs e)
        {
            string separator = (numSeparatorTextBox.Text == "") ? " " : numSeparatorTextBox.Text;
            bool isLower = romanLowerCaseRadioButton.Checked;
            TextTask lineOrderTask;

            if (numericOrderingRadioButton.Checked)
            {
                lineOrderTask = new TextTaskNumberLine(false, separator);
            }
            else if (romanOrderingRadioButton.Checked)
            {
                lineOrderTask = new TextTaskRomanLine(isLower, separator);
            }
            else
            {
                lineOrderTask = new TextTaskLetterLine(isLower, separator);
            }

            ExecuteOrQueue(lineOrderTask);
        }

        private void trimButton_Click(object sender, EventArgs e)
        {
            char trimChar = ' ';

            TextTask trimTask;

            if (customTrimRadioButton.Checked)
            {
                if (String.IsNullOrEmpty(trimCharTextBox.Text))
                {
                    errorProvider1.SetError(trimCharTextBox, 
                        "Please specify a character to trim in the text field!");
                    return;
                }
                else
                {
                    errorProvider1.SetError(trimCharTextBox, String.Empty);
                }

                if (!char.TryParse(trimCharTextBox.Text, out trimChar))
                {
                    errorProvider1.SetError(trimCharTextBox, 
                        "Invalid character value for trimming. Please provide a single character for trimming!");
                    return;
                }
                else
                {
                    errorProvider1.SetError(trimCharTextBox, String.Empty);
                }
            }

            if (bothTrimRadioButton.Checked)
            {
                trimTask = new TextTaskTrimBothEnds(trimChar);
            }
            else if (leftTrimRadioButton.Checked)
            {
                trimTask = new TextTaskLeftTrim(trimChar);
            }
            else
            {
                trimTask = new TextTaskRightTrim(trimChar);
            }

            ExecuteOrQueue(trimTask);
        }

        private void findAndReplaceButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(findToReplaceTextBox.Text))
            {
                errorProvider1.SetError(findToReplaceTextBox,
                    "Please provide a value!");
                return;
            }
            else
            {
                errorProvider1.SetError(findToReplaceTextBox, String.Empty);

                TextTaskReplace replaceTask = new TextTaskReplace(findToReplaceTextBox.Text,
                    replaceTextBox.Text, findIgnoreCaseCheckBox.Checked);
                ExecuteOrQueue(replaceTask);
            }
        }

        private void reverseButton_Click(object sender, EventArgs e)
        {
            TextTaskReverse reverseTask = new TextTaskReverse();
            ExecuteOrQueue(reverseTask);
        }

        private void spaceRemovalButton_Click(object sender, EventArgs e)
        {
            TextTaskReplace replaceTask = new TextTaskReplace(" ", "", findIgnoreCaseCheckBox.Checked);
            ExecuteOrQueue(replaceTask);
        }

        private void removeEmptyLinesButton_Click(object sender, EventArgs e)
        {
            TextTaskRemoveEmptyLines removeEmptyLinesTask = new TextTaskRemoveEmptyLines();
            ExecuteOrQueue(removeEmptyLinesTask);
        }

        public void ExecuteOrQueue(TextTask task)
        {
            if (queueDisabledRadioButton.Checked)
            {
                try
                {
                    task.ExecuteTask(input.Render);
                    RefreshPreviewText();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                taskList.Add(task);
            }
        }

        private void RefreshInputStats()
        {
            if (calcStatFlag)
                inputStatTextBox.Text = input.DisplayStatistics("Input");
        }

        private void RefreshPreviewStats()
        {
            if (calcStatFlag)
                previewStatTextBox.Text = input.Render.DisplayStatistics("Preview");
        }

        private void RefreshInputText()
        {
            try
            {
                input = new InputText(inputTextBox.Text);
                RefreshPreviewText();
                RefreshInputStats();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                inputTextBox.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshPreviewText()
        {
            previewTextBox.Text = input.Render.Value;
            RefreshPreviewStats();
        }
        
        private void resetPreviewButton_Click(object sender, EventArgs e)
        {
            input.ResetPreviewText(input.Value);
            RefreshPreviewText();
        }

        private void copyPreviewButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(previewTextBox.Text);
        }

        private void clearInputButton_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(inputTextBox.Text))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete Input text?", "Clear Input Text Field", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    inputTextBox.Text = "";
                }
            }
        }

        private void customTrimRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            trimCharTextBox.Enabled = true;
        }

        private void trimSpacesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            trimCharTextBox.Enabled = false;
        }

        private void lineNumCapitalizationEnabling(object sender, EventArgs e)
        {
            if (numericOrderingRadioButton.Checked)
            {
                lineNumCasingGroupBox.Enabled = false;
                lineNumCasingGroupBox.Visible = false;
            }
            else if (romanOrderingRadioButton.Checked || alphaOrderingRadioButton.Checked)
            {
                lineNumCasingGroupBox.Enabled = true;
                lineNumCasingGroupBox.Visible = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void queueEnabledRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            section2Panel.Visible = queueEnabledRadioButton.Checked;
        }

        private void clearAllTaskButton_Click(object sender, EventArgs e)
        {
            taskList.Clear();
        }

        private void deleteTaskButton_Click(object sender, EventArgs e)
        {            
            foreach (DataGridViewRow row in taskListDataGridView.SelectedRows)
            {
                taskListDataGridView.Rows.RemoveAt(row.Index);
            }
        }

        private void executeTasksButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in taskList)
                {
                    item.ExecuteTask(input.Render);
                }

                RefreshPreviewText();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsInsertTextValid() => !string.IsNullOrEmpty(insertPreSufTextBox.Text);

        private void prefixRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            insertPrefixSuffixButton.Text = "Insert Prefix";
        }

        private void suffixRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            insertPrefixSuffixButton.Text = "Insert Suffix";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            insertPrefixSuffixButton.Text = "Insert Both Ends";
        }

        private void moveTaskUpButton_Click(object sender, EventArgs e)
        {
            int currIndex = taskListDataGridView.SelectedRows[0].Index;
            if(currIndex > 0)
            {
                TextTask task = taskList[currIndex];
                taskList.RemoveAt(currIndex);
                taskList.Insert(currIndex - 1, task);
                taskListDataGridView.Rows[currIndex-1].Selected = true;
            }
        }

        private void moveTaskDownButton_Click(object sender, EventArgs e)
        {
            int currIndex = taskListDataGridView.SelectedRows[0].Index;
            if (currIndex >= 0 && currIndex < taskList.Count - 1)
            {
                TextTask task = taskList[currIndex];
                taskList.RemoveAt(currIndex);
                taskList.Insert(currIndex + 1, task);
                taskListDataGridView.Rows[currIndex + 1].Selected = true;
            }
        }

        private void TextEditForm_ResizeEnd(object sender, EventArgs e)
        {
            AdjustDataRowGridHeight();
            AdjustInputPreviewHeights();
        }

        private void AdjustInputPreviewHeights()
        {
            inputPanel.Height = previewPanel.Height = this.Height / 2;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Browse a file";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|Json files (*.json)|*.json|CSV files (*.csv)|*.csv";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;

                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (openFileDialog.FileName.Length > MAX_FILE_PATH_LENGTH)
                        {
                            throw new PathTooLongException($"File path: ({openFileDialog.FileName}) " +
                                $"exceeds path char limit: {openFileDialog.FileName.Length} / {MAX_FILE_PATH_LENGTH}");
                        }

                        filePath = openFileDialog.FileName;
                        filePathTextBox.Text = filePath;
                    }
                }
            }
            catch (PathTooLongException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private string GetFileNameAndExt(string filePathName) => (filePathName.Contains('\\')) ? 
        //    filePathName.Substring(filePathName.LastIndexOf('\\') + 1) : filePathName;
 
        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePathTextBox.Text))
            {
                errorProvider1.SetError(filePathTextBox, "Must provide a valid file path");
                return;
            }
            else
            {
                errorProvider1.SetError(filePathTextBox, "");
            }

            ReadFile();
        }

        private void ReadFile()
        {
            StringBuilder strTextBody = new StringBuilder();
            int removeChars = Environment.NewLine.Length;

            int currStringLength = 0;
            inputTextBox.Text = "";

            try
            {
                foreach (var line in File.ReadLines(filePathTextBox.Text))
                {
                    currStringLength += line.Length + 2;
                    //AppendLine had some weird inconsistencies when it came to adding NewLines
                    if (currStringLength > MAX_TEXT_LENGTH)
                    {
                        throw new ArgumentOutOfRangeException(nameof(filePathTextBox), 
                            string.Format("{0:#,###0.#}", $"greater than {string.Format("{0:#,###0.#}", TextEditForm.MAX_TEXT_LENGTH)}"),
                            $"File: ({filePathTextBox.Text})\r\nexceeds the allowed max text length ");
                    }

                    strTextBody.AppendLine(line);
                }

                if (strTextBody.Length > 0)
                {
                    strTextBody.Remove(strTextBody.Length - removeChars, removeChars);
                }

                inputTextBox.Text = strTextBody.ToString();
                inputMessageLabel.Text = $"Source: {Path.GetFileName(filePath)}";
                //inputSaveToSourceButton.Enabled = true;
                inputSyncFromSourceButton.Enabled = true;
                previewSaveAsButton.Enabled = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                inputTextBox.Text = "";
                ResetFileOptionFields();
            }
        }

        private void ResetFileOptionFields()
        {
            inputMessageLabel.Text = filePathTextBox.Text = filePath = "";

            //inputSaveToSourceButton.Enabled = false;
            inputSyncFromSourceButton.Enabled = false;
            previewSaveAsButton.Enabled = false;
        }

        private void inputSyncFromSourceButton_Click(object sender, EventArgs e)
        {
            if(!File.Exists(filePath))
            {
                MessageBox.Show($"File: \"{filePath}\" doesn't exist or is an empty path");
            }
            else
            {
                ReadFile();
            }
        }

        //private void inputSaveToSourceButton_Click(object sender, EventArgs e)
        //{
        //    SaveTextToFile(filePath, inputTextBox.Text);
        //}

        private string GetFilePathToSave(string folderPath, string extension = ".txt")
        {
            //HHmmss
            string fileNameCandidate = $"Text_{DateTime.Now.ToString("yyyyMMdd")}";

            MessageBox.Show("BASE: " + Path.Combine(folderPath, $"{fileNameCandidate}{extension}"));

            if (!File.Exists(Path.Combine(folderPath, $"{fileNameCandidate}{extension}")))
            {
                return Path.Combine(folderPath, $"{fileNameCandidate}{extension}");
            }

            return GetNextAvailableFileName(folderPath, fileNameCandidate, extension);
        }

        private string GetNextAvailableFileName(string folderPath, string fileName, string extension)
        {
            int index = 0;

            string fileNameCandidate = String.Format("{0}_{1}", fileName, index);

            while(File.Exists(Path.Combine(folderPath, $"{fileNameCandidate}{extension}")))
            {
                index++;
                fileNameCandidate = String.Format("{0}_{1}", fileName, index);
            }

            return Path.Combine(folderPath, $"{fileNameCandidate}{extension}");
        }

        private void SaveTextToFile(string textToSave)
        {
            try
            {
                using (FolderBrowserDialog folderPrompt = new FolderBrowserDialog())
                {
                    folderPrompt.ShowNewFolderButton = false;
                    folderPrompt.Description = "Select a folder to save Preview Data";
                    folderPrompt.RootFolder = Environment.SpecialFolder.MyDocuments;

                    if (folderPrompt.ShowDialog() == DialogResult.OK)
                    {
                        string savedDestination = GetFilePathToSave(folderPrompt.SelectedPath, Path.GetExtension(filePath));

                        File.WriteAllText(savedDestination, textToSave);

                        MessageBox.Show($"File Saved Successfully to {savedDestination}\r\n");
                    }
                }
            }
            catch (PathTooLongException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearTextStatMenuStates()
        {
            autoTextStatToolStripMenuItem.Checked = false;
            manualTextStatToolStripMenuItem.Checked = false;
            disableTextStatToolStripMenuItem.Checked = false;
        }

        private void autoTextStatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearTextStatMenuStates();
            autoTextStatToolStripMenuItem.Checked = true;
            calcStatFlag = true;

            manualCalcTextStatButton.Visible = false;
            manualCalcTextStatButton.Enabled = false;
        }

        private void manualTextStatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearTextStatMenuStates();
            manualTextStatToolStripMenuItem.Checked = true;
            calcStatFlag = false;

            manualCalcTextStatButton.Visible = true;
            manualCalcTextStatButton.Enabled = true;
        }

        private void disableTextStatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputStatTextBox.Text = "";
            previewStatTextBox.Text = "";

            ClearTextStatMenuStates();
            disableTextStatToolStripMenuItem.Checked = true;
            calcStatFlag = false;

            manualCalcTextStatButton.Visible = false;
            manualCalcTextStatButton.Enabled = false;
        }

        private void showhideTextStatMenuItem_Click(object sender, EventArgs e)
        {
            inputStatTextBox.Visible = !inputStatTextBox.Visible;
            previewStatTextBox.Visible = !previewStatTextBox.Visible;

            inputSplitContainer.Panel1Collapsed = !inputSplitContainer.Panel1Collapsed;
            previewSplitContainer.Panel1Collapsed = !previewSplitContainer.Panel1Collapsed;
        }

        private void manualCalcTextStatButton_Click(object sender, EventArgs e)
        {
            calcStatFlag = true;
            RefreshInputStats();
            RefreshPreviewStats();
            calcStatFlag = false;
        }

        private void previewSaveAsButton_Click(object sender, EventArgs e)
        {
            SaveTextToFile(previewTextBox.Text);
        }
    }
}