using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopStar
{
    public partial class GridPicture : UserControl
    {
        public GridPicture()
        {
            InitializeComponent();
        }

        private enum StarColor 
        {
            White = 0, //表示星星已经消除
            Purple = 1,
            Red = 2,
            Yellow = 3,
            Blue = 4,
            Green = 5,
        }

        private int[] dx = { -1, 0, 1, 0 };
        private int[] dy = { 0, -1, 0, 1 };
        private Point[] tmp;
        private PictureBox[,] pictureBoxs;
        private StarColor[,] starsColor;
        private Boolean[,] starsStatue; 
        private int starCount;

        private int score = 0;
        private int round = 1;

        private Point one, two;

        private void GridPicture_Load(object sender, EventArgs e)
        {
            pictureBoxs = new PictureBox[Row, Column];
            starsColor = new StarColor[Row, Column];
            starsStatue = new Boolean[Row, Column];
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            label9.Visible = false;
            label3.Text = round.ToString();
            label4.Text = GetRoundScore().ToString();
            label8.Text = score.ToString();
            one = label5.Location;
            two = label6.Location;
            timer2.Start();   
        }
        private void initGridPicture()
        {
            Random rand = new Random();
            Row = 10; Column = 10;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    int temp = rand.Next(0, 5);
                    if (pictureBoxs[i, j] == null)
                    {
                        pictureBoxs[i, j] = new PictureBox();
                        pictureBoxs[i, j].Click += new EventHandler(GridPicture_Click);
                        pictureBoxs[i, j].DoubleClick += new EventHandler(GridPicture_DoubleClick);
                        //pictureBoxs[i, j].BackgroundImageChanged += new EventHandler(GridPicture_BackgroundImageChanged);
                        pictureBoxs[i, j].BackColor = Color.Transparent;
                        pictureBoxs[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                        pictureBoxs[i, j].Name = i.ToString() + "-" + j.ToString();
                        pictureBoxs[i, j].Width = HbWidth;
                        pictureBoxs[i, j].Height = HbHeight;
                        pictureBoxs[i, j].Location = new Point(i * HbWidth, j * HbHeight+190);
                        this.Controls.Add(pictureBoxs[i, j]);
                    }
                    pictureBoxs[i, j].BackgroundImage = imageList.Images[temp];
                    starsColor[i, j] = (StarColor)(temp + 1);
                }
            }
        }

        private void PopStar()
        {
            starCount = 0;
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Column; j++)
                {
                    if (pictureBoxs[i, j].BackgroundImage != null)
                    {
                        starCount++;
                    }
                }
            score += GetReward();
            string str = "剩余 " + starCount + " 个星星 \n 奖励 " + GetReward() + " 分";
            label9.Text = str;
            label9.Visible = true;
            if (score > GetRoundScore())
            {
                for (int i = 0; i < Row; i++)
                {
                    for (int j = 0; j < Column; j++)
                    {
                        pictureBoxs[i, j].BackgroundImage = null;
                    }
                }
                time = 0;
                timer1.Start();
            }
            else
            {
                //Thread.sleep(1000);
                label9.Visible = false;
                DialogResult result = MessageBox.Show("游戏结束,是否重新开始!", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    round = 1; score = 0;
                    label3.Text = round.ToString();
                    label4.Text = GetRoundScore().ToString();
                    label8.Text = score.ToString();
                    label5.Text = "第 " + round + " 关";
                    label6.Text = "过关分数 " + GetRoundScore().ToString();
                    label5.Visible = true;
                    label6.Visible = true;
                    timer2.Start();
                }
            }
        }

        private void GridPicture_Click(object sender, EventArgs e)//单击将选中的星星标出
        {
            this.Refresh();
            if (((PictureBox)sender).BackgroundImage == null)
            {
                return;
            }
            else
            {
                for (int i = 0; i < Row; i++)
                    for (int j = 0; j < Column; j++)
                    {
                        starsStatue[i, j] = false;
                        if (pictureBoxs[i, j] != null)
                        {
                            pictureBoxs[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                
                string name = ((PictureBox)sender).Name;
                int x = Int32.Parse(name.Substring(0, name.IndexOf('-')));
                int y = Int32.Parse(name.Substring(name.IndexOf('-') + 1));

                tmp = new Point[Row * Column];
                int left = 0, right = 1;
                tmp[left] = new Point(x, y);
                while (left < right)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (tmp[left].X + dx[i] >= 0 && tmp[left].X + dx[i] < Row && tmp[left].Y+dy[i] >= 0 && tmp[left].Y+dy[i] < Column
                            && starsColor[tmp[left].X + dx[i], tmp[left].Y + dy[i]] == starsColor[x, y] && starsStatue[tmp[left].X + dx[i], tmp[left].Y + dy[i]] == false)
                        {
                            //MessageBox.Show(starsColor[x, y].ToString());
                            //Console.WriteLine("{0},{1},{2}",tmp[left].X+dx[i],tmp[left].Y+dy[i],starsColor[x, y].ToString());
                            if (right < Row * Column)
                            {
                                tmp[right] = new Point(tmp[left].X + dx[i], tmp[left].Y + dy[i]);
                                right++;
                            }
                            starsStatue[tmp[left].X + dx[i], tmp[left].Y + dy[i]] = true;
                        }
                    }
                    left++;
                }

                for (int i = 0; i < Row; i++)
                    for (int j = 0; j < Column; j++)
                        if (starsStatue[i, j])
                        {
                            pictureBoxs[i, j].BackgroundImageLayout = ImageLayout.Center;
                        }

            }
        }

        private void GridPicture_DoubleClick(object sender, EventArgs e)//双击消除选中的星星
        {
            this.Refresh();
            if (((PictureBox)sender).BackgroundImage == null)
            {
                return;
            }
            else
            {
                for (int i = 0; i < Row; i++)
                    for (int j = 0; j < Column; j++)
                    {
                        starsStatue[i, j] = false;
                        if (pictureBoxs[i, j] != null)
                        {
                            pictureBoxs[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }

                string name = ((PictureBox)sender).Name;
                int x = Int32.Parse(name.Substring(0, name.IndexOf('-')));
                int y = Int32.Parse(name.Substring(name.IndexOf('-') + 1));

                int num = 1;//记录消除的星星数 

                tmp = new Point[Row * Column];
                int left = 0, right = 1;
                tmp[left] = new Point(x, y);
                starsStatue[x, y] = true;
                while (left < right)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (tmp[left].X + dx[i] >= 0 && tmp[left].X + dx[i] < Row && tmp[left].Y + dy[i] >= 0 && tmp[left].Y + dy[i] < Column
                            && starsColor[tmp[left].X + dx[i], tmp[left].Y + dy[i]] == starsColor[x, y] && starsStatue[tmp[left].X + dx[i], tmp[left].Y + dy[i]] == false)
                        {
                            //MessageBox.Show(starsColor[x, y].ToString());
                            //Console.WriteLine("{0},{1},{2}",tmp[left].X+dx[i],tmp[left].Y+dy[i],starsColor[x, y].ToString());
                            if (right < Row * Column)
                            {
                                tmp[right] = new Point(tmp[left].X + dx[i], tmp[left].Y + dy[i]);
                                right++;
                                num++;
                            }
                            starsStatue[tmp[left].X + dx[i], tmp[left].Y + dy[i]] = true;
                        }
                    }
                    left++;
                }

                if (num >= 2)
                {
                    score += getScore(num);
                    label8.Text = score.ToString();
                    starFall();
                    starLeftMove();
                    if (NoLeft())
                    {
                        PopStar();
                    }
                }
            }
        }

        private int time; 
        private void timer1_Tick(object sender, EventArgs e)//每局游戏开始时的起始动画
        {
            if (time == 30000)
            {
                label9.Visible = false;
                timer1.Stop();
                round++;
                label3.Text = round.ToString();
                label4.Text = GetRoundScore().ToString();
                label5.Text = "第 " + round + " 关";
                label6.Text = "过关分数 " + GetRoundScore().ToString();
                label5.Visible = true;
                label6.Visible = true;
                label5.Location = one;
                label6.Location = two;
                timer2.Start();
            }
            else
            {
                time += 1000;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Point roundPonit = label5.Location;
            roundPonit.Y -= 10;
            label5.Location = roundPonit;
            Point scorePonit = label6.Location;
            scorePonit.Y -= 10;
            label6.Location = scorePonit;
            //Console.WriteLine("{0} {1}", roundPonit.Y, scorePonit.Y);
            if (scorePonit.Y + 10 < 200)
            {
                timer2.Stop();
                label5.Visible = false;
                label6.Visible = false;
                initGridPicture();
            }
        }

        private bool NoLeft()//判断剩下星星是否能被消除
        {
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Column; j++)
                {
                    if (pictureBoxs[i, j].BackgroundImage != null)
                    {
                        if (i+1 < Row && starsColor[i,j] == starsColor[i+1,j])
                        {
                            return false;
                        }
                        if (j+1 < Column && starsColor[i,j] == starsColor[i,j+1])
                        {
                            return false;
                        }
                    }
                }
            return true;
        }
        private void starLeftMove()//出现空列时星星向左移动
        {
            int len = 0;
            for (int i = 0; i < Row; i++)
            {
                if (ColumnIsNull(i) == false)
                {
                    for (int j = 0; j < Column; j++)
                    {
                        starsColor[len, j] = starsColor[i, j];
                        pictureBoxs[len, j].BackgroundImage = pictureBoxs[i, j].BackgroundImage;
                    }
                    len++;
                }
            }
            for (int i = len; i < Row; i++)
            {
                for (int j = 0; j< Column; j++)
                {
                    starsColor[i, j] = 0;
                    pictureBoxs[i, j].BackgroundImage = null;
                }
            }
        }

        private bool ColumnIsNull(int x)//判断每列是否为空
        {
            for (int j = 0; j < Column; j++)
            {
                if (pictureBoxs[x, j].BackgroundImage != null)
                {
                    return false;
                }
            }
            return true;
        }

        private void starFall()//星星下落
        {
            for (int i = 0; i < Row ; i++)
            {
                int len = Column-1;
                for (int j = Column-1; j >= 0; j--)
                {
                    if (starsStatue[i, j] == false)
                    {
                        starsColor[i, len] = starsColor[i, j];
                        pictureBoxs[i, len].BackgroundImage = pictureBoxs[i, j].BackgroundImage;
                        len--;
                    }
                }
                for (int j = len; j >= 0; j--)
                {
                    starsColor[i, j] = 0;
                    pictureBoxs[i, j].BackgroundImage = null;
                }
            }
        }

        private int getScore(int count)//获得本次消除获得的分数
        {
            int num = 10;
            for (int i = 3; i <= count; i++)
            {
                num += 5;
            }
            return num * count;
        }

        private int GetReward()//获得本关奖励
        {
            if (starCount >= 10)
            {
                return 0;
            }
            else
            {
                int num = 20;
                int score = 2000;
                for (int i = 0; i < starCount; i++)
                {
                    score -= num;
                    num += 40;
                }
                return score;
            }
        }

        private int GetRoundScore()//获得通过本关所需分数
        {
            int num = 1000;
            int round = Int32.Parse(label3.Text);
            for (int i = 1; i < round; i++)
            {
                num += 500;
            }
            return num;
        }

        private int column, row;

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        private int hbHeight, hbWidth;

        public int HbHeight
        {
            get { return hbHeight; }
            set { hbHeight = value; }
        }

        public int HbWidth
        {
            get { return hbWidth; }
            set { hbWidth = value; }
        }
    }
}
