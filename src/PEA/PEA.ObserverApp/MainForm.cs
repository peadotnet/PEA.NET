using Pea;
using Pea.Core;
using Pea.Core.Events;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ZedGraph;

namespace PEA.TestFormApp
{
    public partial class MainForm : Form
    {
        Type ExternalApp;
        Type? InitDataType;
        object? InitData;
        System.Windows.Forms.Timer timer = new();
        Optimizer _optimizer;
        bool Running = false;

        Stopwatch _stopwatch;

        PointPairList bestPoints;
        PointPairList avgPoints;

        ZedGraphControl graphControl;

        public MainForm()
        {
            InitializeComponent();
            graphControl = new ZedGraphControl();
            graphControl.Dock = DockStyle.Fill;
            var pane = graphControl.GraphPane;
            pane.XAxis.Title.Text = "Iterations";
            pane.YAxis.Title.Text = "Fitness";

            tabPage1.Controls.Add(graphControl);
            timer.Interval = 500;
            timer.Tick += timer_Tick;
        }

        private void importOptimizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logicAssemblyOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = logicAssemblyOpenFileDialog.FileName;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }
                try
                {
                    var assembly = Assembly.LoadFrom(fileName);

                    var externalApp = typeof(IExternalApp);
                    var types = assembly.GetTypes().ToList()
                        .Where(t => t.GetInterfaces().Contains(externalApp))
                        .ToList();
                    if (types.Count == 0)
                    {
                        MessageBox.Show("Optimizer type not found in the assembly.");
                        return;
                    }

                    ExternalApp = types[0];
                    InitDataType = ExternalApp.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType)?.GetGenericArguments().First();

                    var graphPane = graphControl.GraphPane;
                    graphPane.Title.Text = Path.GetFileNameWithoutExtension(fileName);
                    this.Refresh();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading optimizer: {ex.Message}");
                }
            }
        }

        private void openDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExternalApp == null)
            {
                MessageBox.Show("Please load an optimizer first.");
                return;
            }

            if (InitDataType == null)
            {
                MessageBox.Show("The optimizer works without parameters.");
                return;
            }

            if (initDataOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = initDataOpenFileDialog.FileName;
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }
                try
                {
                    var json = File.ReadAllText(fileName);
                    InitData = JsonSerializer.Deserialize(json, InitDataType!);

                    var pane = graphControl.GraphPane;
                    var assembly = Path.GetFileNameWithoutExtension(logicAssemblyOpenFileDialog.FileName);
                    var dataName = Path.GetFileName(fileName);
                    pane.Title.Text = $"{assembly}({dataName})";
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void startStopButton_Click(object sender, EventArgs e)
        {
            if (!Running)
            {
                var pane = graphControl.GraphPane;
                pane.CurveList.Clear();

                bestPoints = new PointPairList();
                LineItem bestCurve = pane.AddCurve("Best fitness", bestPoints, Color.Red);
                bestCurve.Symbol.Type = SymbolType.Circle;
                avgPoints = new PointPairList();
                LineItem avgCurve = pane.AddCurve("Average fitness", avgPoints, Color.Blue);
                avgCurve.Symbol.IsVisible = false;

                _optimizer = Optimizer.Create();
                _optimizer.Settings.AddBestMergedCallback(optimizer_OnNewEntitiesMergedToBest);

                var app = Activator.CreateInstance(ExternalApp);
                var method = ExternalApp.GetMethod("StartAsync");

                timer.Start();
                _stopwatch = Stopwatch.StartNew();

                //var task = (Task)converterType.InvokeMember("Convert", BindingFlags.InvokeMethod, null, converter, new object[] { inObject });
                //await task;
                //var resultProperty = task.GetType().GetProperty("Result");
                //var result = resultProperty.GetValue(task);


                Running = true;

                Task result = (Task)method?.Invoke(app, [InitData]);
                await result;

                timer.Stop();
                _stopwatch.Stop();
                Running = false;
               
            }
            else
            {
                _stopwatch.Stop();
                timer.Stop();
                Optimizer.Reset();
                _optimizer = null;
                Running = false;
            }
        }

        private void optimizer_OnNewEntitiesMergedToBest(NewEntitiesMergedToBestEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => optimizer_OnNewEntitiesMergedToBest(args)));
                return;
            }
            else
            {
                var best = args.BestEntities[0].Fitness.Value[0];
                var statistics = args.FitnessStatistics;
                var mean = statistics.StatisticVariables[0].Mean;

                    logTextBox.AppendText($"{_stopwatch.ElapsedMilliseconds / 1000}.{_stopwatch.Elapsed.Milliseconds} {args.Iteration}: {best} / {mean}" + Environment.NewLine);
                    logTextBox.ScrollToCaret();

                Plot(args.Iteration, best, mean);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var state = _optimizer.GetCurrentState();
            if (state == null) return;

            if (state.BestEntities.Count < 1) return;

            var best = state.BestEntities[0].Fitness.Value[0];
            var statistics = state.FitnessStatistics;
            var mean = statistics.StatisticVariables[0].Mean;

            Plot(state.Iteration, best, mean);

            logTextBox.AppendText($"{state.Iteration}: {best} / {mean}" + Environment.NewLine);
            logTextBox.ScrollToCaret();
            this.Refresh();
        }

        double lastBest = double.MaxValue;

        private void Plot(int iteration, double best, double avg)
        {
            if (lastBest - Math.Abs(best) > 50)
            {
                bestPoints.Add(iteration, Math.Abs(best));
                lastBest = Math.Abs(best);
            }
            avgPoints.Add(iteration, Math.Abs(avg));

            graphControl.AxisChange();
            graphControl.Invalidate();
            graphControl.Update();
            graphControl.Refresh();
        }
    }
}
