using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string path_to_XML = Directory.GetCurrentDirectory() + "\\file.xml";

        Random rand = new Random();

        PhraseAndPicture[] array_of_Cooper = {
        new PhraseAndPicture("Вау, что ты делаешь? Так интересно!", "1493.png"), //0 заинтересованность
        new PhraseAndPicture("Как здорово! А меня научишь так классно алгоритмы строить?", "1489" + ".png"), //1 заинтересованность
        new PhraseAndPicture("Какие-то странные формулы. Мне непонятно!", "1485" + ".png"), //2 заинтересованность
        new PhraseAndPicture("Мне скучно. Давай лучше поиграем","1496" + ".png"), //3 скука
        new PhraseAndPicture("Мне скучно. Давай лучше погуляем","1491" + ".png"), //4 скука
        new PhraseAndPicture("Пока программа считает, можно и поспать...", "1505" + ".png"), //5 сон (ожидание)
        new PhraseAndPicture("Выполняю алгоритм быстро как супермен! ", "1513" + ".png"), //6 работа программы
        new PhraseAndPicture("Ухх, сложную задачу ты мне дал. Хорошо, что у щенят тоже есть учебники", "1488" + ".png"), //7 работа программы
        new PhraseAndPicture("Ура-ура, я всё посчитал. Я хороший мальчик?","1508" + ".png"),//8 успех
        new PhraseAndPicture("А я всё сделал! Я точно заслужил косточку","1495" + ".png"),//9 успех
        new PhraseAndPicture("Я не сумел посчитать... Возможно, этот алгоритм неприменим к этому слову...", "1500" + ".png"),//10 неудача
        new PhraseAndPicture("Спасибо!","1494" + ".png"),//11 награда
        new PhraseAndPicture("В инструкции написано, что пользователь может вводить не все данные. Но написать слово и количество итераций надо!","1488" + ".png")//12 чтение инструкции

                                             };
        /// <summary>
        /// Выбор эмоции щенка
        /// </summary>
        /// <param name="index">Номер эмоции</param>
        public void ChangeCooper(int index)
        {
            label9.Text = array_of_Cooper[index].phrase;
            pictureBox1.Image = Image.FromFile(array_of_Cooper[index].picture_path);
        }

        public void ChangeProgressBox(string message, bool clear_all = false)
        {
            if (clear_all)
                richTextBox1.Text = message;
            else
                richTextBox1.Text += message;
        }


        public void SaveToXMLFile()
        {

            try
            {
                DataSet ds = new DataSet(); // создаем пока что пустой кэш данных
                DataTable dt = new DataTable(); // создаем пока что пустую таблицу данных
                dt.TableName = "Алгоритмы"; // название таблицы
                dt.Columns.Add("Название"); // название колонок
                dt.Columns.Add("Алгоритм");
                ds.Tables.Add(dt); //в ds создается таблица, с названием и колонками, созданными выше

                foreach (DataGridViewRow r in dataGridView1.Rows) // пока в dataGridView1 есть строки
                {
                    DataRow row = ds.Tables["Алгоритмы"].NewRow(); // создаем новую строку в таблице, занесенной в ds
                    row["Название"] = r.Cells[0].Value;  //в столбец этой строки заносим данные из первого столбца dataGridView1
                    row["Алгоритм"] = r.Cells[1].Value; // то же самое со вторыми столбцами
                    ds.Tables["Алгоритмы"].Rows.Add(row); //добавление всей этой строки в таблицу ds.
                }
                // MessageBox.Show(System.IO.Directory.GetCurrentDirectory());

                using (StreamWriter sw = new StreamWriter(path_to_XML, true))
                {

                }

                ds.WriteXml(path_to_XML);

                //MessageBox.Show("XML файл успешно сохранен", "Выполнено");
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        public string AlgorythmCalculation(string[] algorythm, string word, int count_of_iteration)
        {
            Command[] alg = new Command[algorythm.Length];
            for (int i = 0; i < alg.Length; i++)
            {
                alg[i] = new Command();

                string buff_p = ""; string buff_q = ""; bool buff_final = false;
                if (algorythm[i].IndexOf("•") != -1)
                {
                    buff_final = true;
                }

                algorythm[i] = algorythm[i].Replace("•", "");

                buff_p = algorythm[i].Substring(0, algorythm[i].IndexOf("→"));
                buff_q = algorythm[i].Substring(algorythm[i].IndexOf("→") + 1);

                buff_p = buff_p.Trim(); buff_q = buff_q.Trim();

                alg[i].p = buff_p;
                alg[i].q = buff_q;
                alg[i].final = buff_final;
            }

            return AlgorythmCalculation(alg, word, count_of_iteration);
        }

        public string AlgorythmCalculation(Command[] algorythm, string word, int count_of_iteration)
        {
            string _word = word;
            Command[] commands = algorythm;
            int index = count_of_iteration;

            while (index > 0)
            {
                string initial_word = _word; //Если слово после ВСЕХ операций не изменится - цикл завершится

                for (int j = 0; j < commands.Count(); j++)
                {
                    string buff_word = _word.Replace(commands[j].p, commands[j].q);

                    if (commands[j].final == false)
                    {
                        if (buff_word == _word)
                            continue;
                        else
                        {
                            _word = buff_word;                            
                            ChangeProgressBox(commands[j].p + " → " + commands[j].q + " Результат: " + _word + Environment.NewLine);
                        }
                    }
                    else
                    {
                        if (buff_word == _word)
                            continue;
                        else
                        {
                            _word = buff_word;                            
                            ChangeProgressBox("Заключительная подстановка: " + commands[j].p + " → " + commands[j].q + " Результат: " + _word + Environment.NewLine);
                            return _word;
                        }
                    }
                }

                if (initial_word == _word)
                    break;

                index--;
            }

            if (index == 0)
            {
                ChangeProgressBox("Вычисление результата заняло " + count_of_iteration + " итераций. Возможно, последовательность шагов преобразования слова бесконечная, то есть, алгоритм является неприменимым к данному слову");
                ChangeCooper(10);
            }
            else
                ChangeCooper(9);
            return _word;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                listBox1.Items.Add(textBox1.Text + " → • " + textBox2.Text);
            }
            else
            {
                listBox1.Items.Add(textBox1.Text + " → " + textBox2.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button9.Visible = true;

            try
            {
                if (listBox1.SelectedItem.ToString().IndexOf("•") == -1)
                {
                    textBox1.Text = listBox1.SelectedItem.ToString().Replace(" ", string.Empty).Substring(0, listBox1.SelectedItem.ToString().Replace(" ", string.Empty).IndexOf("→"));
                    textBox2.Text = listBox1.SelectedItem.ToString().Replace(" ", string.Empty).Substring(listBox1.SelectedItem.ToString().Replace(" ", string.Empty).IndexOf("→") + 1);
                }
                else
                {
                    checkBox1.Checked = true;
                    textBox1.Text = listBox1.SelectedItem.ToString().Replace(" ", string.Empty).Replace("•", string.Empty).Substring(0, listBox1.SelectedItem.ToString().Replace(" ", string.Empty).Replace("•", string.Empty).IndexOf("→"));
                    textBox2.Text = listBox1.SelectedItem.ToString().Replace(" ", string.Empty).Replace("•", string.Empty).Substring(listBox1.SelectedItem.ToString().Replace(" ", string.Empty).Replace("•", string.Empty).IndexOf("→") + 1);
                }
                buff_index = listBox1.SelectedIndex;
            }
            catch (Exception e1)
            {
           //     MessageBox.Show(e1.ToString() + Environment.NewLine + "Вы не выбрали подстановку для редактирования");
            }

        }

        public int buff_index;

        private void button9_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button9.Visible = false;

            listBox1.Items.Remove(listBox1.SelectedItem);

            if (checkBox1.Checked)
            {
                listBox1.Items.Insert(buff_index, textBox1.Text + " → • " + textBox2.Text);
            }
            else
            {
                listBox1.Items.Insert(buff_index, textBox1.Text + " → " + textBox2.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ChangeCooper(11);
        }

        private void listBox1_MouseEnter(object sender, EventArgs e)
        {
            ChangeCooper(0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                MessageBox.Show("Введите название алгоритма", "Ошибка");
            }
            else
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = textBox8.Text; // столбец Name
                string[] alg = new string[listBox1.Items.Count];
                int i = 0;
                foreach (var s in listBox1.Items)
                {
                    alg[i] = s.ToString() + "\n";
                    i++;
                }

                dataGridView1.Rows[n].Cells[1].Value = String.Join("", alg);
            }
            SaveToXMLFile();
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            ChangeCooper(2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(path_to_XML)) // если существует данный файл
            {
                DataSet ds = new DataSet(); // создаем новый пустой кэш данных
                ds.ReadXml(path_to_XML); // записываем в него XML-данные из файла

                foreach (DataRow item in ds.Tables["Алгоритмы"].Rows)
                {
                    int n = dataGridView1.Rows.Add(); // добавляем новую сроку в dataGridView1
                    dataGridView1.Rows[n].Cells[0].Value = item["Название"]; // заносим в первый столбец созданной строки данные из первого столбца таблицы ds.
                    dataGridView1.Rows[n].Cells[1].Value = item["Алгоритм"]; // то же самое со вторым столбцом
                }
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_MouseEnter(object sender, EventArgs e)
        {
            ChangeCooper(1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCellAddress.Y); //удаление
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления", "Ошибка");
            }
            SaveToXMLFile();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SaveToXMLFile();
        }

        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {
            ChangeCooper(1);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCellAddress.Y;

            listBox1.Items.Clear();

            string str = dataGridView1.Rows[index].Cells[1].Value.ToString();

            while (str.IndexOf("\n") != -1)
            {
                listBox1.Items.Add(str.Substring(0, str.IndexOf("\n")));
                str = str.Substring(str.IndexOf("\n") + 1);
            }

            textBox8.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ChangeCooper(rand.Next(5, 7));
            ChangeProgressBox("", true);

            textBox4.Text = "Λ" + textBox4.Text + "Λ";

            string[] alg = new string[listBox1.Items.Count];
            int i = 0;
            foreach (var s in listBox1.Items)
            {
                alg[i] = s.ToString();
                i++;
            }

            if (textBox4.Text == "" && textBox5.Text == "")
            {
                MessageBox.Show("Введите параметры", "Ошибка");
            }
            else
            {
                try
                {
                    textBox7.Text = AlgorythmCalculation(alg, textBox4.Text, Convert.ToInt32(textBox5.Text)).Trim('Λ');
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }


        }

        private void textBox3_MouseHover(object sender, EventArgs e)
        {
            ChangeCooper(12);
        }

        private void textBox6_MouseHover(object sender, EventArgs e)
        {
            ChangeCooper(12);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Λ";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox2.Text += "Λ";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox4.Text += "Λ";
        }



    }

    public class PhraseAndPicture
    {
        string _phrase;
        string _picture_path = System.IO.Directory.GetCurrentDirectory() + "\\512\\";

        public PhraseAndPicture(string phrase, string path)
        {
            _phrase = phrase;
            _picture_path = _picture_path + path;
        }

        public string phrase
        {
            get { return _phrase; }
            set { this._phrase = phrase; }
        }

        public string picture_path
        {
            get { return _picture_path; }
            private set { this._picture_path = picture_path; }
        }
    }

    public class Command
    {
        public string p;
        public string q;
        public bool final;
    }

}
