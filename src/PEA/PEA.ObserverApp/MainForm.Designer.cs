namespace PEA.TestFormApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            importOptimizerToolStripMenuItem = new ToolStripMenuItem();
            openDataToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            startStopButton = new ToolStripButton();
            splitContainer1 = new SplitContainer();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            logTextBox = new RichTextBox();
            logicAssemblyOpenFileDialog = new OpenFileDialog();
            initDataOpenFileDialog = new OpenFileDialog();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(985, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importOptimizerToolStripMenuItem, openDataToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "&File";
            // 
            // importOptimizerToolStripMenuItem
            // 
            importOptimizerToolStripMenuItem.Name = "importOptimizerToolStripMenuItem";
            importOptimizerToolStripMenuItem.Size = new Size(270, 34);
            importOptimizerToolStripMenuItem.Text = "I&mport optimizer";
            importOptimizerToolStripMenuItem.Click += importOptimizerToolStripMenuItem_Click;
            // 
            // openDataToolStripMenuItem
            // 
            openDataToolStripMenuItem.Name = "openDataToolStripMenuItem";
            openDataToolStripMenuItem.Size = new Size(270, 34);
            openDataToolStripMenuItem.Text = "&Open data";
            openDataToolStripMenuItem.Click += openDataToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(270, 34);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { startStopButton });
            toolStrip1.Location = new Point(0, 33);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(985, 33);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // startStopButton
            // 
            startStopButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            startStopButton.Image = (Image)resources.GetObject("startStopButton.Image");
            startStopButton.ImageTransparentColor = Color.Magenta;
            startStopButton.Name = "startStopButton";
            startStopButton.Size = new Size(34, 28);
            startStopButton.Text = "toolStripButton1";
            startStopButton.Click += startStopButton_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 66);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(logTextBox);
            splitContainer1.Size = new Size(985, 482);
            splitContainer1.SplitterDistance = 352;
            splitContainer1.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(985, 352);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(977, 314);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // logTextBox
            // 
            logTextBox.Dock = DockStyle.Fill;
            logTextBox.Location = new Point(0, 0);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(985, 126);
            logTextBox.TabIndex = 0;
            logTextBox.Text = "";
            // 
            // logicAssemblyOpenFileDialog
            // 
            logicAssemblyOpenFileDialog.Filter = ".dll files| *.dll|.exe files |*.exe|All files| *.*";
            // 
            // initDataOpenFileDialog
            // 
            initDataOpenFileDialog.Filter = "Json files|*.json|All files|*.*";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(985, 548);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "PEA.NET";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton startStopButton;
        private SplitContainer splitContainer1;
        private RichTextBox logTextBox;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private ToolStripMenuItem importOptimizerToolStripMenuItem;
        private ToolStripMenuItem openDataToolStripMenuItem;
        private OpenFileDialog logicAssemblyOpenFileDialog;
        private OpenFileDialog initDataOpenFileDialog;
    }
}
