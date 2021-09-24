using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.Text.RegularExpressions;

/* segment a = new segment(); segment b = new segment();
            a.processName = "a";b.processName = "a";a.segmentName = "a";
            b.segmentName = "a";a.Base = 1;b.Base = 1;a.limit = 1;b.limit = 1;
            if (a==b) { textBox2.Text = "yes"; }
            
     
            public static bool operator== (segment obj1, segment obj2)
            {
               

                return ((true)&&(true)&&(true)&&(true));
            }
            public static bool operator !=(segment obj1, segment obj2)
            {


                return (false);
            }
     
     */


namespace OS_memory_test0
{
   

    public partial class Form1 : Form
    {
        //global
        //global variables
        int memSize=0;
        segment search= new segment();
        SortedSet<string> processNames = new SortedSet<string>();
        bool error = new bool();
        // create  a linked list
        public static LinkedList<segment> memory = new LinkedList<segment>();
        LinkedList<segment>.Enumerator iterator = memory.GetEnumerator();
        //global functions
        void textWrite(string name, string data)
        {
            string path = @name + ".txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            StreamWriter text = new StreamWriter(path);
            text.Write("{0}\r\n", data);
            text.Close();
        }
        void memoryDraw()
        {
            //get drawData
            segment[] drawData = new segment[memory.Count]; int ji=0;
            foreach (segment se in memory)
            {
                drawData[ji] = se; 
                
                ji++;
            }
           
           
            //delete chart
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
                
            }
            chart1.ChartAreas[0].AxisY.CustomLabels.Clear();
            //chart1.Series[0].Label.Remove(0, 0);
            //chart1.Series[1].Label.Remove(0, 0);
            for (int i=0;i<100;i++) { chart1.Series[i].Label = ""; }
            //delete grid
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

            for (int i = 0; i < drawData.Length; i++)
            {
                //draw segment

                this.chart1.Series[i].Points.Add(drawData[drawData.Length-1-i].limit);


                


                //draw addresses
                CustomLabel endduration = new CustomLabel(memSize-(drawData[i].Base - 0.5), memSize-(drawData[ i].Base + 0.5), (drawData[ i].Base).ToString(), 0, LabelMarkStyle.None);
                chart1.ChartAreas[0].AxisY.CustomLabels.Add(endduration);

                

            }

            //draw last addresses
            CustomLabel lastaddress = new CustomLabel(memSize-(drawData[drawData.Length-1].Base + drawData[drawData.Length - 1].limit - 0.5),memSize- (drawData[drawData.Length-1].Base+ drawData[drawData.Length - 1].limit+ 0.5), (drawData[drawData.Length-1].Base + drawData[drawData.Length-1].limit).ToString(), 0, LabelMarkStyle.None);
            chart1.ChartAreas[0].AxisY.CustomLabels.Add(lastaddress);




            //hole white & reserved gray color
            for (int i = 0; i < drawData.Length; i++)
            {
              
                if (drawData[drawData.Length - 1 - i].processName == "Hole")
                {
                    chart1.Series[i].Points[0].Color = Color.WhiteSmoke;

                }

                if (drawData[drawData.Length - 1 - i].processName == "RESERVED")
                {
                    chart1.Series[i].Points[0].Color = Color.Gray;

                }
            }

            //draw names
            for (int i = drawData.Length - 1; i >= 0; i--)
            {
                if (drawData[drawData.Length - 1 - i].processName != "RESERVED" && drawData[drawData.Length - 1 - i].processName != "Hole")
                {
                    chart1.Series[i].Label = drawData[drawData.Length - 1 - i].processName + " : " + drawData[drawData.Length - 1 - i].segmentName;
                }
                else
                { chart1.Series[i].Label = drawData[drawData.Length - 1 - i].processName; }
            }


            //give every process a color
            int checkname(string name)
            {
                int i = 0;
                foreach(string se in processNames)
                {
                   
                    if (name == se)
                    {
                        return i;
                    } 
                    i++;
                }
                return -1;
            }


            //randomm color to every point
            for (int i = 0; i < drawData.Length; i++)
            {
                // k is number of random colors
                int k = checkname(drawData[drawData.Length - 1 - i].processName) % 10;
                
                if (k == 0)
                {
                    chart1.Series[i].Points[0].Color = Color.Gold;

                }

                if (k == 1)
                {
                    chart1.Series[i].Points[0].Color = Color.Red;
                }

                if (k == 2)
                {
                    chart1.Series[i].Points[0].Color = Color.Green;
                }

                if (k == 3)
                {
                    chart1.Series[i].Points[0].Color = Color.Blue;
                }

                if (k == 4)
                {
                    chart1.Series[i].Points[0].Color = Color.Orange;
                }
                if (k == 5)
                {
                    chart1.Series[i].Points[0].Color = Color.Cyan;
                }
                if (k == 6)
                {
                    chart1.Series[i].Points[0].Color = Color.Magenta;
                }
                if (k == 7)
                {
                    chart1.Series[i].Points[0].Color = Color.DarkBlue;
                }
                if (k == 8)
                {
                    chart1.Series[i].Points[0].Color = Color.Brown;
                }
                if (k == 9)
                {
                    chart1.Series[i].Points[0].Color = Color.Aqua;
                }


            }

        }

        void tableDraw()
        {
            //clear table
            dataGridView1.Rows.Clear();
            //draw 3 columns
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Address";
            dataGridView1.Columns[2].Name = "Limit";
            //get drawData
            segment[] drawData = new segment[memory.Count]; int ji = 0;
            foreach (segment se in memory)
            {
                drawData[ji] = se;

                ji++;
            }
            //draw table
            for (int i=0;i< drawData.Length; i++) 
            {
                if (drawData[i].processName!="RESERVED"&& drawData[i].processName != "Hole") 
                {
                    string[] row = { drawData[i].processName + " : " + drawData[i].segmentName, drawData[i].Base.ToString(), drawData[i].limit.ToString() };
                    dataGridView1.Rows.Add(row);
                }

            }


            //give every process a color
            int checkname(string name)
            {
                int i = 0;
                foreach (string se in processNames)
                {

                    if (name == se)
                    {
                        return i;
                    }
                    i++;
                }
                return -1;
            }
            int c = -1;
            //randomm color to every point
            for (int i = 0; i < drawData.Length; i++)
            {
                // k is number of random colors
                int k=-1;


                if ((drawData[i].processName != "Hole") && (drawData[i].processName != "RESERVED"))
                { k = checkname(drawData[i].processName) % 10; c++; }

                else { continue; }
                if (k == 0)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Gold;

                }

                if (k == 1)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Red;
                }

                if (k == 2)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Green;
                }

                if (k == 3)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Blue;
                }

                if (k == 4)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Orange;
                }
                if (k == 5)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Cyan;
                }
                if (k == 6)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Magenta;
                }
                if (k == 7)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.DarkBlue;
                }
                if (k == 8)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Brown;
                }
                if (k == 9)
                {
                    dataGridView1.Rows[c].DefaultCellStyle.BackColor = Color.Aqua;
                }


            }


        }
        //merge holes
        void holeMerge()
        {
            segment first = new segment();
            segment second = new segment();
            segment total = new segment();
            
                first.Base = -1; first.limit = -1;
            for (int j = 0; j < memory.Count; j++)
            {

                //find and store first
                foreach (segment se in memory)
                {
                    if (se.processName == "Hole")
                    {
                        if ((se.Base > first.Base))
                        {

                            first = se; break;
                        }
                    }
                }

                foreach (segment se in memory)
                {
                    if (se.processName == "Hole")
                    {


                        if ((se.Base != first.Base) && (se.Base == (first.Base + first.limit)))
                        {
                            second = se;
                            LinkedListNode<segment> foundFirst = memory.Find(second);
                            LinkedListNode<segment> foundSecond = memory.Find(first);
                            total = first + second;
                            memory.AddAfter(foundFirst, total);
                            memory.Remove(foundFirst);
                            memory.Remove(foundSecond);


                            break;
                        }
                    }
                }

            

        }
        }
        public Form1()
        {
            InitializeComponent();
        }
        //create a new class
        public class segment 
        {
            public int Base = 0;
            public int limit = 0;
            public string processName="";
            public string segmentName="";
           public segment()
            {  
              Base = 0;
              limit = 0;
              processName = "";
              segmentName = "";
        }
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is segment))
                { return false; }
                segment other = obj as segment;
                return ((this.Base==other.Base)&&(this.limit == other.limit) &&(this.processName == other.processName) &&(this.segmentName == other.segmentName) );
            
            }
            public static  segment operator +(segment a1, segment a2)
            {
                segment result= new segment();
                result.Base = a1.Base;
                result.limit = a1.limit + a2.limit;
                result.processName = a1.processName;
                result.segmentName = a1.segmentName;
            return result;
            
            }

            public static segment operator -(segment a1, segment a2)
            {
                segment result = new segment();
                result.Base = a2.Base+a2.limit ;
                result.limit = a1.limit - a2.limit;
                result.processName = a1.processName;
                result.segmentName = a1.segmentName;
                return result;

            }


        }
       
        



        private void button1_Click(object sender, EventArgs e)
        {



           
            //ok button only @ code beginning
            if ((button1.Text == "OK")&&(richTextBox1.Text!=""))
            {
                //show
                label4.Visible = true;
                chart1.Visible = true;
                button1.Text = "Reset";
                //write initial memory shape
                textWrite("Hole Address", richTextBox1.Text);
                textWrite("Hole Size",richTextBox2.Text);
                string[] initialAddress = File.ReadAllLines("Hole Address.txt");
                string[] initialSize = File.ReadAllLines("Hole Size.txt");

                //bubble sort holes
                /*
                string swap;
                for (int i = 0; i < initialAddress.Length; i++)
                {


                    for (int j = i; j < initialAddress.Length; j++)
                    {
                        if (Int32.Parse(initialAddress[i])>Int32.Parse(initialAddress[j]))
                        {
                            swap = initialAddress[i]; initialAddress[i] = initialAddress[j]; initialAddress[j] = swap;
                            swap = initialSize[i]; initialSize[i] = initialSize[j]; initialSize[j] = swap;
                            
                        }
                    }



                }
                */
                //store initial shape in linked list
                //store holes
                segment[] initialHole = new segment[initialAddress.Length] ;
                
                for (int i=0;i< initialAddress.Length; i++)
                {
                    initialHole[i] = new segment();

                    initialHole[i].processName = "Hole";
                    initialHole[i].segmentName = "Hole";
                    initialHole[i].Base =Int32.Parse(initialAddress[i]);
                    initialHole[i].limit = Int32.Parse(initialSize[i]);
                    if (i == 0) { memory.AddFirst(initialHole[i]); }
                    else { memory.AddAfter(memory.Last, initialHole[i]); }
                    
                }

                //store reserved segments
                segment[] initialSeg = new segment[initialAddress.Length+1];
                
                for (int i = 0; i < initialAddress.Length; i++)
                {
                    initialSeg[i] = new segment();
                    //first reserved seg
                    if (i == 0)
                    {
                        if (Int32.Parse(initialAddress[i]) != 0)
                        {
                            
                            initialSeg[i].processName = "RESERVED";
                            initialSeg[i].segmentName = "RESERVED";
                            initialSeg[i].Base = 0; initialSeg[i].limit = Int32.Parse(initialAddress[i]);
                            memory.AddFirst(initialSeg[i]);
                        }
                    }
                    //medium reserved seg
                    else 
                    {

                        initialSeg[i].processName = "RESERVED";
                        initialSeg[i].segmentName = "RESERVED";
                        initialSeg[i].Base = Int32.Parse(initialAddress[i - 1]) + Int32.Parse(initialSize[i - 1]);
                        initialSeg[i].limit = Int32.Parse(initialAddress[i]) - initialSeg[i].Base;
                        search.Base = Int32.Parse(initialAddress[i]); search.limit = Int32.Parse(initialSize[i]); search.processName = "Hole"; search.segmentName = "Hole";
                        if(initialSeg[i].limit !=0)
                        { LinkedListNode<segment> found = memory.Find(search); memory.AddBefore(found, initialSeg[i]);  }
                        //foreach (segment se in memory) { if (se == initialSeg) {  } }
                        //foreach (LinkedListNode<segment> se in memory) {; }
                   
                       
                    }
                    //last reserved seg
                    if (i == (initialAddress.Length - 1))
                    {
                        if (Int32.Parse(initialAddress[i]) + Int32.Parse(initialSize[i]) < memSize)
                        {
                            initialSeg[i + 1] = new segment();
                            initialSeg[i + 1].processName = "RESERVED";
                            initialSeg[i + 1].segmentName = "RESERVED";
                            initialSeg[i + 1].Base = Int32.Parse(initialAddress[i]) + Int32.Parse(initialSize[i]);
                            initialSeg[i + 1].limit = memSize - initialSeg[i + 1].Base;
                            memory.AddLast(initialSeg[i + 1]);

                        }
                    }

                }
                //merge all holes
                for (int j=0;j<memory.Count;j++) { holeMerge(); }
                
                //draw that shape in chart1
                memoryDraw();
                tableDraw();
            }

           // reset button only
            else if (button1.Text=="Reset")
            {
                //hide chart
                label4.Visible = false;
                chart1.Visible = false;
                button1.Text = "OK";
                //clear linked list
                memory.Clear();
                //clear table
                dataGridView1.Rows.Clear();
                //delete all
                textBox2.Text = "";richTextBox1.Text = ""; richTextBox2.Text = ""; richTextBox3.Text = ""; richTextBox4.Text = "";numericUpDown1.Value = 0; numericUpDown2.Value = 0;comboBox1.Text = ""; checkBox2.Checked = false; checkBox3.Checked = false;
            }
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dataGridView1.Visible = true;
            }
            else
            { dataGridView1.Visible = false; }
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked==true ) { checkBox2.Checked = false; }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) { checkBox3.Checked = false; }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //take process & segments data 
            textWrite("Segment Name", richTextBox4.Text);
            textWrite("Segment Size", richTextBox3.Text);

            //read segment name and size
            int noOfSegments = Decimal.ToInt32(numericUpDown1.Value);
            string[] segmentName = File.ReadAllLines("Segment Name.txt");
            string[] SegmentSize = File.ReadAllLines("Segment Size.txt");
            segment[] addedSeg = new segment[noOfSegments];
            

            //add process name to set
            if (textBox2.Text!="") { processNames.Add(textBox2.Text); }
            //delete comboBox then add set
            comboBox1.Items.Clear();
            foreach (string p in processNames)
            { comboBox1.Items.Add(p); }

            bool populated =new bool();
            error = false;
            //first fit
            if (checkBox3.Checked==true)
            {
                //loop number of segments
                for (int i=0;i<noOfSegments;i++)
                {
                    addedSeg[i] = new segment();
                    addedSeg[i].processName = textBox2.Text;
                    addedSeg[i].segmentName = segmentName[i];
                    addedSeg[i].limit = Int32.Parse(SegmentSize[i]);


                    populated = false;
                    //loop on all memory list
                    foreach (segment se in memory)
                    {
                        //if enought hole found remove it
                        if ((se.processName == "Hole")&&(se.limit>=Int32.Parse(SegmentSize[i]))) 
                        {
                            addedSeg[i].Base = se.Base;
                            //find the first hole
                            LinkedListNode<segment> found = memory.Find(se);
                            //add seg
                            memory.AddBefore(found, addedSeg[i]);
                            //add remaining hole if any
                            if ((se.limit - addedSeg[i].limit)!=0)
                            { memory.AddAfter(found, se - addedSeg[i]); }
                            
                            memory.Remove(found);
                            populated = true;
                            break;
                        }
                        
                    }
                    for (int j = 0; j < memory.Count; j++) { holeMerge(); }
                    if (!populated) { MessageBox.Show("Error not enough memory space"); error = true; break; }
                }

                
            
            }
            //best fit
            if (checkBox2.Checked == true)
            {
                //loop number of segments
                for (int i = 0; i < noOfSegments; i++)
                {
                    addedSeg[i] = new segment();
                    addedSeg[i].processName = textBox2.Text;
                    addedSeg[i].segmentName = segmentName[i];
                    addedSeg[i].limit = Int32.Parse(SegmentSize[i]);
                    segment min = new segment();
                    min.limit = memSize;

                    populated = false;
                    //loop on all memory list
                    foreach (segment se in memory)
                    {
                        //if enought hole found remove it
                        if ((se.processName == "Hole") && (se.limit >= Int32.Parse(SegmentSize[i])))
                        {
                            if (se.limit < min.limit)
                            { min = se; populated = true; }

                        }

                    }
                    //find the first hole

                    LinkedListNode<segment> found = memory.Find(min);
                    try 
                    {


                        //add seg
                        addedSeg[i].Base = min.Base;
                        memory.AddBefore(found, addedSeg[i]);
                    //add remaining hole if any
                    if ((min.limit - addedSeg[i].limit) != 0)
                    { memory.AddAfter(found, min - addedSeg[i]); }

                    memory.Remove(found);
                     for (int j = 0; j < memory.Count; j++) { holeMerge(); }
                    }
                    catch {; }

                    if (!populated) { MessageBox.Show("Error : not enough memory space !"); error = true; break; }

                }
               

            }
            //if no onough space remove this process
            if (error) { comboBox1.SelectedItem = textBox2.Text; button3_Click(sender, e); }
            memoryDraw();
            tableDraw();

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            memSize = Decimal.ToInt32(numericUpDown2.Value);
             
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                segment[] addedSeg = new segment[memory.Count * 3];
                segment deallocate = new segment();
                deallocate.processName = comboBox1.SelectedItem.ToString();
                for (int i = 0; i < memory.Count * 3; i++)
                {
                    foreach (segment se in memory)
                    {
                        if (se.processName == deallocate.processName)
                        {
                            addedSeg[i] = new segment();
                            LinkedListNode<segment> found = memory.Find(se);
                            addedSeg[i].processName = "Hole";
                            addedSeg[i].segmentName = "Hole";
                            addedSeg[i].Base = se.Base;
                            addedSeg[i].limit = se.limit;



                            memory.AddAfter(found, addedSeg[i]);
                            memory.Remove(found);
                            for (int jj = 0; jj < memory.Count; jj++) { holeMerge(); }
                            break;
                        }
                    }
                }
                
            
            //remove selected item
            processNames.Remove(comboBox1.SelectedItem.ToString());
            comboBox1.Text = "";
            comboBox1.Items.Remove(comboBox1.SelectedItem.ToString());
            for (int jj = 0; jj < memory.Count; jj++) { holeMerge(); }
            //draw
            memoryDraw();
            tableDraw();
            }
            catch {; }
        }
    }
}
