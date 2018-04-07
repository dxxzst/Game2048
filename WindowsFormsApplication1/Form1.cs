using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private PictureBox[] cubes = new PictureBox[16];
        enum MoveDict { Up, Down, Left, Right };
        private int score = 0;//积分
        private int prevScore = 0;//积分
        private PictureBox[] prev = new PictureBox[16]; //上1步方格值和积分'

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Focus();
            cubes[0] = pictureBox1;
            cubes[1] = pictureBox2;
            cubes[2] = pictureBox3;
            cubes[3] = pictureBox4;
            cubes[4] = pictureBox5;
            cubes[5] = pictureBox6;
            cubes[6] = pictureBox7;
            cubes[7] = pictureBox8;
            cubes[8] = pictureBox9;
            cubes[9] = pictureBox10;
            cubes[10] = pictureBox11;
            cubes[11] = pictureBox12;
            cubes[12] = pictureBox13;
            cubes[13] = pictureBox14;
            cubes[14] = pictureBox15;
            cubes[15] = pictureBox16;
            start();
        }

        private void start()
        {
            score = 0;//初始积分为0
            lblScore.Text = "0";
            //4个角之一放一个数，其他位置放一个数
            System.Random r = new Random();
            int c1 = r.Next(1, 5);
            int c2 = c1;
           
            for (int i = 0; i < 16; i++)  //初始化
            {
                cubes[i].Image = imageList1.Images[0];
            }

            int n1 = r.Next(1, 100);
            //int n2 = r.Next(1, 100);
            switch (c1)
            {
                case 1:
                    cubes[0].Image = (n1 < 90 ? imageList1.Images[1] : imageList1.Images[2]);
                    do
                    {
                        c2 = r.Next(1, 17);
                    } while (c2 == 1);
                    break;
                case 2:
                    cubes[3].Image = (n1 < 90 ? imageList1.Images[1] : imageList1.Images[2]);
                    do
                    {
                        c2 = r.Next(1, 17);
                    } while (c2 == 4);
                    break;
                case 3:
                    cubes[12].Image = (n1 < 90 ? imageList1.Images[1] : imageList1.Images[2]);
                    do
                    {
                        c2 = r.Next(1, 17);
                    } while (c2 == 13);
                    break;
                case 4:
                    cubes[15].Image = (n1 < 90 ? imageList1.Images[1] : imageList1.Images[2]);
                    do
                    {
                        c2 = r.Next(1, 17);
                    } while (c2 == 16);
                    break;
            }
            cubes[c2 - 1].Image = (n1 < 90 ? imageList1.Images[1] : imageList1.Images[2]);
            SavePrevVaule();
        }

        /// <summary>
        /// 保存上一步
        /// </summary>
        private void SavePrevVaule()
        {
            for (int i = 0; i < 16; i++)
                prev[i] = cubes[i];
            prevScore = score;
        }

        /// <summary>
        /// 加载上一步值
        /// </summary>
        private void LoadPrevValue()
        {
            for (int i = 0; i < 16; i++)
                cubes[i].Image = prev[i].Image;
            score = prevScore;
            lblScore.Text = score.ToString();
        }

        /// <summary>
        /// 移动一次
        /// </summary>
        /// <param name="dict"></param>
        private void OneMove(MoveDict dict)
        {
            SavePrevVaule();
            switch (dict)
            {
                case MoveDict.Up:
                    score += MoveLine(pictureBox13, pictureBox9, pictureBox5, pictureBox1) + MoveLine(pictureBox14, pictureBox10, pictureBox6, pictureBox2) + MoveLine(pictureBox15, pictureBox11, pictureBox7, pictureBox3) + MoveLine(pictureBox16, pictureBox12, pictureBox8, pictureBox4);
                    break;
                case MoveDict.Down:
                    score += MoveLine(pictureBox1, pictureBox5, pictureBox9, pictureBox13) + MoveLine(pictureBox2, pictureBox6, pictureBox10, pictureBox14) + MoveLine(pictureBox3, pictureBox7, pictureBox11, pictureBox15) + MoveLine(pictureBox4, pictureBox8, pictureBox12, pictureBox16);
                    break;
                case MoveDict.Right:
                    score += MoveLine(pictureBox1, pictureBox2, pictureBox3, pictureBox4) + MoveLine(pictureBox5, pictureBox6, pictureBox7, pictureBox8) + MoveLine(pictureBox9, pictureBox10, pictureBox11, pictureBox12) + MoveLine(pictureBox13, pictureBox14, pictureBox15, pictureBox16);
                    break;
                case MoveDict.Left:
                    score += MoveLine(pictureBox4, pictureBox3, pictureBox2, pictureBox1) + MoveLine(pictureBox8, pictureBox7, pictureBox6, pictureBox5) + MoveLine(pictureBox12, pictureBox11, pictureBox10, pictureBox9) + MoveLine(pictureBox16, pictureBox15, pictureBox14, pictureBox13);
                    break;
            }
            lblScore.Text = score.ToString();  //显示总分
            getNextCube();//取下一个方块
        }

        /// <summary>
        /// 计算一行或一列得分
        /// </summary>
        /// <param name="cube1"></param>
        /// <param name="cube2"></param>
        /// <param name="cube3"></param>
        /// <param name="cube4"></param>
        /// <returns></returns>
        private int MoveLine(PictureBox cube1, PictureBox cube2, PictureBox cube3, PictureBox cube4)
        {
            int score = 0;
            if (EQimgs(cube1.Image, imageList1.Images[0]) && EQimgs(cube2.Image, imageList1.Images[0]) && EQimgs(cube3.Image, imageList1.Images[0]) && EQimgs(cube4.Image, imageList1.Images[0]))
            {
                return 0; 
            }

            PictureBox[] cubestemp = { cube1, cube2, cube3, cube4 };
            ArrayList iCubes = new ArrayList();   //去除0之后的方块

            for (int i = 0; i < cubestemp.Length; i++)
            {
                if (!EQimgs(cubestemp[i].Image, imageList1.Images[0]))
                {
                    iCubes.Add(cubestemp[i].Image);
                }
            }
            for (int i = 0; i < cubestemp.Length - iCubes.Count; i++) // 换后头上变0
            {
                cubestemp[i].Image = imageList1.Images[0];
            }
            for (int i = 0; i < iCubes.Count; i++)
            {
                cubestemp[i + cubestemp.Length - iCubes.Count].Image = (Image)iCubes[i];
            }

            if (!EQimgs(cubestemp[2].Image, imageList1.Images[0]))//如果等于0，说明该行只有一个非0的数，则不需要再移动和累加分值了。
            {
                if (EQimgs(cubestemp[3].Image, cubestemp[2].Image))
                {
                    score += 2 * (int)Math.Pow(2, check_in(cubestemp[3]));
                    cubestemp[3].Image = imageList1.Images[check_in(cubestemp[3]) + 1];
                    if (EQimgs(cubestemp[1].Image, cubestemp[0].Image) && !(EQimgs(cubestemp[1].Image, imageList1.Images[0])))
                    {
                        score += 2 * (int)Math.Pow(2, check_in(cubestemp[1]));
                        cubestemp[2].Image = imageList1.Images[check_in(cubestemp[1]) + 1];
                        cubestemp[1].Image = imageList1.Images[0];
                        cubestemp[0].Image = imageList1.Images[0];
                    }
                    else
                    {
                        cubestemp[2].Image = cubestemp[1].Image;
                        cubestemp[1].Image = cubestemp[0].Image; ;
                        cubestemp[0].Image = imageList1.Images[0];
                    }
                }
                else
                {
                    if (EQimgs(cubestemp[2].Image, cubestemp[1].Image))
                    {
                        score += 2 * (int)Math.Pow(2, check_in(cubestemp[2]));
                        cubestemp[2].Image = imageList1.Images[check_in(cubestemp[2]) + 1];
                        cubestemp[1].Image = cubestemp[0].Image; ;
                        cubestemp[0].Image = imageList1.Images[0];
                    }
                    else
                    {
                        if (EQimgs(cubestemp[1].Image, cubestemp[0].Image) && !(EQimgs(cubestemp[1].Image, imageList1.Images[0])))
                        {
                            score += 2 * (int)Math.Pow(2, check_in(cubestemp[1]));
                            cubestemp[1].Image = imageList1.Images[check_in(cubestemp[1]) + 1];
                            cubestemp[0].Image = imageList1.Images[0];
                        }
                    }
                }
            }
            return score;
        }

        /// <summary>
        /// list中位置确定
        /// </summary>
        /// <param name="pctemp"></param>
        /// <returns></returns>
        private int check_in(PictureBox pctemp)
        {
            for (int i =  0; i <= 13; ++i)
            {
                if (EQimgs(pctemp.Image, imageList1.Images[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 新增
        /// </summary>
        private void getNextCube()
        {
            ArrayList iCubes = new ArrayList();   //去除0之后的方块
            //找出全部为0的方块
            for (int i = 0; i < 16; i++)
            {
                if (EQimgs(cubes[i].Image, imageList1.Images[0]))
                {
                    iCubes.Add(i);
                }
            }
            if (iCubes.Count > 0)
            {
                //在所有为0的位置上随机增加一个数
                Random r = new Random();
                int temp = r.Next(0, 16);
                while (!EQimgs(cubes[temp].Image, imageList1.Images[0]))
                {
                    temp = r.Next(0, 16);
                }
                cubes[temp].Image = (r.Next(1, 100) < 90 ? imageList1.Images[1] : imageList1.Images[2]);

                //在所有为0的位置上随机增加一个数
                //System.Random r = new Random();
                //cubes[(int)iCubes[r.Next(1, iCubes.Count) - 1]].Image = (r.Next(1, 100) < 90 ? imageList1.Images[1] : imageList1.Images[2]);
            }
            else
            {
                //全部方格已填满，是否全部方向不能移动
                if (!CanMove())
                {
                    MessageBox.Show("珞珞真棒\\(^o^)/~!", "提示");
                }
            }
        }

        /// <summary>
        /// 能否移动判断
        /// </summary>
        /// <returns></returns>
        private bool CanMove()
        {
            //能左右移动吗？
            if (EQimgs(pictureBox1.Image , pictureBox2.Image) || EQimgs(pictureBox2.Image , pictureBox3.Image) || EQimgs(pictureBox3.Image , pictureBox4.Image) ||
               EQimgs( pictureBox5.Image , pictureBox6.Image) || EQimgs(pictureBox6.Image , pictureBox7.Image) || EQimgs(pictureBox7.Image , pictureBox8.Image) ||
                EQimgs(pictureBox9.Image , pictureBox10.Image) || EQimgs(pictureBox10.Image , pictureBox11.Image) || EQimgs(pictureBox11.Image , pictureBox12.Image) ||
                EQimgs(pictureBox13.Image , pictureBox14.Image) || EQimgs(pictureBox14.Image , pictureBox15.Image) || EQimgs(pictureBox15.Image , pictureBox16.Image)
                )
                return true;
            //能上下移动吗？
            if (EQimgs(pictureBox1.Image , pictureBox5.Image) || EQimgs(pictureBox5.Image , pictureBox9.Image) || EQimgs(pictureBox9.Image , pictureBox13.Image )||
               EQimgs( pictureBox2.Image , pictureBox6.Image) || EQimgs(pictureBox6.Image , pictureBox10.Image )|| EQimgs(pictureBox10.Image , pictureBox14.Image) ||
                EQimgs(pictureBox3.Image , pictureBox7.Image) || EQimgs(pictureBox7.Image , pictureBox11.Image )|| EQimgs(pictureBox11.Image , pictureBox15.Image )||
                EQimgs(pictureBox4.Image, pictureBox8.Image) || EQimgs(pictureBox8.Image, pictureBox12.Image) || EQimgs(pictureBox12.Image, pictureBox16.Image)
                )
                return true;
            return false;
        }
        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_start_Click(object sender, EventArgs e)
        {
            start();
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_replay_Click(object sender, EventArgs e)
        {
            LoadPrevValue();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                case Keys.NumPad8:
                case Keys.W:
                    OneMove(MoveDict.Up);
                    break;

                case Keys.Down:
                case Keys.NumPad2:
                case Keys.X:
                    OneMove(MoveDict.Down);
                    break;

                case Keys.Left:
                case Keys.NumPad4:
                case Keys.A:
                    OneMove(MoveDict.Left);
                    break;

                case Keys.Right:
                case Keys.NumPad6:
                case Keys.D:
                    OneMove(MoveDict.Right);
                    break;
            }
        }

        public bool EQimgs(Image image1, Image image2)
        {
            MemoryStream ms1 = new MemoryStream(); 
            MemoryStream ms2 = new MemoryStream(); 
            image1.Save(ms1, System.Drawing.Imaging.ImageFormat.Bmp);
            image2.Save(ms2, System.Drawing.Imaging.ImageFormat.Bmp); 
            byte[] im1 = ms1.GetBuffer();
            byte[] im2 = ms2.GetBuffer(); 
            if (im1.Length != im2.Length)
                return false; 
            else 
            { 
                for (int i = 0; i < im1.Length; i++)
                    if (im1[i] != im2[i])
                        return false; 
            } 
            return true;
        }
    }
}