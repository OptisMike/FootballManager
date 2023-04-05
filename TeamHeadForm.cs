using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballManager
{
    public partial class TeamHeadForm : Form
    {
        SoccerContext db;
        public TeamHeadForm()
        {
            InitializeComponent();

            db = new SoccerContext();
            db.Teams.Load();
            dataGridView1.DataSource = db.Teams.Local.ToBindingList();
        }

        //ADD
        private void button1_Click(object sender, EventArgs e)
        {
            TeamForm teamForm = new TeamForm();
            DialogResult result = teamForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Team team = new Team();
            team.Name = teamForm.textBox1.Text;
            team.Coach = teamForm.textBox2.Text;

            db.Teams.Add(team);
            db.SaveChanges();
        }

        //EDIT
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            bool isConverted = int.TryParse(dataGridView1[0, rowIndex].Value.ToString(), out int id);

            if (!isConverted)
                return;

            Team team = db.Teams.Find(id);

            TeamForm teamForm = new TeamForm();
            teamForm.textBox1.Text = team.Name;
            teamForm.textBox2.Text = team.Coach;

            DialogResult result = teamForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            team.Name = teamForm.textBox1.Text;
            team.Coach = teamForm.textBox2.Text;

            db.Entry(team).State = EntityState.Modified;
            db.SaveChanges();
            dataGridView1.Refresh();
        }

        //VIEW players team list
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            bool isConverted = int.TryParse(dataGridView1[0, rowIndex].Value.ToString(), out int id);

            if (!isConverted)
                return;

            Team team = db.Teams.Find(id);
            listBox1.DataSource = team.Players.ToList();
            listBox1.DisplayMember = "Name";
        }
        //REMOVE
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            bool isConverted = int.TryParse(dataGridView1[0, rowIndex].Value.ToString(), out int id);

            if (!isConverted)
                return;

            Team team = db.Teams.Find(id);

            foreach (var player in team.Players.ToList())
            {
                player.Team = null;
                db.Entry(player).State = EntityState.Modified;
            }
            
            team.Players.Clear();
            db.Teams.Remove(team);
            db.SaveChanges();
        }

        private void TeamHeadForm_Load(object sender, EventArgs e)
        {

        }
    }
}
