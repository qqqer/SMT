using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace SMT
{
    public partial class Form1 : Form
    {
        SocketHelper socket = new SocketHelper();
        Videojet.VJ1000ActiveX.VJ1000ActiveX activeX = new Videojet.VJ1000ActiveX.VJ1000ActiveX();


        public Form1()
        {
            InitializeComponent();
        }

        private bool checkFormat()
        {
            if (job.Text.Trim() == "") return false;

            if (v1.Text.Trim() == "" && v2.Text.Trim() == "" && v3.Text.Trim() == "" && v4.Text.Trim() == "" && v5.Text.Trim() == ""
                && v6.Text.Trim() == "" && v7.Text.Trim() == "" && v8.Text.Trim() == "" && v9.Text.Trim() == "" && v10.Text.Trim() == "")
                return false;

            int number;

            if (p1.Text.Trim() != "" && (v1.Text.Trim() == "" || !Int32.TryParse(p1.Text.Trim(), out number))) return false;
            if (p2.Text.Trim() != "" && (v2.Text.Trim() == "" || !Int32.TryParse(p2.Text.Trim(), out number))) return false;
            if (p3.Text.Trim() != "" && (v3.Text.Trim() == "" || !Int32.TryParse(p3.Text.Trim(), out number))) return false;
            if (p4.Text.Trim() != "" && (v4.Text.Trim() == "" || !Int32.TryParse(p4.Text.Trim(), out number))) return false;
            if (p5.Text.Trim() != "" && (v5.Text.Trim() == "" || !Int32.TryParse(p5.Text.Trim(), out number))) return false;
            if (p6.Text.Trim() != "" && (v6.Text.Trim() == "" || !Int32.TryParse(p6.Text.Trim(), out number))) return false;
            if (p7.Text.Trim() != "" && (v7.Text.Trim() == "" || !Int32.TryParse(p7.Text.Trim(), out number))) return false;
            if (p8.Text.Trim() != "" && (v8.Text.Trim() == "" || !Int32.TryParse(p8.Text.Trim(), out number))) return false;
            if (p9.Text.Trim() != "" && (v9.Text.Trim() == "" || !Int32.TryParse(p9.Text.Trim(), out number))) return false;
            if (p10.Text.Trim() != "" && (v10.Text.Trim() == "" || !Int32.TryParse(p10.Text.Trim(), out number))) return false;



            if (v1.Text.Trim() != "" && (p1.Text.Trim() == "" && !s1.Checked)) return false;
            if (v2.Text.Trim() != "" && (p2.Text.Trim() == "" && !s2.Checked)) return false;
            if (v3.Text.Trim() != "" && (p3.Text.Trim() == "" && !s3.Checked)) return false;
            if (v4.Text.Trim() != "" && (p4.Text.Trim() == "" && !s4.Checked)) return false;
            if (v5.Text.Trim() != "" && (p5.Text.Trim() == "" && !s5.Checked)) return false;
            if (v6.Text.Trim() != "" && (p6.Text.Trim() == "" && !s6.Checked)) return false;
            if (v7.Text.Trim() != "" && (p7.Text.Trim() == "" && !s7.Checked)) return false;
            if (v8.Text.Trim() != "" && (p8.Text.Trim() == "" && !s8.Checked)) return false;
            if (v9.Text.Trim() != "" && (p9.Text.Trim() == "" && !s9.Checked)) return false;
            if (v10.Text.Trim() != "" && (p10.Text.Trim() == "" && !s10.Checked)) return false;


            List<int> nums = new List<int>();
            if (p1.Text.Trim() != "") nums.Add(int.Parse(p1.Text.Trim()));
            if (p2.Text.Trim() != "") nums.Add(int.Parse(p2.Text.Trim()));
            if (p3.Text.Trim() != "") nums.Add(int.Parse(p3.Text.Trim()));
            if (p4.Text.Trim() != "") nums.Add(int.Parse(p4.Text.Trim()));
            if (p5.Text.Trim() != "") nums.Add(int.Parse(p5.Text.Trim()));
            if (p6.Text.Trim() != "") nums.Add(int.Parse(p6.Text.Trim()));
            if (p7.Text.Trim() != "") nums.Add(int.Parse(p7.Text.Trim()));
            if (p8.Text.Trim() != "") nums.Add(int.Parse(p8.Text.Trim()));
            if (p9.Text.Trim() != "") nums.Add(int.Parse(p9.Text.Trim()));
            if (p10.Text.Trim() != "") nums.Add(int.Parse(p10.Text.Trim()));


            if (nums.Count > 0)
            {
                nums.Sort();

                if (nums[0] != 1) return false;

                for (int i = 1; i < nums.Count; i++)
                {
                    if (nums[i] - nums[i - 1] != 1)
                        return false;
                }
            }


            return true;
        }

        private bool SendStrCommand(string StrCommand, Encoding encoding)
        {
            byte[] ByteCommand = encoding.GetBytes(StrCommand); ;
            if (socket.Send(ByteCommand) == 0)
            {
                return false;
            }

            byte[] buffer = new byte[1024];
            int receivedBytesCount = socket.Receive(buffer);
            string receivedStr = Encoding.Default.GetString(buffer).TrimEnd('\0');
            if (receivedBytesCount == 0 || !activeX.CheckDone(receivedStr))
            {
                return false;
            }

            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            button3.Focus();
            if (checkFormat() == false)
            {
                MessageBox.Show("错误：格式错误");
                return;
            }


            //建立位置映射
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (p1.Text.Trim() != "") dic.Add(p1.Text.Trim(), v1.Text);
            if (p2.Text.Trim() != "") dic.Add(p2.Text.Trim(), v2.Text);
            if (p3.Text.Trim() != "") dic.Add(p3.Text.Trim(), v3.Text);
            if (p4.Text.Trim() != "") dic.Add(p4.Text.Trim(), v4.Text);
            if (p5.Text.Trim() != "") dic.Add(p5.Text.Trim(), v5.Text);
            if (p6.Text.Trim() != "") dic.Add(p6.Text.Trim(), v6.Text);
            if (p7.Text.Trim() != "") dic.Add(p7.Text.Trim(), v7.Text);
            if (p8.Text.Trim() != "") dic.Add(p8.Text.Trim(), v8.Text);
            if (p9.Text.Trim() != "") dic.Add(p9.Text.Trim(), v9.Text);
            if (p10.Text.Trim() != "") dic.Add(p10.Text.Trim(), v10.Text);


            ///连接喷码机
            if (socket.Connect(ConfigurationManager.AppSettings["IP"], activeX.SendPort) == false)
            {
                MessageBox.Show("错误：连接喷印机失败");
                return;
            }


            ///选择模板
            string StrCommand = activeX.Select(job.Text);
            if (SendStrCommand(StrCommand, Encoding.Default) == false)
            {
                MessageBox.Show("错误：选择模板失败");
                return;
            }

            ///发送二维码值           
            if (dic.Count > 0)
            {
                string QRStrData = "[)>\u001e12\u001d";
                for (int i = 0; i < dic.Count; i++)
                {
                    QRStrData += dic[(i + 1).ToString()] + (i < dic.Count - 1 ? "\u001d" : "");
                }

                QRStrData += "\u001e\u0004";
                //StrCommand = activeX.UpdateUserFiled("Q1", QRStrData);
                StrCommand = activeX.SendMsg("Q1", QRStrData);

                if (SendStrCommand(StrCommand, Encoding.ASCII) == false)
                {
                    MessageBox.Show("错误：二维码参数设置失败");
                    return;
                }
            }

            ///发送可见字段值
            if (v1.Text.Trim() != "" && s1.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V1", v1.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v2.Text.Trim() != "" && s2.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V2", v2.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v3.Text.Trim() != "" && s3.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V3", v3.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v4.Text.Trim() != "" && s4.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V4", v4.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v5.Text.Trim() != "" && s5.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V5", v5.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v6.Text.Trim() != "" && s6.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V6", v6.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v7.Text.Trim() != "" && s7.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V7", v7.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v8.Text.Trim() != "" && s8.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V8", v8.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v9.Text.Trim() != "" && s9.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V9", v9.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }
            if (v10.Text.Trim() != "" && s10.Checked)
            {
                StrCommand = activeX.UpdateUserFiled("V10", v10.Text);
                if (SendStrCommand(StrCommand, Encoding.Default) == false)
                {
                    MessageBox.Show("错误：可见参数设置失败");
                    return;
                }
            }


            ///开启喷印
            if (SendStrCommand(activeX.Start(), Encoding.Default) == false)
            {
                MessageBox.Show("错误：开启喷印失败");
                return;
            }
            MessageBox.Show("设置成功，已开启喷印");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Focus();

            //job.Text = "";
            v1.Text = v2.Text = v3.Text = v4.Text = v5.Text = v6.Text = v7.Text = v8.Text = v9.Text = v10.Text = "";
            //p1.Text = p2.Text = p3.Text = p4.Text = p5.Text = "";
            //s1.Checked = s2.Checked = s3.Checked = s4.Checked = s5.Checked = false;
        }
    }
}
