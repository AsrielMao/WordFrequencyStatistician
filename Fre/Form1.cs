using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordFrequencyStatistician
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_word.Text.Trim().Length == 0 || dataGridView1.RowCount - 1 < 1)
            {
                return;
            }
            //清空当前选择
            dataGridView1.ClearSelection();

            //查找
            string word = txt_word.Text.Trim();
            bool founded = false;
            int rowCount = dataGridView1.RowCount - 1;
            for (int i = 0; i < rowCount; i++)
            {
                if(word == dataGridView1.Rows[i].Cells[0].Value.ToString())
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.Rows[i].Selected = true;
                    founded = true;
                    break;
                }
            }

            if (!founded)
            {
                MessageBox.Show("当前文章不存在这个词！");
            }
        }

        private void btn_statistics_Click(object sender, EventArgs e)
        {
            if (txt_article.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入文本！");
                return;
            }

            //清空列表内原有的内容
            dataGridView1.Rows.Clear();

            //处理输入的文章，将标点符号去掉，然后以空格分隔文章为单词组存入数组words中
            string article = txt_article.Text.ToLower();

            article = Regex.Replace(article, @"\W+", " ");
            article = Regex.Replace(article, "\r\n", " ");
            article = article.Trim();

            string[] words = Regex.Split(article, " ");

            //对words中的每一个词，查看是否已经存在于列表中，并计数
            int word_total = 0;
            foreach(string word in words)
            {
                word_total++;
                int rc = dataGridView1.RowCount - 1;
                bool had_added = false;
                for(int i = 0; i < rc; i++)
                {
                    if(word == dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        int value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                        dataGridView1.Rows[i].Cells[1].Value = value + 1;
                        had_added = true;
                        break;
                    }
                }
                if (had_added)
                {
                    continue;
                }

                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = word;
                dataGridView1.Rows[index].Cells[1].Value = 1;
                
            }

            //计算频率
            int rowCount = dataGridView1.RowCount - 1;
            for(int i = 0; i < rowCount; i++)
            {
                double count = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                dataGridView1.Rows[i].Cells[2].Value = (count / word_total * 100).ToString("0.00") + "%";
            }

            //显示总词数
            label_result.Text = "本篇文章共有单词 " + word_total + " 个。"; ;
        }
    }
}
