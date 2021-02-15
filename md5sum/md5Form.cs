using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace md5sum
{
    public partial class Md5Form : Form
    {
        public const string Md5FormName = "janbar-md5sum";
        private readonly CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;

        private CheckBox[] CbArr = null;
        private XmlConfig AppConfig;

        /* 线程安全队列,用于异步处理传入需要校验文件路径 */
        private readonly ConcurrentQueue<string[]> handleQueue = new ConcurrentQueue<string[]>();

        private CancellationTokenSource CtrlTask = null;

        public Md5Form(string[] args)
        {
            InitializeComponent();
            handleQueue.Enqueue(args);
        }

        private void Md5Form_Load(object sender, EventArgs e)
        {
            AppConfig = new XmlConfig(Md5FormName, "1110001", Location).GetConfig();
            if (AppConfig.Pos == Point.Empty)
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - Width) / 2,
                    (Screen.PrimaryScreen.Bounds.Height - Height) / 2); /* 窗体居中显示 */
            else
                Location = AppConfig.Pos;

            Text = Md5FormName; /* 设置标题栏文字 */
            TbShow.Font = new Font(TbShow.Font.Name, 10);

            CbArr = new CheckBox[] { CbVersion, CbTime, CbMd5, CbSha1, CbSha256, CbCrc32, CbUpLe };
            for (int i = 0; i < AppConfig.CheckBoxConfig.Length; i++)
                if (AppConfig.CheckBoxConfig[i] != '0')
                    CbArr[i].Checked = true;

            ThreadPool.QueueUserWorkItem(delegate
            {
                while (true)
                {   /* 通过命名管道接收其他进程发送的参数 */
                    using (NamedPipeServerStream server = new NamedPipeServerStream(Md5FormName, PipeDirection.In))
                    {
                        server.WaitForConnection();
                        using (StreamReader sr = new StreamReader(server))
                        {
                            handleQueue.Enqueue(sr.ReadLine().Split('|'));
                        }
                    }
                }
            });

            ThreadPool.QueueUserWorkItem(delegate
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                while (true)
                {
                    Thread.Sleep(rand.Next(50, 500)); /* 让电脑适当放松 */
                    if (handleQueue.TryDequeue(out string[] paths))
                    {
                        paths = FilterPath(paths);
                        if (paths.Length > 0)
                        {
                            CtrlTask = new CancellationTokenSource();

                            Hash hash = new Hash(CtrlTask.Token, CbVersion.Checked, CbTime.Checked, CbMd5.Checked, CbSha1.Checked, CbSha256.Checked, CbCrc32.Checked, CbUpLe.Checked);
                            hash.UpdatePbDelegate = UpdatePbUIStatus;
                            hash.AppendTbShowText = AppendTbShowStatus;
                            hash.UpdateStartStopUI = StartStopUpdateUI;

                            /* 开启计算任务,可以用CtrlTask取消任务 */
                            Task calcTask = new Task(() => { hash.Calculate(paths); }, CtrlTask.Token);
                            calcTask.Start();
                            calcTask.Wait();
                            calcTask.Dispose();

                            CtrlTask.Dispose();
                            CtrlTask = null;
                        }
                    }
                }
            });
        }

        /* 异步更新进度条 */
        delegate void SetProgressBar(ProgressBar pb, int val, int max);
        private void UpdatePbUIStatus(bool isNow, int val, int max)
        {
            ProgressBar pb = isNow ? PbNow : PbAll;
            if (pb.InvokeRequired)
            {
                pb.Invoke(new SetProgressBar(delegate (ProgressBar a, int b, int c)
                {
                    if (b >= 0) a.Value = b;
                    if (c >= 0) a.Maximum = c;
                }), pb, val, max);
            }
        }
        /* 异步追加文本 */
        delegate void AppendTextBox(TextBox tb, string text);
        private void AppendTbShowStatus(string text)
        {
            if (TbShow.InvokeRequired)
            {
                TbShow.Invoke(new AppendTextBox(delegate (TextBox a, string b)
                {
                    a.AppendText(b);
                }), TbShow, text);
            }
        }

        /* 启动+停止计算时更新UI */
        private void StartStopUpdateUI(bool isStart)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    if (isStart)
                    {
                        foreach (CheckBox cb in CbArr)
                            cb.Enabled = false;
                        btnBrowse.Enabled = false;
                        btnCopy.Enabled = false;
                        btnClear.Enabled = false;
                        btnSave.Enabled = false;
                        btnClose.Text = "停止(&X)";
                    }
                    else
                    {
                        foreach (CheckBox cb in CbArr)
                            cb.Enabled = true;
                        btnBrowse.Enabled = true;
                        if (TbShow.TextLength > 0)
                        {
                            btnCopy.Enabled = true;
                            btnClear.Enabled = true;
                            btnSave.Enabled = true;
                        }
                        btnClose.Text = "关闭(&X)";
                        btnClose.Enabled = true;
                    }
                }));
            }
        }

        private void BtnCmpare_Click(object sender, EventArgs e)
        {
            SelectCmpText();
        }

        private void SelectCmpText()
        {
            string cmp = TbInput.Text;
            if (!string.IsNullOrEmpty(cmp))
            {   // 查找比较内容,忽略大小写
                int start = Compare.IndexOf(TbShow.Text, cmp, CompareOptions.IgnoreCase);
                if (start == -1)
                    MessageBox.Show("无匹配项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    TbShow.Focus();
                    TbShow.SelectionStart = start;
                    TbShow.SelectionLength = cmp.Length;
                    TbShow.ScrollToCaret();
                }
            }
        }

        private void TbInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                e.Handled = true;
                SelectCmpText();
            }
            else
            {
                btnCmpare.Enabled = TbShow.TextLength > 0 && TbInput.TextLength > 0;
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (TbInput.Focused)
            { /* 焦点在界面敲回车触发,需要识别焦点在输入框中敲回车 */
                SelectCmpText();
            }
            else if (openAllFile.ShowDialog() == DialogResult.OK)
            {
                handleQueue.Enqueue(openAllFile.FileNames);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(TbShow.Text, true);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TbShow.Clear();
            PbNow.Value = 0;
            PbAll.Value = 0;
            btnCmpare.Enabled = false;
            btnCopy.Enabled = false;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (saveAllFile.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveAllFile.FileName, TbShow.Text);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (CtrlTask != null)
            {
                btnClose.Enabled = false;
                CtrlTask.Cancel(); /* 取消正在执行的任务,同时禁用按钮防止多次取消 */
                return;
            }
            Close();
        }

        /* 关闭窗体将配置写入文件 */
        private void Md5Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            string config = "";
            foreach (CheckBox cb in CbArr)
                if (cb.Checked)
                    config += "1";
                else
                    config += "0";
            AppConfig.SaveXmlConfig(Location, config);
        }

        private void TbShow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                TbShow.Cursor = Cursors.Arrow;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TbShow_DragDrop(object sender, DragEventArgs e)
        {
            handleQueue.Enqueue((string[])e.Data.GetData(DataFormats.FileDrop));
            TbShow.Cursor = Cursors.IBeam;
        }

        private void TbShow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                TextBox input = (TextBox)sender;
                if (e.KeyCode == Keys.A)
                {
                    input.SelectAll();
                    Clipboard.SetDataObject(input.Text, true);
                }
                else if (e.KeyCode == Keys.C)
                {
                    Clipboard.SetDataObject(input.SelectedText, true);
                }
            }
        }

        /// <summary>
        /// 传入参数,返回其中文件存在的路径
        /// </summary>
        /// <param name="paths">传入参数</param>
        /// <returns>返回文件存在的路径</returns>
        private static string[] FilterPath(string[] paths)
        {
            List<string> sendPaths = new List<string>();
            foreach (string s in paths)
                if (File.Exists(s))
                    /* 文件存在则将文件绝对路径加入列表 */
                    sendPaths.Add(Path.GetFullPath(s));
            return sendPaths.ToArray();
        }

        /// <summary>
        /// 将文件路径通过命名管道发送给正在运行的实例
        /// </summary>
        /// <param name="paths">文件路径</param>
        public static void SendFile(string[] paths)
        {
            paths = FilterPath(paths);
            if (paths.Length <= 0)
                return;

            using (NamedPipeClientStream client = new NamedPipeClientStream("localhost", Md5FormName, PipeDirection.Out))
            {
                client.Connect(5000);
                using (StreamWriter sw = new StreamWriter(client))
                {
                    sw.WriteLine(string.Join("|", paths));
                }
            }
        }

        /// <summary>
        /// 处理配置文件
        /// </summary>
        private class XmlConfig
        {
            public Point Pos;
            public string CheckBoxConfig;
            private string ConfigPath;

            public XmlConfig(string configName, string checkBox, Point pos)
            {
                Pos = pos;
                ConfigPath = configName;
                CheckBoxConfig = checkBox;
            }

            public XmlConfig GetConfig()
            {
                try
                {
                    string BaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "janbar");
                    if (!Directory.Exists(BaseDir)) /* 确保默认目录存在 */
                        Directory.CreateDirectory(BaseDir);
                    ConfigPath = Path.Combine(BaseDir, ConfigPath);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(ConfigPath);
                    XmlNode rootNode = xmldoc.SelectSingleNode("md5sum");

                    XmlNode tmpNode = rootNode.SelectSingleNode("CheckBox");
                    string text = tmpNode.InnerText;
                    if (text.Length == CheckBoxConfig.Length)
                        CheckBoxConfig = text;

                    tmpNode = rootNode.SelectSingleNode("Location");
                    Pos = new Point(Convert.ToInt32(tmpNode.Attributes.GetNamedItem("X").Value),
                        Convert.ToInt32(tmpNode.Attributes.GetNamedItem("Y").Value));
                }
                catch
                {
                    Pos = Point.Empty;
                }
                return this;
            }

            public void SaveXmlConfig(Point pos, string checkBox)
            {
                bool isChange = false;
                XmlDocument xmlDoc = new XmlDocument();
                if (Pos == Point.Empty || !File.Exists(ConfigPath))
                {   /* 读取时发生异常,或者文件不存在时需要创建新的文件 */
                    xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

                    XmlElement rootElement = xmlDoc.CreateElement("md5sum");
                    xmlDoc.AppendChild(rootElement);

                    XmlElement element = xmlDoc.CreateElement("Location");
                    XmlAttribute attr = xmlDoc.CreateAttribute("X");
                    attr.Value = pos.X.ToString();
                    element.Attributes.Append(attr);

                    attr = xmlDoc.CreateAttribute("Y");
                    attr.Value = pos.Y.ToString();
                    element.Attributes.Append(attr);
                    rootElement.AppendChild(element);

                    element = xmlDoc.CreateElement("CheckBox");
                    element.InnerText = checkBox;
                    rootElement.AppendChild(element);
                    isChange = true;
                }
                else
                {   /* 解析文件,并修改对应属性 */
                    if (!pos.Equals(Pos))
                    {
                        xmlDoc.Load(ConfigPath);
                        XmlElement setAttr = (XmlElement)xmlDoc.SelectSingleNode("md5sum/Location");
                        setAttr.SetAttribute("X", pos.X.ToString());
                        setAttr.SetAttribute("Y", pos.Y.ToString());
                        isChange = true;
                    }
                    if (CheckBoxConfig != checkBox)
                    {
                        if (!isChange)
                            xmlDoc.Load(ConfigPath);
                        XmlElement setAttr = (XmlElement)xmlDoc.SelectSingleNode("md5sum/CheckBox");
                        setAttr.InnerText = checkBox;
                        isChange = true;
                    }
                }
                if (isChange)
                    xmlDoc.Save(ConfigPath);
            }
        }
    }
}
