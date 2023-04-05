using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace FootballManager
{
    public partial class PlayerHeadForm : Form
    {
        SoccerContext db;
        public PlayerHeadForm()
        {
            InitializeComponent();

            db = new SoccerContext();
            db.Players.Load();
            dataGridView1.DataSource = db.Players.Local.ToBindingList();
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {

        }

        //ADD
        private void button1_Click(object sender, EventArgs e)
        {
            PlayerForm playerForm = new PlayerForm();

            List<Team> teams = db.Teams.ToList();
            playerForm.comboBox2.DataSource = teams;
            playerForm.comboBox2.ValueMember = "Id";
            playerForm.comboBox2.DisplayMember = "Name";

            DialogResult result = playerForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Player player = new Player();
            player.Name = playerForm.textBox1.Text;
            player.Position = playerForm.comboBox1.SelectedItem.ToString();
            player.Age = (int)playerForm.numericUpDown1.Value;
            player.Team = (Team)playerForm.comboBox2.SelectedItem;

            db.Players.Add(player);
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

            Player player = db.Players.Find(id);

            PlayerForm playerForm = new PlayerForm();
            playerForm.textBox1.Text = player.Name;
            playerForm.comboBox1.SelectedItem = player.Position;
            playerForm.numericUpDown1.Value = player.Age;

            List<Team> teams = db.Teams.ToList();
            playerForm.comboBox2.DataSource = teams;
            playerForm.comboBox2.ValueMember = "Id";
            playerForm.comboBox2.DisplayMember = "Name";

            if (player.Team != null)
                playerForm.comboBox2.SelectedValue = player.Team.Id;

            DialogResult result = playerForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            player.Age = (int)playerForm.numericUpDown1.Value;
            player.Name = playerForm.textBox1.Text;
            player.Position = playerForm.comboBox1.SelectedItem.ToString();
            player.Team = (Team)playerForm.comboBox2.SelectedItem;

            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            dataGridView1.Refresh();
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

            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
        }

        //OPEN TeamFormHead
        private void button4_Click(object sender, EventArgs e)
        {
            TeamHeadForm team = new TeamHeadForm();
            team.Show();
        }    
        
    }
}
